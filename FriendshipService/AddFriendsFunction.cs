using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FriendshipService;

public class AddFriendsFunction
{
    private readonly ILogger _logger;
    private readonly GremlinService _gremlinService;

    public AddFriendsFunction(ILoggerFactory loggerFactory, GremlinService gremlinService)
    {
        _gremlinService = gremlinService;
        _logger = loggerFactory.CreateLogger<GetFriendsFunction>();
    }

    [Function(nameof(AddFriendsFunction))]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "friends")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a {0} request.", nameof(AddFriendsFunction));

        var request = JsonConvert.DeserializeObject<AddFriendRequest>(await new StreamReader(req.Body).ReadToEndAsync());

        var persons = new string[]
        {
            string.Format(_gremlinService.UpsertPersonQuery,request.UserId,request.UserName,request.UserEmail),
            string.Format(_gremlinService.UpsertPersonQuery,request.FriendId,request.FriendName,request.FriendEmail),
            string.Format(_gremlinService.AddFriendshipQuery,request.UserId,request.FriendId),
        };

        using (var gremlinClient = _gremlinService.CreateClient())
        {
            var requests = persons.Select(query => gremlinClient.SubmitAsync<dynamic>(query));
            await Task.WhenAll(requests);
        }

        return req.OkResult();
    }
}

internal record AddFriendRequest
{
    public string UserId { get; init; }
    public string UserEmail { get; init; }
    public string UserName { get; init; }
    public string FriendId { get; init; }
    public string FriendName { get; init; }
    public string FriendEmail { get; init; }
}