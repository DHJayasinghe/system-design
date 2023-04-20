using LikeService.Events;
using LikeService.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LikeService.API;

public class ReactionCounterFunction
{
    [FunctionName(nameof(ReactionCounterFunction))]
    public async Task Run([ServiceBusTrigger(ServiceBusConfigs.TopicName, ServiceBusConfigs.SubscriptionName, Connection = ServiceBusConfigs.ConnectionName, IsSessionsEnabled = true)] ReactionChangedIntegrationEvent @event,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName2, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} function processed a request for post: {1}.", nameof(ReactionCounterFunction), @event.PostId);

        if (@event.State != State.Removed)
        {
            await IncrementCurrentReactionCounter(@event, cosmosClient);
            await DecrementPreviousReactionCounterIfAny(@event, cosmosClient);
        }
        if (@event.State == State.Removed)
        {
            await DecrementCurrentReactionCounter(@event, cosmosClient);
        }
    }

    private static async Task IncrementCurrentReactionCounter(ReactionChangedIntegrationEvent @event, CosmosClient cosmosClient)
    {
        var reactionCountForCurrentReaction = await GetReactionCountByIdAsync(cosmosClient, ReactionCount.Map(@event));
        reactionCountForCurrentReaction.Count++;

        await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2)
          .UpsertItemAsync(reactionCountForCurrentReaction, new PartitionKey(@event.PostId.ToString()));
    }

    private static async Task DecrementCurrentReactionCounter(ReactionChangedIntegrationEvent @event, CosmosClient cosmosClient)
    {
        var reactionCountForCurrentReaction = await GetReactionCountByIdAsync(cosmosClient, ReactionCount.Map(@event));
        reactionCountForCurrentReaction.Count--;

        await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2)
          .UpsertItemAsync(reactionCountForCurrentReaction, new PartitionKey(@event.PostId.ToString()));
    }

    private static async Task DecrementPreviousReactionCounterIfAny(ReactionChangedIntegrationEvent @event, CosmosClient cosmosClient)
    {
        if (!@event.PreviousReactionType.HasValue) return;

        var reactionCountForPreviousReaction = await GetReactionCountByIdAsync(cosmosClient, ReactionCount.Map(@event, @event.PreviousReactionType));
        reactionCountForPreviousReaction.Count--;

        await cosmosClient
         .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2)
         .UpsertItemAsync(reactionCountForPreviousReaction, new PartitionKey(reactionCountForPreviousReaction.PostId.ToString()));
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
