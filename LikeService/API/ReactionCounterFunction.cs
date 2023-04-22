using LikeService.Configs;
using LikeService.Events;
using LikeService.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LikeService.API;

public static class ReactionCounterFunction
{
    [FunctionName(nameof(ReactionCounterFunction))]
    public static async Task Run([ServiceBusTrigger(ServiceBusConfigs.TopicName, ServiceBusConfigs.SubscriptionName, Connection = ServiceBusConfigs.ConnectionName, IsSessionsEnabled = true)] ReactionChangedIntegrationEvent @event,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName2, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} function processed a request for post: {1}.", nameof(ReactionCounterFunction), @event.PostId);

        var reactionCountForCurrentReaction = await GetReactionCountByIdAsync(cosmosClient, ReactionCount.Map(@event));

        if (@event.State != State.Removed)
        {
            reactionCountForCurrentReaction.Increment(@event.ReactionType);
            if (@event.PreviousReactionType.HasValue)
                reactionCountForCurrentReaction.Decrement(@event.PreviousReactionType.Value);
            await SaveChanges(@event, cosmosClient, reactionCountForCurrentReaction);
        }
        if (@event.State == State.Removed)
        {
            reactionCountForCurrentReaction.Decrement(@event.ReactionType);
            await SaveChanges(@event, cosmosClient, reactionCountForCurrentReaction);
        }
    }

    private static async Task SaveChanges(ReactionChangedIntegrationEvent @event, CosmosClient cosmosClient, ReactionCount reactionCountForCurrentReaction)
    {
        await cosmosClient
                    .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2)
                    .UpsertItemAsync(reactionCountForCurrentReaction, new PartitionKey(@event.PostId.ToString()));
    }

    private static async Task<ReactionCount> GetReactionCountByIdAsync(CosmosClient cosmosClient, ReactionCount reactionCount)
    {
        try
        {
            return (await cosmosClient
               .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2).ReadItemAsync<ReactionCount>(reactionCount.Id, new PartitionKey(reactionCount.PostId.ToString())))
               .Resource;
        }
        catch (System.Exception ex)
        {
            if (!ex.Message.Contains("NotFound (404)")) throw;
            return reactionCount;
        }
    }
}
