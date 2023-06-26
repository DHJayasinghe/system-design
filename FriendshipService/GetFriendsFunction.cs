using System.Collections.Generic;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FriendshipService;

public class GetFriendsFunction
***REMOVED***
    private readonly ILogger _logger;
    private readonly GremlinService _gremlinService;

    public GetFriendsFunction(ILoggerFactory loggerFactory, GremlinService gremlinService)
***REMOVED***
        _gremlinService = gremlinService;
        _logger = loggerFactory.CreateLogger<GetFriendsFunction>();
    ***REMOVED***

    [Function(nameof(GetFriendsFunction))]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "friends")] HttpRequestData req)
***REMOVED***
        _logger.LogInformation("C# HTTP trigger function processed a ***REMOVED***0***REMOVED*** request.", nameof(GetFriendsFunction));
        var userId = req.Query.Get("userId");
        var friends = new List<FriendsResponse>();
        using (var gremlinClient = _gremlinService.CreateClient())
    ***REMOVED***
            var results = await gremlinClient.SubmitAsync<dynamic>("g.V().hasLabel('person').has('userId', '" + userId + "').both('friends').valueMap('name','email','userId').fold()");

            foreach (var result in results)
        ***REMOVED***
                foreach (var item in result)
            ***REMOVED***
                    Dictionary<string, string> expanedItem = GetItems(item);
                    if (expanedItem["name"] is null) break;

                    friends.Add(new FriendsResponse
                ***REMOVED***
                        Name = expanedItem["name"],
                        Email = expanedItem["email"],
                        UserId = expanedItem["userId"]
                ***REMOVED***;
                ***REMOVED***
            ***REMOVED***
        ***REMOVED***

        return req.OkObjectResult(friends);
    ***REMOVED***

    private static string GetValue(dynamic item)
***REMOVED***
        foreach (dynamic value in item.Value)
    ***REMOVED***
            return (string)value;
        ***REMOVED***
        return null;
    ***REMOVED***

    private static Dictionary<string, string> GetItems(dynamic itemProperties)
***REMOVED***
        var itemsExpanded = new Dictionary<string, string>();
        foreach (dynamic item in itemProperties)
    ***REMOVED***
            itemsExpanded[item.Key] = GetValue(item);
        ***REMOVED***
        return itemsExpanded;
    ***REMOVED***
***REMOVED***


public record FriendsResponse
***REMOVED***
    public string Email ***REMOVED*** get; init; ***REMOVED***
    public string Name ***REMOVED*** get; init; ***REMOVED***
    public string UserId ***REMOVED*** get; init; ***REMOVED***
***REMOVED***
