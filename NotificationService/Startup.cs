using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SharedKernal;

[assembly: FunctionsStartup(typeof(NotificationService.Startup))]
namespace NotificationService;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;

        services
            .AddHttpContextAccessor()
            .AddScoped<ICurrentUser, CurrentUser>();
    }
}
