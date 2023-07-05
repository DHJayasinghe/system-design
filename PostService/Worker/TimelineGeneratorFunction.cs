using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PostService.Configs;
using PostService.Models;

namespace PostService.Worker;

public class TimelineGeneratorFunction
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public TimelineGeneratorFunction(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _configuration = configuration;
    }

    [FunctionName(nameof(TimelineGeneratorFunction))]
    public async Task Run(
        [CosmosDBTrigger(
            databaseName:  CosmosDbConfigs.DatabaseName,
            containerName: CosmosDbConfigs.PostsContainer,
            Connection = CosmosDbConfigs.ConnectionName,
            CreateLeaseContainerIfNotExists = true,
            LeaseContainerName = $"{CosmosDbConfigs.PostsContainer}-lease")] IReadOnlyList<Post> posts,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.PostsContainer, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} function processed a request for post.", nameof(TimelineGeneratorFunction));

        foreach (var post in posts)
        {
            List<Friend> friends = await GetAuthorFriends(post);
            friends.Add(new Friend { UserId = post.AuthorId });

            var createActivityTasks = new List<Task>();
            foreach (var friend in friends.Distinct())
            {
                var activity = new TimelineActivity
                {
                    OwnerId = friend.UserId,
                    PostId = post.PostId
                };
                createActivityTasks.Add(cosmosClient.GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.TimelineContainer).CreateItemAsync(activity, new PartitionKey(activity.Key)));
            }
            await Task.WhenAll(createActivityTasks);
        }
    }

    private async Task<List<Friend>> GetAuthorFriends(Post post)
    {
        var friendsResponse = await _httpClient.GetAsync($"{_configuration["FriendshipService"]}?userId={post.AuthorId}");
        var friends = await friendsResponse.Content.ReadAsAsync<List<Friend>>();
        return friends;
    }
}

public record Friend
{
    public string UserId { get; init; }
}