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
using PostService.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using PostService.API.Models;
using SharedKernal;
using Microsoft.Azure.WebJobs.ServiceBus;
using PostService.Events;

namespace PostService;

public class AddPostsFunction
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ICurrentUser _currentUser;

    public AddPostsFunction(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ICurrentUser currentUser)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _currentUser = currentUser;
    }

    [FunctionName(nameof(AddPostsFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posts")] AddPostRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.PostsContainer, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        [ServiceBus(queueOrTopicName: "post", entityType: ServiceBusEntityType.Topic, Connection = "ServiceBus")] IAsyncCollector<EventBusMessageWrapper> eventBus,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(AddPostsFunction));

        var content = new StringContent(JsonConvert.SerializeObject(new
        {
            Assets = req.Assets
        }));


        using var httpClient = _httpClientFactory.CreateClient();
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        var response = await httpClient.PostAsync(_configuration.GetValue<string>("AssetService"), content);
        var assets = await response.Content.ReadAsAsync<List<string>>();

        var entity = Post.Map(req);
        entity.Assets = assets;
        entity.AuthorId = _currentUser.Id;

        var result = await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.PostsContainer)
          .CreateItemAsync(entity, new PartitionKey(entity.PostId));

        await eventBus.AddAsync(new EventBusMessageWrapper(new PostCreatedIntegrationEvent(entity)));

        return new OkObjectResult(new { result.Resource.PostId });
    }
}