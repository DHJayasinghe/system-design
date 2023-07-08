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
using Microsoft.Extensions.Configuration;

namespace PostService.API;

public class GetPostsFunction
{
    private readonly IConfiguration _configuration;

    public GetPostsFunction(IConfiguration configuration) => _configuration = configuration;

    [FunctionName(nameof(GetPostsFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "posts")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.PostsContainer, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(GetPostsFunction));

        var feed = GetRecentPosts(cosmosClient);
        var posts = new List<Post>();
        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            foreach (var post in response) posts.Add(post);
        }

        return new OkObjectResult(posts.OrderByDescending(post => post.CreatedAt));
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
