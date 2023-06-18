***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using PostService.Configs;
using Microsoft.Azure.Cosmos;
using PostService.Models;
***REMOVED***
***REMOVED***

namespace PostService.API;

public static class GetPostsFunction
***REMOVED***
    [FunctionName(nameof(GetPostsFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "posts")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** HTTP trigger processed a request.", nameof(GetPostsFunction));

        var feed = GetRecentPosts(cosmosClient);
        var posts = new List<Post>();
        while (feed.HasMoreResults)
        ***REMOVED***
            var response = await feed.ReadNextAsync();
            foreach (var post in response) posts.Add(post);
    ***REMOVED***
        posts.ForEach(post => post.Assets = post.Assets?.Select(asset => "https://simadfutilityfuncaue.blob.core.windows.net/" + asset).ToList());

        return new OkObjectResult(posts.OrderByDescending(post => post.CreatedAt));
***REMOVED***


    private static FeedIterator<Post> GetRecentPosts(CosmosClient cosmosClient)
    ***REMOVED***
        var container = cosmosClient.GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName);
        var query = $"SELECT * FROM ***REMOVED***nameof(CosmosDbConfigs.ContainerName)***REMOVED***";

        return container.GetItemQueryIterator<Post>(
                     queryDefinition: new QueryDefinition(
             query: query
         ));
***REMOVED***
***REMOVED***
