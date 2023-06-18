***REMOVED***
***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
using PostService.Configs;
using Microsoft.Azure.Cosmos;
using System.Net.Http;
***REMOVED***
using PostService.Models;
***REMOVED***
***REMOVED***

namespace PostService.API;

public class GetCommentsFunction
***REMOVED***
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GetCommentsFunction(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    ***REMOVED***
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
***REMOVED***

    [FunctionName(nameof(GetCommentsFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "posts/***REMOVED***postId***REMOVED***/comments")] HttpRequest req,
        Guid postId,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: nameof(Comment), Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** HTTP trigger processed a request.", nameof(GetCommentsFunction));

        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.PostId = @postId ORDER BY c.CreatedAt DESC")
              .WithParameter("@postId", postId);

        var container = cosmosClient.GetContainer(CosmosDbConfigs.DatabaseName, nameof(Comment));
        using var feedIterator = container.GetItemQueryIterator<Comment>(queryDefinition);

        var comments = new List<Comment>();
        while (feedIterator.HasMoreResults)
        ***REMOVED***
            var response = await feedIterator.ReadNextAsync();
            foreach (var comment in response) comments.Add(comment);
    ***REMOVED***

        return new OkObjectResult(comments);
***REMOVED***
***REMOVED***
