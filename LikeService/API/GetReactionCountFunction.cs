***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using Microsoft.Azure.Cosmos;
using LikeService.Models;
***REMOVED***

namespace LikeService.API;

public static class GetReactionCountFunction
***REMOVED***
    [FunctionName(nameof(GetReactionCountFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "reaction/count")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName2, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    ***REMOVED***
        string postId = req.Query[nameof(postId)].ToString();
        string commentId = req.Query[nameof(commentId)].ToString();

        log.LogInformation("***REMOVED***0***REMOVED*** function processed a request for post: ***REMOVED***1***REMOVED***.", nameof(GetReactionCountFunction), postId);

        var feed = GetReactionCountByPostIdAndCommentId(cosmosClient, postId, commentId);

        var reactions = new List<ReactionCountDto>();
        while (feed.HasMoreResults)
        ***REMOVED***
            var response = await feed.ReadNextAsync();
            foreach (var item in response)
            ***REMOVED***
                reactions.Add(ReactionCountDto.Map(item));
        ***REMOVED***
    ***REMOVED***

        return new OkObjectResult(reactions);
***REMOVED***

    private static FeedIterator<ReactionCount> GetReactionCountByPostIdAndCommentId(CosmosClient cosmosClient, string postId, string commentId)
    ***REMOVED***
        var container = cosmosClient.GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2);
        var query = $"SELECT * FROM ***REMOVED***nameof(CosmosDbConfigs.ContainerName2)***REMOVED*** p WHERE p.***REMOVED***nameof(ReactionCount.PostId)***REMOVED*** = @partitionKey";
        if (!string.IsNullOrEmpty(commentId))
            query += $" AND p.***REMOVED***nameof(ReactionCount.CommentId)***REMOVED*** = @commentId";

       return  container.GetItemQueryIterator<ReactionCount>(
                    queryDefinition: new QueryDefinition(
            query: query
        )
        .WithParameter("@partitionKey", postId)
        .WithParameter("@commentId", commentId));
***REMOVED***
***REMOVED***

public record ReactionCountDto
***REMOVED***
    public string Id ***REMOVED*** get; init; ***REMOVED***
    public string PostId ***REMOVED*** get; init; ***REMOVED***
    public string CommentId ***REMOVED*** get; set; ***REMOVED***
    public ReactionType ReactionType ***REMOVED*** get; init; ***REMOVED***
    public int Count ***REMOVED*** get; init; ***REMOVED*** = 0;
    public static ReactionCountDto Map(ReactionCount data)
    ***REMOVED***
        return new ReactionCountDto
        ***REMOVED***
            Id = data.Id,
            PostId = data.PostId,
            CommentId = data.CommentId,
            Count = data.Count,
            ReactionType = data.ReactionType
    ***REMOVED***;
***REMOVED***
***REMOVED***
