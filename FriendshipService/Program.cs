using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace FriendshipService;

public class Program
{
    static async Task Main(string[] args)
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults((builderContext, builder) =>
            {
                builder.Services.AddSingleton<GremlinService>();
            })
            .Build();

        await host.RunAsync();
    }
}
