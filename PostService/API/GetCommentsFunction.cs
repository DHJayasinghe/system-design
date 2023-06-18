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
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace PostService.API;

public class GetCommentsFunction
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GetCommentsFunction(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [FunctionName(nameof(GetCommentsFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "posts/{postId}/comments")] HttpRequest req,
        Guid postId,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: nameof(Comment), Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(GetCommentsFunction));

        QueryDefinition queryDefinition = new QueryDefinition("SELECT * FROM c WHERE c.PostId = @postId ORDER BY c.CreatedAt DESC")
              .WithParameter("@postId", postId);

        var container = cosmosClient.GetContainer(CosmosDbConfigs.DatabaseName, nameof(Comment));
        using var feedIterator = container.GetItemQueryIterator<Comment>(queryDefinition);

        var comments = new List<Comment>();
        while (feedIterator.HasMoreResults)
        {
            var response = await feedIterator.ReadNextAsync();
            foreach (var comment in response) comments.Add(comment);
        }

        return new OkObjectResult(comments);
    }
}
