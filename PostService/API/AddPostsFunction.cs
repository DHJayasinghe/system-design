using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using PostService.Configs;
using System;
using PostService.Models;
using Microsoft.Azure.Cosmos;

namespace PostService;

public class AddPostsFunction
{
    private readonly IHttpClientFactory _httpClientFactory;
    public AddPostsFunction(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [FunctionName(nameof(AddPostsFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posts")] PostRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(AddPostsFunction));

        HttpContent content = new StringContent(JsonConvert.SerializeObject(new
        {
            Assets = req.Assets
        }));

        using var httpClient = _httpClientFactory.CreateClient();
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await httpClient.PostAsync("http://localhost:8083/assets", content);
        var assets = await response.Content.ReadAsAsync<List<string>>();

        var entity = Post.Map(req);
        entity.Assets = assets;

        var result = await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName)
          .UpsertItemAsync(entity, new PartitionKey(entity.PostId));

        return new OkResult();
    }
}

public record PostRequest
{
    public Guid PostId { get; private init; } = Guid.NewGuid();
    public string Content { get; init; }
    public List<string> Assets { get; init; }
}
