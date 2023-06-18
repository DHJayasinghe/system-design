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
using System.Linq;

namespace PostService.API;

public static class GetPostsFunction
{
    [FunctionName("GetPostsFunction")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "posts")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        var feed = GetRecentPosts(cosmosClient);
        var posts = new List<Post>();
        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            foreach (var post in response) posts.Add(post);
        }
        posts.ForEach(post => post.Assets = post.Assets?.Select(asset => "https://simadfutilityfuncaue.blob.core.windows.net/" + asset).ToList());

        return new OkObjectResult(posts.OrderByDescending(post => post.CreatedAt));
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
