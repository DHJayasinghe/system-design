***REMOVED***
using Microsoft.Extensions.Hosting;
***REMOVED***

namespace FriendshipService;

public class Program
***REMOVED***
    static async Task Main(string[] args)
    ***REMOVED***
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults((builderContext, builder) =>
            ***REMOVED***
        ***REMOVED***.Services.AddSingleton<GremlinService>();
        ***REMOVED***)
            .Build();

        await host.RunAsync();
***REMOVED***
***REMOVED***
