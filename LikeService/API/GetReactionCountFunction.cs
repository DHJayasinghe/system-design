using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using LikeService.Models;
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

        var feed = GetReactionCountByPostIdAndCommentId(cosmosClient, postId, commentId);

        var reactions = new List<ReactionCountDto>();
        while (feed.HasMoreResults)
        {
            var response = await feed.ReadNextAsync();
            foreach (var item in response)
            {
                reactions.Add(ReactionCountDto.Map(item));
            }
        }

        return new OkObjectResult(reactions);
    }

    private static FeedIterator<ReactionCount> GetReactionCountByPostIdAndCommentId(CosmosClient cosmosClient, string postId, string commentId)
    {
        var container = cosmosClient.GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2);
        var query = $"SELECT * FROM {nameof(CosmosDbConfigs.ContainerName2)} p WHERE p.{nameof(ReactionCount.PostId)} = @partitionKey";
        if (!string.IsNullOrEmpty(commentId))
            query += $" AND p.{nameof(ReactionCount.CommentId)} = @commentId";

       return  container.GetItemQueryIterator<ReactionCount>(
                    queryDefinition: new QueryDefinition(
            query: query
        )
        .WithParameter("@partitionKey", postId)
        .WithParameter("@commentId", commentId));
    }
}

public record ReactionCountDto
{
    public string Id { get; init; }
    public string PostId { get; init; }
    public string CommentId { get; set; }
    public ReactionType ReactionType { get; init; }
    public int Count { get; init; } = 0;
    public static ReactionCountDto Map(ReactionCount data)
    {
        return new ReactionCountDto
        {
            Id = data.Id,
            PostId = data.PostId,
            CommentId = data.CommentId,
            Count = data.Count,
            ReactionType = data.ReactionType
        };
    }
}
