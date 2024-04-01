using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using CryptographingElectronicVotingSystem.Dal.Models.Authentication;
using Radzen;


namespace CryptographingElectronicVotingSystem.Web.Services
{
    public partial class SecurityService
    {

        private readonly HttpClient httpClient;

        private readonly Uri baseUri;

        private readonly NavigationManager navigationManager;

        public ApplicationUser User { get; private set; } = new ApplicationUser { Name = "Anonymous" };

        public ClaimsPrincipal Principal { get; private set; }


        private bool IsDevelopment()
        {
#if DEBUG
            return true;
#else
            return false;
#endif
        }
        public SecurityService(NavigationManager navigationManager, IHttpClientFactory factory)
        {
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/Identity/");
            this.httpClient = factory.CreateClient("CryptographingElectronicVotingSystem.Web");
            this.navigationManager = navigationManager;
        }

        public bool IsInRole(params string[] roles)
        {
#if DEBUG
            if (User.Name == "admin")
            {
                return true;
            }
#endif

            if (roles.Contains("Everybody"))
            {
                return true;
            }

            if (!IsAuthenticated())
            {
                return false;
            }

            if (roles.Contains("Authenticated"))
            {
                return true;
            }

            return roles.Any(role => Principal.IsInRole(role));
        }

        public bool IsAuthenticated()
        {
            return Principal?.Identity.IsAuthenticated == true;
        }

        public async Task<bool> InitializeAsync(AuthenticationState result)
        {
            try
            {
                Principal = result.User;
#if DEBUG
                if (Principal.Identity.Name == "admin")
                {
                    User = new ApplicationUser { Name = "Admin" };
                    return true;
                }
#endif
                var userId = Principal?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != null && User?.Id != userId)
                {
                    User = await GetUserById(userId);
                    if (User != null && Tenant == null)
                    {
                        if (IsDevelopment() && (User.Name == "admin" || User.Name == "tenantsadmin"))
                        {
                            var tenants = await GetTenants();
                            if (tenants.Any())
                            {
                                Tenant = tenants.FirstOrDefault();
                            }
                        }
                        else if (User.TenantId != null)
                        {
                            User.ApplicationTenant = await GetTenantById(User.TenantId.Value);
                            Tenant = User.ApplicationTenant;
                        }
                    }
                }
                return IsAuthenticated();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                // For example: LogError($"An error occurred while initializing the authentication state: {ex.Message}");
                return false;
            }
        }


        public ApplicationTenant Tenant { get; set; }

