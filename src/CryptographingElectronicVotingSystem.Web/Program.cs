using Radzen;
using CryptographingElectronicVotingSystem.Web.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddHttpClient();
builder.Services.AddScoped<CryptographingElectronicVotingSystem.Service.Services.ElectronicVotingSystemService>();
builder.Services.AddDbContext<CryptographingElectronicVotingSystem.Dal.Data.ElectronicVotingSystemContext>(options =>
{
   options.UseMySql(builder.Configuration.GetConnectionString("ElectronicVotingSystemConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("ElectronicVotingSystemConnection")));
});

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
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();