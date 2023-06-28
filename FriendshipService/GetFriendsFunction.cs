using System.Collections.Generic;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FriendshipService;

public class GetFriendsFunction
{
    private readonly ILogger _logger;
    private readonly GremlinService _gremlinService;

    public GetFriendsFunction(ILoggerFactory loggerFactory, GremlinService gremlinService)
    {
        _gremlinService = gremlinService;
        _logger = loggerFactory.CreateLogger<GetFriendsFunction>();
    }

    [Function(nameof(GetFriendsFunction))]
    public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "friends")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a {0} request.", nameof(GetFriendsFunction));
        var userId = req.Query.Get("userId");
        var friends = new List<FriendsResponse>();
        using (var gremlinClient = _gremlinService.CreateClient())
        {
            var results = await gremlinClient.SubmitAsync<dynamic>("g.V().hasLabel('person').has('userId', '" + userId + "').both('friends').valueMap('name','email','userId').fold()");

            foreach (var result in results)
            {
                foreach (var item in result)
                {
                    Dictionary<string, string> expanedItem = GetItems(item);
                    if (expanedItem["name"] is null) break;

                    friends.Add(new FriendsResponse
                    {
                        Name = expanedItem["name"],
                        Email = expanedItem["email"],
                        UserId = expanedItem["userId"]
                    });
                }
            }
        }

        return req.OkObjectResult(friends);
    }

    private static string GetValue(dynamic item)
    {
        foreach (dynamic value in item.Value)
        {
            return (string)value;
        }
        return null;
    }

    private static Dictionary<string, string> GetItems(dynamic itemProperties)
    {
        var itemsExpanded = new Dictionary<string, string>();
        foreach (dynamic item in itemProperties)
        {
            itemsExpanded[item.Key] = GetValue(item);
        }
        return itemsExpanded;
    }
}


public record FriendsResponse
{
    public string Email { get; init; }
    public string Name { get; init; }
    public string UserId { get; init; }
}
