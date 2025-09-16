using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using ZelmasBakeriApp.Components;
using ZelmasBakeriBackend.DataAccess;
using ZelmasBakeriBackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services
    .AddSingleton<IDbAccess, SqliteConnector>()
    .AddSingleton<IEmailSender>(_ =>
    {
        return new GmailSender
        {
            FromAddress = "zelmasbakeri@gmail.com",
            UserName = "zelmasbakeri@gmail.com",
            Password = builder.Configuration["EmailSettings:Password"] ?? "", 
        };
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

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
