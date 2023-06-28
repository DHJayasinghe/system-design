using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PostService.Configs;
using Microsoft.Azure.Cosmos;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using PostService.Models;
using PostService.API.Models;

namespace PostService.API;

public class AddCommentsFunction
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AddCommentsFunction(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [FunctionName(nameof(AddCommentsFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posts/{postId}/comments")] AddCommentRequest req,
        Guid postId,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: nameof(Comment), Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(AddCommentsFunction));

        var entity = Comment.Map(postId, req);

        var result = await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, nameof(Comment))
          .CreateItemAsync(entity, new PartitionKey(entity.PostId));

        return new OkObjectResult(new { Id = result.Resource.Id });
    }
}