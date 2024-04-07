using CryptographicElectronicVotingSystem.Dal.Data;
using CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity;
using Radzen;
using Microsoft.EntityFrameworkCore;
using CryptographicElectronicVotingSystem.Web.Components;
using CryptographicElectronicVotingSystem.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<CryptographicElectronicVotingSystemContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("ElectronicVotingSystemConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ElectronicVotingSystemConnection")));
});
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("ElectronicVotingSystemConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ElectronicVotingSystemConnection")));
});

builder.Services.AddScoped<CryptographicElectronicVotingSystemService>();
builder.Services.AddScoped<SecurityService>();

builder.Services.AddHttpClient("CryptographicElectronicVotingSystem").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false }).AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllers().AddOData(o =>
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
    var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));
    oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles");
    o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});

builder.Services.AddScoped<AuthenticationStateProvider, ApplicationAuthenticationStateProvider>();

builder.Services.AddScoped<FakeDataGenerator>(provider => new FakeDataGenerator(
    provider.GetRequiredService<UserManager<ApplicationUser>>(),
    provider.GetRequiredService<RoleManager<ApplicationRole>>(),
    provider.GetRequiredService<ApplicationIdentityDbContext>(),
    provider.GetRequiredService<CryptographicElectronicVotingSystemContext>()
));

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseHeaderPropagation();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();