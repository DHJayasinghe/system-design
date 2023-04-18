using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using LikeService.Models;
using Microsoft.Azure.Cosmos;

namespace LikeService;

public static class AddReactionFunction
{
    [FunctionName(nameof(AddReactionFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "reaction")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        string requestBody = new StreamReader(req.Body).ReadToEnd();
        var data = JsonConvert.DeserializeObject<Reaction>(requestBody).WithDefaults();

        log.LogInformation("{0} function processed a request for post: {1} from user: {2}.", nameof(AddReactionFunction), data.PostId, data.UserId);

        var result = await cosmosClient
            .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName)
            .UpsertItemAsync(data, new PartitionKey(data.PostId.ToString()));

        return new OkResult();
    }
}