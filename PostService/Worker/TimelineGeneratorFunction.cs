using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PostService.Configs;
using PostService.Models;

namespace PostService.Worker;

public static class TimelineGeneratorFunction
{
    [FunctionName(nameof(TimelineGeneratorFunction))]
    public static async Task Run(
        [CosmosDBTrigger(
            databaseName:  CosmosDbConfigs.DatabaseName,
            containerName: CosmosDbConfigs.PostsContainer,
            Connection = CosmosDbConfigs.ConnectionName,
            CreateLeaseContainerIfNotExists = true,
            LeaseContainerName = $"{CosmosDbConfigs.PostsContainer}-lease")] IReadOnlyList<Post> input,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.PostsContainer, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} function processed a request for post.", nameof(TimelineGeneratorFunction));

        //var reactionCountForCurrentReaction = await GetReactionCountByIdAsync(cosmosClient, ReactionCount.Map(@event));

        //if (@event.State != State.Removed)
        //{
        //    reactionCountForCurrentReaction.Increment(@event.ReactionType);
        //    if (@event.PreviousReactionType.HasValue)
        //        reactionCountForCurrentReaction.Decrement(@event.PreviousReactionType.Value);
        //    await SaveChanges(cosmosClient, reactionCountForCurrentReaction);
        //}
        //if (@event.State == State.Removed)
        //{
        //    reactionCountForCurrentReaction.Decrement(@event.ReactionType);
        //    await SaveChanges(cosmosClient, reactionCountForCurrentReaction);
        //}
    }
}
