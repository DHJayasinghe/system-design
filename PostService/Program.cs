using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SharedKernal;

[assembly: FunctionsStartup(typeof(PostService.Startup))]
namespace PostService;

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
