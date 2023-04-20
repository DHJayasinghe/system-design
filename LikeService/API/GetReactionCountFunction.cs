using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using LikeService.Models;
using System.Linq;
using Microsoft.Azure.Cosmos.Linq;
using System.Collections.Generic;

namespace LikeService.API;

public static class GetReactionCountFunction
{
    [FunctionName(nameof(GetReactionCountFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "reaction/count")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName2, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        string postId = req.Query[nameof(postId)].ToString();
        string commentId = req.Query[nameof(commentId)].ToString();

        log.LogInformation("{0} function processed a request for post: {1}.", nameof(GetReactionCountFunction), postId);

        var feed = cosmosClient
               .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2).GetItemLinqQueryable<ReactionCount>()
               .Where(d => d.PostId == postId && d.CommentId == (string.IsNullOrEmpty(commentId) ? d.CommentId : null))
               .ToFeedIterator();

        var reactions = new List<ReactionCount>();
        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            foreach (var item in response)
            {
                reactions.Add(item);
            }
        }

        return new OkObjectResult(reactions);
    }
}
