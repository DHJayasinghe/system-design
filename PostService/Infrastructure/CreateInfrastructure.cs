using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
***REMOVED***
using System.Collections.Generic;
using PostService.Configs;
using PostService.Models;

namespace PostService.Infrastructure;

public class CreateInfrastructure
***REMOVED***
    private readonly IConfiguration _configuration;

    public CreateInfrastructure(IConfiguration configuration) => _configuration = configuration;

    [FunctionName(nameof(CreateInfrastructure))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posts/infrastructure")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
***REMOVED***
        log.LogInformation("C# HTTP trigger function processed a request.");

        var tasks = new List<Task>();
        await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosDbConfigs.DatabaseName);
        tasks.Add(cosmosClient.GetDatabase(CosmosDbConfigs.DatabaseName)
            .DefineContainer(name: CosmosDbConfigs.ContainerName, partitionKeyPath: $"/***REMOVED***nameof(Post.PostId)***REMOVED***")
            .WithUniqueKey()
                .Path($"/***REMOVED***nameof(Post.AuthorId)***REMOVED***")
            .Attach()
            .CreateIfNotExistsAsync());
        tasks.Add(cosmosClient.GetDatabase(CosmosDbConfigs.DatabaseName)
            .DefineContainer(name: nameof(Comment), partitionKeyPath: $"/***REMOVED***nameof(Comment.PostId)***REMOVED***")
            .WithUniqueKey()
                .Path($"/***REMOVED***nameof(Comment.CommentId)***REMOVED***")
            .Attach()
            .CreateIfNotExistsAsync());

        await Task.WhenAll(tasks);

        return new OkResult();
    ***REMOVED***
***REMOVED***