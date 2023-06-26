using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using LikeService.Models;
using System.Collections.Generic;
using LikeService.Configs;

namespace LikeService.API;

public static class GetReactionsFunction
***REMOVED***
    [FunctionName(nameof(GetReactionsFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "reactions")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName2, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
***REMOVED***
        string postId = req.Query[nameof(postId)].ToString();
        string commentId = req.Query[nameof(commentId)].ToString();

        log.LogInformation("***REMOVED***0***REMOVED*** function processed a request for post: ***REMOVED***1***REMOVED***.", nameof(GetReactionsFunction), postId);

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

        return container.GetItemQueryIterator<ReactionCount>(
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
    public int LikeCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int HeartCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int WowCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int CareCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int LaughCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int SadCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int AngryCount ***REMOVED*** get; set; ***REMOVED*** = 0;
    public int TotalReactions => LikeCount + HeartCount + WowCount + CareCount + LaughCount + SadCount + AngryCount;

    public static ReactionCountDto Map(ReactionCount data)
***REMOVED***
        return new ReactionCountDto
    ***REMOVED***
            Id = data.Id,
            PostId = data.PostId,
            CommentId = data.CommentId,
            LikeCount = data.LikeCount,
            HeartCount = data.HeartCount,
            WowCount = data.WowCount,
            CareCount = data.CareCount,
            LaughCount = data.LaughCount,
            SadCount = data.SadCount,
            AngryCount = data.AngryCount
***REMOVED***
    ***REMOVED***
***REMOVED***
