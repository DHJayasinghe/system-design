using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PostService.Configs;
using Microsoft.Azure.Cosmos;
using PostService.Models;
using System.Collections.Generic;

namespace PostService.API;

public static class GetPostsFunction
{
    [FunctionName("GetPostsFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "posts")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        var feed = GetRecentPosts(cosmosClient);
        var reactions = new List<Post>();
        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            foreach (var item in response)
            {
                reactions.Add(item);
            }
        }

        return new OkObjectResult(reactions);
    }


    private static FeedIterator<Post> GetRecentPosts(CosmosClient cosmosClient)
    {
        var container = cosmosClient.GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName);
        var query = $"SELECT * FROM {nameof(CosmosDbConfigs.ContainerName)}";

        return container.GetItemQueryIterator<Post>(
                     queryDefinition: new QueryDefinition(
             query: query
         ));
    }
}
