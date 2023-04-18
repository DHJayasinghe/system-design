using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using LikeService.Models;
using System;

namespace LikeService;

public static class RemoveReactionFunction
{
    [FunctionName(nameof(RemoveReactionFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "reaction")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        string requestBody = new StreamReader(req.Body).ReadToEnd();
        var data = JsonConvert.DeserializeObject<Reaction>(requestBody).WithDefaults();

        log.LogInformation("{0} function processed a request for post: {1} from user: {2}.", nameof(RemoveReactionFunction), data.PostId, data.UserId);

        try
        {
            var result = await cosmosClient
               .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName)
               .DeleteItemAsync<Reaction>(data.Id, new PartitionKey(data.PostId.ToString()));
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("NotFound (404)")) return new BadRequestObjectResult("Reaction does not exist");
            throw;
        }

        return new OkResult();
    }
}
