using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SharedKernal;
using PostService.Models;
using PostService.Configs;
using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace PostService.API;

public class GetTimelineFunction
{
    private readonly IConfiguration _configuration;
    private readonly ICurrentUser _currentUser;

    public GetTimelineFunction(IConfiguration configuration, ICurrentUser currentUser)
    {
        _configuration = configuration;
        _currentUser = currentUser;
    }

    [FunctionName("GetTimelineFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "posts/timeline")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.TimelineContainer, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(GetTimelineFunction));

        var currentUserTimelineKey = TimelineActivity.GetKey(_currentUser.Id);

        var activities = await GetTimelineByUserAsync(cosmosClient, currentUserTimelineKey);
        var posts = await GetTimelinePostsAsync(cosmosClient, activities);
        posts.ForEach(post => post.Assets = post.Assets?.Select(asset => $"{_configuration["AssetsBaseUrl"]}/{asset}").ToList());

        return new OkObjectResult(posts);
    }

    private static async Task<List<Post>> GetTimelinePostsAsync(CosmosClient cosmosClient, List<TimelineActivity> activities)
    {
        var postIds = string.Join(',', activities.Select(activity => $"'{activity.PostId}'"));
        var query = $"SELECT * FROM c WHERE c.PostId IN ({postIds})";

        var postsIterator = cosmosClient.GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.PostsContainer)
            .GetItemQueryIterator<Post>(new QueryDefinition(query: query));
        var posts = new List<Post>();
        while (postsIterator.HasMoreResults)
        {
            var response = await postsIterator.ReadNextAsync();
            foreach (var post in response) posts.Add(post);
        }

        return posts;
    }

    private static async Task<List<TimelineActivity>> GetTimelineByUserAsync(CosmosClient cosmosClient, string key)
    {
        var timelineIterator = cosmosClient
            .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.TimelineContainer)
            .GetItemQueryIterator<TimelineActivity>(queryDefinition: new QueryDefinition($"SELECT * FROM c WHERE c.Key='{key}' ORDER BY c.CreatedAt DESC"));

        var activities = new List<TimelineActivity>();
        while (timelineIterator.HasMoreResults)
        {
            var response = await timelineIterator.ReadNextAsync();
            foreach (var activity in response) activities.Add(activity);
        }

        return activities;
    }
}
