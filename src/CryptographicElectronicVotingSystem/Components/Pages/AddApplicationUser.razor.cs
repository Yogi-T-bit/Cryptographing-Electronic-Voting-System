using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptographicElectronicVotingSystem.Data;
using CryptographicElectronicVotingSystem.Models.ApplicationIdentity;
using CryptographicElectronicVotingSystem.Services;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CryptographicElectronicVotingSystem.Components.Pages
{
    public partial class AddApplicationUser
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        
        [Inject]
        protected FakeDataGenerator FakeDataGenerator { get; set; }

        protected IEnumerable<ApplicationRole> roles;
        protected ApplicationUser user;
        protected IEnumerable<string> userRoles = Enumerable.Empty<string>();
        protected string error;
        protected bool errorVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            user = new ApplicationUser();

            roles = await Security.GetRoles();
        }
        
        protected async Task GenerateApplicationUsersAndVoters()
        {
            try
            {
                int numOfUsers = 1; // Number of users to generate
                // Assuming `GenerateUsersAndRoles` returns a boolean indicating success
                bool success = await FakeDataGenerator.GenerateUsersAndRolesAsync(numOfUsers);

                if (success)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Operation Successful", "Users and voters have been successfully generated.", 5000);
                }
                else
                {
                    throw new Exception("Failed to generate all users and voters.");
                }

                // Refresh or update UI components if necessary
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                // Log the error or handle it as required
                Console.WriteLine($"An error occurred: {ex}");
                
                // Update UI to reflect the error
                errorVisible = true;

                // Notify user about the error
                NotificationService.Notify(NotificationSeverity.Error, "Error", "There was an error during the operation. Please try again.", 7000);
            }
        }
        
        
        // update users address
        protected async Task UpdateUserAddress()
        {
            try
            {
                // Assuming `UpdateUserAddress` returns a boolean indicating success
                bool success = await FakeDataGenerator.AddAddressesToUsersAsync();

                if (success)
                {
                    NotificationService.Notify(NotificationSeverity.Success, "Operation Successful", "User addresses have been successfully updated.", 5000);
                }
                else
                {
                    throw new Exception("Failed to update user addresses.");
                }

                // Refresh or update UI components if necessary
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                // Log the error or handle it as required
                Console.WriteLine($"An error occurred: {ex}");
                
                // Update UI to reflect the error
                errorVisible = true;

                // Notify user about the error
                NotificationService.Notify(NotificationSeverity.Error, "Error", "There was an error during the operation. Please try again.", 7000);
            }
        }

        protected async Task FormSubmit(ApplicationUser user)
        {
            try
            {
                user.Roles = roles.Where(role => userRoles.Contains(role.Id)).ToList();
                await Security.CreateUser(user);
                DialogService.Close(null);
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }

        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }
    }
}