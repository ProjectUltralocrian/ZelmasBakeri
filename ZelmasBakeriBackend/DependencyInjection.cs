using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZelmasBakeriBackend.DataAccess;
using ZelmasBakeriBackend.Services;

namespace ZelmasBakeriBackend;

public static class DependencyInjection
{
    public static IServiceCollection AddBackendServices(
        this IServiceCollection services,
        IConfiguration config,
        bool isDevelopment
    )
    {
        services.AddSingleton<IDbAccess, SqlServerConnector>();
        if (isDevelopment)
        {
            services.AddSingleton<IEmailSender, DummyEmailSender>();
        }
        else
        {
            services.AddSingleton<IEmailSender>(_ =>
            {
                return new GmailSender
                {
                    FromAddress = "zelmasbakeri@gmail.com",
                    UserName = "zelmasbakeri@gmail.com",
                    Password = config.GetValue<string>("EmailSettings:Password") ?? "",
                };
            });
        }

        return services;
    }
}
