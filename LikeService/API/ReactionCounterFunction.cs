using LikeService.Configs;
using LikeService.Events;
using LikeService.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
***REMOVED***
***REMOVED***

namespace LikeService.API;

public static class ReactionCounterFunction
***REMOVED***
    [FunctionName(nameof(ReactionCounterFunction))]
    public static async Task Run([ServiceBusTrigger(ServiceBusConfigs.TopicName, ServiceBusConfigs.SubscriptionName, Connection = ServiceBusConfigs.ConnectionName, IsSessionsEnabled = true)] ReactionChangedIntegrationEvent @event,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName2, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** function processed a request for post: ***REMOVED***1***REMOVED***.", nameof(ReactionCounterFunction), @event.PostId);

        var reactionCountForCurrentReaction = await GetReactionCountByIdAsync(cosmosClient, ReactionCount.Map(@event));

        if (@event.State != State.Removed)
        ***REMOVED***
            reactionCountForCurrentReaction.Increment(@event.ReactionType);
            if (@event.PreviousReactionType.HasValue)
                reactionCountForCurrentReaction.Decrement(@event.PreviousReactionType.Value);
            await SaveChanges(@event, cosmosClient, reactionCountForCurrentReaction);
    ***REMOVED***
        if (@event.State == State.Removed)
        ***REMOVED***
            reactionCountForCurrentReaction.Decrement(@event.ReactionType);
            await SaveChanges(@event, cosmosClient, reactionCountForCurrentReaction);
    ***REMOVED***
***REMOVED***

    private static async Task SaveChanges(ReactionChangedIntegrationEvent @event, CosmosClient cosmosClient, ReactionCount reactionCountForCurrentReaction)
    ***REMOVED***
        await cosmosClient
                    .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2)
                    .UpsertItemAsync(reactionCountForCurrentReaction, new PartitionKey(@event.PostId.ToString()));
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
