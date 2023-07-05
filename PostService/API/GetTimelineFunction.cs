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

namespace PostService.API;

public class GetTimelineFunction
{
    private readonly ICurrentUser _currentUser;

    public GetTimelineFunction(ICurrentUser currentUser) => _currentUser = currentUser;

    [FunctionName("GetTimelineFunction")]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "timeline")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.TimelineContainer, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(GetTimelineFunction));

        var key = TimelineActivity.GetKey(_currentUser.Id);
        var feedIterator = cosmosClient
            .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.TimelineContainer)
            .GetItemQueryIterator<TimelineActivity>(queryDefinition: new QueryDefinition($"SELECT * FROM c WHERE c.Key='{key}'"));

        var activities = new List<TimelineActivity>();
        while (feedIterator.HasMoreResults)
        {
            var response = await feedIterator.ReadNextAsync();
            foreach (var activity in response) activities.Add(activity);
        }

        return new OkObjectResult(activities);
    }


    private static FeedIterator<Post> GetRecentPosts(CosmosClient cosmosClient)
    {
        var container = cosmosClient.GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.PostsContainer);
        var query = $"SELECT * FROM {nameof(CosmosDbConfigs.PostsContainer)}";

        return container.GetItemQueryIterator<Post>(
                     queryDefinition: new QueryDefinition(
             query: query
         ));
    }
}
