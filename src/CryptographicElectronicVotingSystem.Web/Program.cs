using Radzen;
using CryptographicElectronicVotingSystem.Web.Components;
using Microsoft.EntityFrameworkCore;
using CryptographicElectronicVotingSystem.Dal.Data;
using Microsoft.AspNetCore.Identity;
using CryptographicElectronicVotingSystem.Dal.Models.ApplicationIdentity;
using CryptographicElectronicVotingSystem.Dal.Models.CryptographicElectronicVotingSystem;

using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();
builder.Services.AddDbContext<CryptographicElectronicVotingSystem.Dal.Data.CryptographicElectronicVotingSystemContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("ElectronicVotingSystemConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ElectronicVotingSystemConnection")));
});
builder.Services.AddDbContext<CryptographicElectronicVotingSystem.Dal.Data.ApplicationIdentityDbContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("ElectronicVotingSystemConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ElectronicVotingSystemConnection")));
});


builder.Services.AddScoped<CryptographicElectronicVotingSystem.Web.Services.CryptographicElectronicVotingSystemService>();
builder.Services.AddScoped<CryptographicElectronicVotingSystem.Web.Services.SecurityService>();

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
builder.Services.AddScoped<AuthenticationStateProvider, CryptographicElectronicVotingSystem.Web.Services.ApplicationAuthenticationStateProvider>();
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
//app.Services.CreateScope().ServiceProvider.GetRequiredService<CryptographicElectronicVotingSystem.Dal.Data.ApplicationIdentityDbContext>().Database.Migrate();
app.Run();