***REMOVED***
***REMOVED***
***REMOVED***

namespace BnA.IAM.Application;

public static class DependencyInjection
***REMOVED***
    public static IServiceCollection AddApplication(this IServiceCollection ***REMOVED***, IConfiguration configuration)
***REMOVED***
        ***REMOVED***
           .AddScoped<ITableStorageService, TableStorageService>();

        return ***REMOVED***;
    ***REMOVED***
***REMOVED***