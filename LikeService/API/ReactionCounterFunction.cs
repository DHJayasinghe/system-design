using LikeService.Events;
using LikeService.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
***REMOVED***
***REMOVED***

namespace LikeService.API;

public class ReactionCounterFunction
***REMOVED***
    [FunctionName(nameof(ReactionCounterFunction))]
    public async Task Run([ServiceBusTrigger(ServiceBusConfigs.TopicName, ServiceBusConfigs.SubscriptionName, Connection = ServiceBusConfigs.ConnectionName, IsSessionsEnabled = true)] ReactionChangedIntegrationEvent @event,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName2, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** function processed a request for post: ***REMOVED***1***REMOVED***.", nameof(ReactionCounterFunction), @event.PostId);

        if (@event.State != State.Removed)
        ***REMOVED***
            await IncrementCurrentReactionCounter(@event, cosmosClient);
            await DecrementPreviousReactionCounterIfAny(@event, cosmosClient);
    ***REMOVED***
        if (@event.State == State.Removed)
        ***REMOVED***
            await DecrementCurrentReactionCounter(@event, cosmosClient);
    ***REMOVED***
***REMOVED***

    private static async Task IncrementCurrentReactionCounter(ReactionChangedIntegrationEvent @event, CosmosClient cosmosClient)
    ***REMOVED***
        var reactionCountForCurrentReaction = await GetReactionCountByIdAsync(cosmosClient, ReactionCount.Map(@event));
        reactionCountForCurrentReaction.Count++;

        await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2)
          .UpsertItemAsync(reactionCountForCurrentReaction, new PartitionKey(@event.PostId.ToString()));
***REMOVED***

    private static async Task DecrementCurrentReactionCounter(ReactionChangedIntegrationEvent @event, CosmosClient cosmosClient)
    ***REMOVED***
        var reactionCountForCurrentReaction = await GetReactionCountByIdAsync(cosmosClient, ReactionCount.Map(@event));
        reactionCountForCurrentReaction.Count--;

        await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2)
          .UpsertItemAsync(reactionCountForCurrentReaction, new PartitionKey(@event.PostId.ToString()));
***REMOVED***

    private static async Task DecrementPreviousReactionCounterIfAny(ReactionChangedIntegrationEvent @event, CosmosClient cosmosClient)
    ***REMOVED***
        if (!@event.PreviousReactionType.HasValue) return;

        var reactionCountForPreviousReaction = await GetReactionCountByIdAsync(cosmosClient, ReactionCount.Map(@event, @event.PreviousReactionType));
        reactionCountForPreviousReaction.Count--;

        await cosmosClient
         .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2)
         .UpsertItemAsync(reactionCountForPreviousReaction, new PartitionKey(reactionCountForPreviousReaction.PostId.ToString()));
***REMOVED***

    private static async Task<ReactionCount> GetReactionCountByIdAsync(CosmosClient cosmosClient, ReactionCount reactionCount)
    ***REMOVED***
***REMOVED***
        ***REMOVED***
            return (await cosmosClient
               .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2).ReadItemAsync<ReactionCount>(reactionCount.Id, new PartitionKey(reactionCount.PostId.ToString())))
               .Resource;
    ***REMOVED***
        catch (System.Exception ex)
        ***REMOVED***
            if (!ex.Message.Contains("NotFound (404)")) throw;
            return reactionCount;
    ***REMOVED***
***REMOVED***
***REMOVED***
