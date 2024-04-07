using CryptographicElectronicVotingSystem;
using Radzen;
using CryptographicElectronicVotingSystem.Components;
using Microsoft.EntityFrameworkCore;
using CryptographicElectronicVotingSystem.Data;
using Microsoft.AspNetCore.Identity;
using CryptographicElectronicVotingSystem.Models;
using CryptographicElectronicVotingSystem.Models.ApplicationIdentity;
using CryptographicElectronicVotingSystem.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Win32;

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

// Determine the environment
var environment = builder.Environment;
var destFolder = Path.Combine(System.Environment.GetEnvironmentVariable("LOCALAPPDATA"), "CryptographicElectronicVotingSystem-keys");

// Configure Data Protection
if (environment.IsDevelopment())
{
    builder.Services.AddDataProtection()
        .SetApplicationName("CryptographicElectronicVotingSystem")
        .PersistKeysToFileSystem(new DirectoryInfo(destFolder))
        .ProtectKeysWithDpapi() // Uncomment if you decide to use DPAPI in development
        .SetDefaultKeyLifetime(TimeSpan.FromDays(14));
}
else
{
    // Ensure you have a reference to Microsoft.Win32.Registry for accessing the Windows Registry
    builder.Services.AddDataProtection()
        .PersistKeysToRegistry(Registry.CurrentUser.OpenSubKey(@"SOFTWARE\CryptographicElectronicVotingSystem\DataProtection", writable: true));
}
builder.Services.AddSingleton<DataProtectionService>();


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
//app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();
app.Run();