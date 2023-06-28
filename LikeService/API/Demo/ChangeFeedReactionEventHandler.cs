using System.Collections.Generic;
using System.Threading.Tasks;
using LikeService.Configs;
using LikeService.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;

namespace LikeService.API;

public static class ChangeFeedReactionEventHandler
{
    [Disable]
    [FunctionName(nameof(ChangeFeedReactionEventHandler))]
    public static async Task Run(
        [CosmosDBTrigger(
            databaseName:  CosmosDbConfigs.DatabaseName,
            containerName: CosmosDbConfigs.ContainerName,
            Connection = CosmosDbConfigs.ConnectionName,
            CreateLeaseContainerIfNotExists = true,
            LeaseContainerName = "reaction-count-lease")] IReadOnlyList<Reaction> input,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName2, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient)
    {
        if (input == null || input.Count <= 0) return;

        foreach (var item in input)
        {
            var reactionCountForCurrentReaction = await GetReactionCountByIdAsync(cosmosClient, ReactionCount.Map(item));
            reactionCountForCurrentReaction.Increment(item.ReactionType);
            await SaveChanges(cosmosClient, reactionCountForCurrentReaction);
        }
    }

    private static async Task SaveChanges(CosmosClient cosmosClient, ReactionCount reactionCountForCurrentReaction) => await cosmosClient
        .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2)
        .UpsertItemAsync(reactionCountForCurrentReaction, new PartitionKey(reactionCountForCurrentReaction.PostId.ToString()));

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
