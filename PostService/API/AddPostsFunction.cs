***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using PostService.Configs;
using PostService.Models;
using Microsoft.Azure.Cosmos;
***REMOVED***
using PostService.API.Models;

namespace PostService;

public class AddPostsFunction
***REMOVED***
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public AddPostsFunction(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    ***REMOVED***
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
***REMOVED***

    [FunctionName(nameof(AddPostsFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posts")] AddPostRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** HTTP trigger processed a request.", nameof(AddPostsFunction));

        var content = new StringContent(JsonConvert.SerializeObject(new
        ***REMOVED***
            Assets = req.Assets
    ***REMOVED***));


        using var httpClient = _httpClientFactory.CreateClient();
        content.Headers.ContentType = new MediaTypeHeaderValue("***REMOVED***lication/json");

        var response = await httpClient.PostAsync(_configuration.GetValue<string>("AssetService"), content);
        var assets = await response.Content.ReadAsAsync<List<string>>();

        var entity = Post.Map(req);
        entity.Assets = assets;

        var result = await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName)
          .CreateItemAsync(entity, new PartitionKey(entity.PostId));

        return new OkObjectResult(result.Resource.PostId);
***REMOVED***
***REMOVED***