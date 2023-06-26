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
***REMOVED***
    private readonly ILogger _logger;
    private readonly GremlinService _gremlinService;

    public AddFriendsFunction(ILoggerFactory loggerFactory, GremlinService gremlinService)
***REMOVED***
        _gremlinService = gremlinService;
        _logger = loggerFactory.CreateLogger<GetFriendsFunction>();
    ***REMOVED***

    [Function(nameof(AddFriendsFunction))]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "friends")] HttpRequestData req)
***REMOVED***
        _logger.LogInformation("C# HTTP trigger function processed a ***REMOVED***0***REMOVED*** request.", nameof(AddFriendsFunction));

        var request = JsonConvert.DeserializeObject<AddFriendRequest>(await new StreamReader(req.Body).ReadToEndAsync());

        var persons = new string[]
    ***REMOVED***
            string.Format(_gremlinService.UpsertPersonQuery,request.UserId,request.UserName,request.UserEmail),
            string.Format(_gremlinService.UpsertPersonQuery,request.FriendId,request.FriendName,request.FriendEmail)
***REMOVED***
        var friendship = string.Format(_gremlinService.AddFriendshipQuery, request.UserId, request.FriendId);

        using (var gremlinClient = _gremlinService.CreateClient())
    ***REMOVED***
            var requests = persons.Select(query => gremlinClient.SubmitAsync<dynamic>(query));
            await Task.WhenAll(requests);
            await gremlinClient.SubmitAsync<dynamic>(friendship);
        ***REMOVED***

        return req.OkResult();
    ***REMOVED***
***REMOVED***

internal record AddFriendRequest
***REMOVED***
    public string UserId ***REMOVED*** get; init; ***REMOVED***
    public string UserEmail ***REMOVED*** get; init; ***REMOVED***
    public string UserName ***REMOVED*** get; init; ***REMOVED***
    public string FriendId ***REMOVED*** get; init; ***REMOVED***
    public string FriendName ***REMOVED*** get; init; ***REMOVED***
    public string FriendEmail ***REMOVED*** get; init; ***REMOVED***
***REMOVED***