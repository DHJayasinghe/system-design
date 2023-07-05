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
using SharedKernal;

namespace PostService.API;

public class AddCommentsFunction
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ICurrentUser _currentUser;

    public AddCommentsFunction(
        IHttpClientFactory httpClientFactory, 
        IConfiguration configuration,
        ICurrentUser currentUser)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _currentUser = currentUser;
    }

    [FunctionName(nameof(AddCommentsFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posts/{postId}/comments")] AddCommentRequest req,
        Guid postId,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.CommentContainer, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(AddCommentsFunction));

        var entity = Comment.Map(postId, req);
        entity.AuthorId = _currentUser.Id;

        var result = await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, nameof(Comment))
          .CreateItemAsync(entity, new PartitionKey(entity.PostId));

        return new OkObjectResult(new { Id = result.Resource.Id });
    }
}