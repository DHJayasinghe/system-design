using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PostService.Configs;
using PostService.Models;
using SharedKernal;

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
        [CosmosDB(
            databaseName: CosmosDbConfigs.DatabaseName,
            containerName: CosmosDbConfigs.PostsContainer,
            Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        [ServiceBus(
            queueOrTopicName: "post",
            entityType: ServiceBusEntityType.Topic,
            Connection = "ServiceBus")] IAsyncCollector<EventBusMessageWrapper> eventBus,
        ILogger log)
    {
        log.LogInformation("{0} function processed a request for post.", nameof(TimelineGeneratorFunction));
        var eventsPublishedTasks = new List<Task>();
        var timelineActivityCreateTasks = new List<Task>();

        foreach (var post in posts)
        {
            List<FriendResponse> friends = await GetAuthorFriends(post);
            friends.Add(new FriendResponse { UserId = post.AuthorId });

            foreach (var friend in friends.Distinct())
            {
                var activity = new TimelineActivity
                {
                    OwnerId = friend.UserId,
                    PostId = post.PostId
                };
                AddTimelineActivity(cosmosClient, timelineActivityCreateTasks, activity);
            }
        }

        await Task.WhenAll(timelineActivityCreateTasks);
        await Task.WhenAll(eventsPublishedTasks);
    }

    private static void AddTimelineActivity(CosmosClient cosmosClient, List<Task> timelineActivityCreateTasks, TimelineActivity activity)
    {
        timelineActivityCreateTasks.Add(cosmosClient.GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.TimelineContainer).CreateItemAsync(activity, new PartitionKey(activity.Key)));
    }

    private async Task<List<FriendResponse>> GetAuthorFriends(Post post)
    {
        var friendsResponse = await _httpClient.GetAsync($"{_configuration["FriendshipService"]}?userId={post.AuthorId}");
        var friends = await friendsResponse.Content.ReadAsAsync<List<FriendResponse>>();
        return friends;
    }
}

public record FriendResponse
{
    public string UserId { get; init; }
}