        public async Task<ApplicationAuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var uri = new Uri($"{navigationManager.BaseUri}Account/CurrentUser");
                var response = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, uri));

                if (!response.IsSuccessStatusCode)
                {
                    // Log the error or throw a more specific exception
                    // Example: LogWarning($"Failed to retrieve the current user. Status code: {response.StatusCode}");
                    return new ApplicationAuthenticationState { IsAuthenticated = false };
                }

                return await response.ReadAsync<ApplicationAuthenticationState>();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                // For example: LogError($"Exception occurred while getting the authentication state: {ex.Message}");
                return new ApplicationAuthenticationState { IsAuthenticated = false };
            }
        }


        public void Logout()
        {
            navigationManager.NavigateTo("Account/Logout", true);
        }

        public void Login()
        {
            navigationManager.NavigateTo("Login", true);
        }

        public async Task<IEnumerable<ApplicationRole>> GetRoles()
        {
            try
            {
                var uri = new Uri(baseUri, $"ApplicationRoles");
                uri = uri.GetODataUri(filter: $"TenantId eq {Tenant.Id}");

                var response = await httpClient.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle the failure as appropriate, such as logging the error
                    return Enumerable.Empty<ApplicationRole>();
                }

                var result = await response.ReadAsync<ODataServiceResult<ApplicationRole>>();
                return result.Value;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                return Enumerable.Empty<ApplicationRole>();
            }
        }

        public async Task<ApplicationRole> CreateRole(ApplicationRole role)
        {
            try
            {
                if (Tenant != null)
                {
                    role.TenantId = Tenant.Id;
                }

                var uri = new Uri(baseUri, $"ApplicationRoles");
                var content = new StringContent(ODataJsonSerializer.Serialize(role), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle the failure as appropriate, such as logging the error
                    return null; // Or throw an exception if you prefer
                }

                return await response.ReadAsync<ApplicationRole>();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                return null; // Return null or throw, depending on your error handling strategy
            }
        }


        public async Task<HttpResponseMessage> DeleteRole(string id)
        {
            try
            {
                var uri = new Uri(baseUri, $"ApplicationRoles('{id}')");
                var response = await httpClient.DeleteAsync(uri);

                // Optionally, check response.IsSuccessStatusCode here and handle accordingly
                if (!response.IsSuccessStatusCode)
                {
                    // Log error or handle failed response
                    // For example, you might want to log the error or throw a custom exception
                }

                return response;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as necessary
                // Depending on your application's requirements, you might want to:
                // - Log the error
                // - Return a custom error message
                // - Throw a custom exception

                // Example of logging the exception and returning a null or default HttpResponseMessage
                // LogError($"An error occurred while trying to delete role with ID {id}: {ex.Message}");

                // Return null, an empty HttpResponseMessage, or throw, depending on your error handling strategy
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"An error occurred: {ex.Message}")
                };
            }
        }


        public async Task<IEnumerable<ApplicationUser>> GetUsers()
        {
            try
            {
                var uri = new Uri(baseUri, $"ApplicationUsers");

                // Assuming Tenant.Id is accessible here; otherwise, you'll need to obtain it appropriately.
                uri = uri.GetODataUri(filter: $"TenantId eq {Tenant.Id}");

                var response = await httpClient.GetAsync(uri);
        
                if (!response.IsSuccessStatusCode)
                {
                    // Log the error or throw an exception
                    // LogError($"Error retrieving users: {response.ReasonPhrase}");
                    return Enumerable.Empty<ApplicationUser>();
                }

                var result = await response.ReadAsync<ODataServiceResult<ApplicationUser>>();

                return result.Value;
            }
            catch (Exception ex)
            {
                // Handle or log the exception as needed
                // LogError($"Exception when trying to retrieve users: {ex.Message}");
                return Enumerable.Empty<ApplicationUser>();
            }
        }

        public async Task<ApplicationUser> CreateUser(ApplicationUser user)
        {
            try
            {
                if (Tenant != null)
                {
                    user.TenantId = Tenant.Id;
                }
                var uri = new Uri(baseUri, $"ApplicationUsers");
                var content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    // Handle unsuccessful response appropriately
                    // Log the error, return null, or throw a custom exception
                    return null;
                }

                return await response.ReadAsync<ApplicationUser>();
            }
            catch (Exception ex)
            {
                // Log the exception, return null, or throw a custom exception
                return null;
            }
        }

        public async Task<HttpResponseMessage> DeleteUser(string id)
        {
            try
            {
                var uri = new Uri(baseUri, $"ApplicationUsers('{id}')");
                var response = await httpClient.DeleteAsync(uri);
                if (!response.IsSuccessStatusCode)
                {
                    // Handle unsuccessful response appropriately
                    // Log the error, return an error response, or throw a custom exception
                }
                return response;
            }
            catch (Exception ex)
            {
                // Log the exception, return an error response, or throw a custom exception
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"An error occurred: {ex.Message}")
                };
            }
        }

        public async Task<ApplicationUser> GetUserById(string id)
        {
            try
            {
                var uri = new Uri(baseUri, $"ApplicationUsers('{id}')?$expand=Roles");
                var response = await httpClient.GetAsync(uri);
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Handle 404 NotFound specifically if needed
                    return null;
                }
                if (!response.IsSuccessStatusCode)
                {
                    // Handle other unsuccessful responses
                    return null;
                }
                return await response.ReadAsync<ApplicationUser>();
            }
            catch (Exception ex)
            {
                // Log the exception, return null, or throw a custom exception
                return null;
            }
        }

        public async Task<ApplicationUser> UpdateUser(string id, ApplicationUser user)
        {
            try
            {
                var uri = new Uri(baseUri, $"ApplicationUsers('{id}')");
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri)
                {
                    Content = new StringContent(JsonSerializer.Serialize(user), Encoding.UTF8, "application/json")
                };

                var response = await httpClient.SendAsync(httpRequestMessage);
                if (!response.IsSuccessStatusCode)
                {
                    // Handle unsuccessful response appropriately
                    return null;
                }

                return await response.ReadAsync<ApplicationUser>();
            }
            catch (Exception ex)
            {
                // Log the exception, return null, or throw a custom exception
                return null;
            }
        }


        // Tenants
        public async Task<IEnumerable<ApplicationTenant>> GetTenants()
        {
            try
            {
                var uri = new Uri(baseUri, $"ApplicationTenants");
                uri = uri.GetODataUri();
                var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                var response = await httpClient.SendAsync(httpRequestMessage);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle unsuccessful response appropriately
                    return Enumerable.Empty<ApplicationTenant>();
                }

                var result = await response.ReadAsync<ODataServiceResult<ApplicationTenant>>();
                return result.Value;
            }
            catch (Exception ex)
            {
                // Log the exception, return an empty collection, or throw a custom exception
                return Enumerable.Empty<ApplicationTenant>();
            }
        }

        public async Task<ApplicationTenant> CreateTenant(ApplicationTenant tenant = default)
        {
            try
            {
                var uri = new Uri(baseUri, $"ApplicationTenants");
                var content = new StringContent(ODataJsonSerializer.Serialize(tenant), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle the error appropriately
                    return null;
                }

                return await response.ReadAsync<ApplicationTenant>();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return null;
            }
        }

        public async Task<HttpResponseMessage> DeleteTenant(int id = default)
        {
            try
            {
                var uri = new Uri(baseUri, $"ApplicationTenants({id})");
                return await httpClient.DeleteAsync(uri);
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                // Possibly return an indication of the failure
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ApplicationTenant> GetTenantById(int id = default)
        {
            try
            {
                var uri = new Uri(baseUri, $"ApplicationTenants({id})");
                var response = await httpClient.GetAsync(uri);

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                return await response.ReadAsync<ApplicationTenant>();
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                return null;
            }
        }

        public async Task<HttpResponseMessage> UpdateTenant(int id = default, ApplicationTenant tenant = default)
        {
            try
            {
                var uri = new Uri(baseUri, $"ApplicationTenants({id})");
                var content = new StringContent(ODataJsonSerializer.Serialize(tenant), Encoding.UTF8, "application/json");
                return await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Patch, uri) { Content = content });
            }
            catch (Exception ex)
            {
                // Handle or log the exception
                // Possibly return an indication of the failure
                return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
            }
        }

        public async Task ChangePassword(string oldPassword, string newPassword)
        {
            try
            {
                var uri = new Uri($"{navigationManager.BaseUri}Account/ChangePassword");
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "oldPassword", oldPassword },
                    { "newPassword", newPassword }
                });

                var response = await httpClient.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Failed to change password: {message}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException("An error occurred while changing the password.", ex);
            }
        }

        public async Task Register(string userName, string password)
        {
            try
            {
                var uri = new Uri($"{navigationManager.BaseUri}Account/Register");
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "userName", userName },
                    { "password", password }
                });

                var response = await httpClient.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Failed to register: {message}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException("An error occurred during registration.", ex);
            }
        }

        public async Task ResetPassword(string userName)
        {
            try
            {
                var uri = new Uri($"{navigationManager.BaseUri}Account/ResetPassword");
                var content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "userName", userName }
                });

                var response = await httpClient.PostAsync(uri, content);
                if (!response.IsSuccessStatusCode)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Failed to reset password: {message}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new ApplicationException("An error occurred while resetting the password.", ex);
            }
        }

    }
}