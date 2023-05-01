using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Gremlin.Net.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FriendshipService;

public class AddFriendFunction
{
    private readonly GremlinService _gremlinService;

    public AddFriendFunction(GremlinService gremlinService) => _gremlinService = gremlinService;

    [Function(nameof(AddFriendFunction))]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "friend")] HttpRequestData req, FunctionContext executionContext)
    {
        var log = executionContext.GetLogger(nameof(AddFriendFunction));
        log.LogInformation("C# HTTP trigger function processed a {0} request.", nameof(AddFriendFunction));

        var request = JsonConvert.DeserializeObject<AddFriendRequest>(await new StreamReader(req.Body).ReadToEndAsync());

        var persons = new GremlinQuery[]
        {
           _gremlinService.G.AddV("Person").Property("id", request.UserId).Property("email", request.UserEmail).Property("name",request.UserName).ToGremlinQuery(),
           _gremlinService.G.AddV("Person").Property("id", request.FriendId).Property("email", request.FriendEmail).Property("name", request.FriendName).ToGremlinQuery(),
        };

        using (var gremlinClient = _gremlinService.CreateClient())
        {
            var requests = persons.Select(person => gremlinClient.SubmitAsync<dynamic>(person.ToString(), new Dictionary<string, object>(person.Arguments)));
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