using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using LikeService.Models;

namespace LikeService.Infrastructure;

public class CreateInfrastructure
{
    private readonly IConfiguration _configuration;

    public CreateInfrastructure(IConfiguration configuration) => _configuration = configuration;

    [FunctionName(nameof(CreateInfrastructure))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "infrastructure")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient client,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        await client.CreateDatabaseIfNotExistsAsync(CosmosDbConfigs.DatabaseName);
        await client.GetDatabase(CosmosDbConfigs.DatabaseName)
            .DefineContainer(name:CosmosDbConfigs.ContainerName, partitionKeyPath: $"/{nameof(Reaction.PostId)}")
            .WithUniqueKey()
                .Path($"/{nameof(Reaction.UserId)}")
            .Attach()
            .CreateIfNotExistsAsync();
        return new OkResult();
    }
}