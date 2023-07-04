using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Gremlin.Net.Driver;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FriendshipService;

public class AddFriendsFunction
{
    private readonly ILogger _logger;
    private readonly GremlinService _gremlinService;
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AddFriendsFunction(
        ILoggerFactory loggerFactory,
        IHttpClientFactory httpClientFactory,
        GremlinService gremlinService,
        IConfiguration configuration)
    {
        _gremlinService = gremlinService;
        _logger = loggerFactory.CreateLogger<AddFriendsFunction>();
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
    }

    [Function(nameof(AddFriendsFunction))]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "friends")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a {0} request.", nameof(AddFriendsFunction));

        var requesterId = GetNameIdentifier(req);
        var request = JsonConvert.DeserializeObject<AddFriendRequest>(await new StreamReader(req.Body).ReadToEndAsync());

        var requesterResponse = await _httpClient.GetAsync($"{_configuration["UserServiceBaseUrl"]}/users/{requesterId}");
        var friendResponse = await _httpClient.GetAsync($"{_configuration["UserServiceBaseUrl"]}/users/{request.FriendId}");

        var requester = JsonConvert.DeserializeObject<UserAccount>(await requesterResponse.Content.ReadAsStringAsync());
        var friend = JsonConvert.DeserializeObject<UserAccount>(await friendResponse.Content.ReadAsStringAsync());

        var persons = new string[]
        {
            string.Format(_gremlinService.UpsertPersonQuery,requester.Id,requester.Username, requester.Email),
            string.Format(_gremlinService.UpsertPersonQuery,friend.Id,friend.Username, friend.Email)
        };
        var friendship = string.Format(_gremlinService.AddFriendshipQuery, requester.Id, friend.Id);

        using (var gremlinClient = _gremlinService.CreateClient())
        {
            var requests = persons.Select(query => gremlinClient.SubmitAsync<dynamic>(query));
            await Task.WhenAll(requests);
            await gremlinClient.SubmitAsync<dynamic>(friendship);
        }

        return req.OkResult();
    }

    private static string GetNameIdentifier(HttpRequestData req)
    {
        var headers = req.Headers;
        headers.TryGetValues("user-id", out IEnumerable<string> userId);
        return userId.First();
    }
}

internal record AddFriendRequest
{
    public string FriendId { get; init; }
}

internal record UserAccount
{
    public string Id { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string Surname { get; init; }
    public string Username { get; init; }
}