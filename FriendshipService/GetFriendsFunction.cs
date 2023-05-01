using System.Net;
using System.Threading.Tasks;
using Gremlin.Net.Extensions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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

    //[Function(nameof(GetFriendsFunction))]
    //public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "friends")] HttpRequestData req)
    //{
    //    _logger.LogInformation("C# HTTP trigger function processed a {0} request.", nameof(GetFriendsFunction));

    //    var persons = new GremlinQuery[]
    //   {
    //       _gremlinService.G.V().HasLabel("Person").Has("email", "u5@gmail.com").ToGremlinQuery()
    //   };

    //    using (var gremlinClient = _gremlinService.CreateClient())
    //    {
    //        var requests = persons.Select(person => gremlinClient.SubmitAsync<dynamic>(person.ToString(), new Dictionary<string, object>(person.Arguments)));
    //        await Task.WhenAll(requests);
    //    }
    //}
}
