using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace LikeService.Infrastructure;

public class RemoveInfrastructure
{
    private readonly IConfiguration _configuration;

    public RemoveInfrastructure(IConfiguration configuration) => _configuration = configuration;

    [FunctionName(nameof(RemoveInfrastructure))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "infrastructure")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient client,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        await client.GetDatabase(CosmosDbConfigs.DatabaseName).DeleteAsync();

        return new OkResult();
    }
}
