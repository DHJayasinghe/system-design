using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using LikeService.Models;

namespace LikeService;

public static class AddReactionFunction
{
    [FunctionName(nameof(AddReactionFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "reaction")] HttpRequest req,
        [CosmosDB(databaseName: "like-service", containerName: "reaction", Connection = "CosmosDBConnection", PartitionKey = "/id", CreateIfNotExists = true)] IAsyncCollector<PostLike> documents,
        ILogger log)
    {
        string requestBody = new StreamReader(req.Body).ReadToEnd();
        var data = JsonConvert.DeserializeObject<PostLike>(requestBody);
        data.AddDefaults();

        log.LogInformation("{0} function processed a request for post: {1} from user: {2}.", nameof(AddReactionFunction), data.PostId, data.UserId);

        await documents.AddAsync(data);

        return new OkObjectResult(1);
    }
}