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

namespace PostService.API;

public class AddCommentFunction
***REMOVED***
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AddCommentFunction(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    ***REMOVED***
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
***REMOVED***

    [FunctionName(nameof(AddCommentFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posts/***REMOVED***postId***REMOVED***/comments")] AddCommentRequest req,
        Guid postId,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: nameof(Comment), Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** HTTP trigger processed a request.", nameof(AddPostsFunction));

        var entity = Comment.Map(postId, req);

        var result = await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, nameof(Comment))
          .CreateItemAsync(entity, new PartitionKey(entity.PostId));

        return new OkObjectResult(new ***REMOVED*** Id = result.Resource.Id ***REMOVED***);
***REMOVED***
***REMOVED***

public record AddCommentRequest
***REMOVED***
    public string Content ***REMOVED*** get; init; ***REMOVED***
***REMOVED***
