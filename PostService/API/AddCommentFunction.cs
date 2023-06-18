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

namespace PostService.API;

public class AddCommentFunction
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AddCommentFunction(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [FunctionName(nameof(AddCommentFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posts/{postId}/comments")] AddCommentRequest req,
        Guid postId,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: nameof(Comment), Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(AddPostsFunction));

        var entity = Comment.Map(postId, req);

        var result = await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, nameof(Comment))
          .CreateItemAsync(entity, new PartitionKey(entity.PostId));

        return new OkObjectResult(new { Id = result.Resource.Id });
    }
}

public record AddCommentRequest
{
    public string Content { get; init; }
}
