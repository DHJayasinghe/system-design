***REMOVED***
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PostService.Configs;
using Microsoft.Azure.Cosmos;
using System.Net.Http;
***REMOVED***
using PostService.Models;
using PostService.API.Models;

namespace PostService.API;

public class AddCommentsFunction
***REMOVED***
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AddCommentsFunction(IHttpClientFactory httpClientFactory, IConfiguration configuration)
***REMOVED***
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    ***REMOVED***

    [FunctionName(nameof(AddCommentsFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posts/***REMOVED***postId***REMOVED***/comments")] AddCommentRequest req,
        Guid postId,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: nameof(Comment), Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** HTTP trigger processed a request.", nameof(AddCommentsFunction));

        var entity = Comment.Map(postId, req);

        var result = await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, nameof(Comment))
          .CreateItemAsync(entity, new PartitionKey(entity.PostId));

        return new OkObjectResult(new ***REMOVED*** Id = result.Resource.Id ***REMOVED***);
    ***REMOVED***
***REMOVED***