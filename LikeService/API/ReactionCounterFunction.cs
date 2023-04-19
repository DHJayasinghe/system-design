using LikeService.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LikeService.API;

public class ReactionCounterFunction
{
    [FunctionName(nameof(ReactionCounterFunction))]
    public async Task Run([ServiceBusTrigger(ServiceBusConfigs.TopicName, ServiceBusConfigs.SubscriptionName, Connection = ServiceBusConfigs.ConnectionName, IsSessionsEnabled = true)] Reaction reaction,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName2, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("{0} function processed a request for post: {1}.", nameof(ReactionCounterFunction), reaction.PostId);

        var newStats = ReactionCount.Map(reaction);

        try
        {
            var existingStats = await cosmosClient
               .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2).ReadItemAsync<ReactionCount>(newStats.Id, new PartitionKey(reaction.PostId.ToString()));
            newStats.Count = existingStats.Resource.Count + 1;
        }
        catch (System.Exception ex)
        {
            if (!ex.Message.Contains("NotFound (404)")) throw;
            newStats.Count = 1;
        }


        await cosmosClient
          .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName2)
          .UpsertItemAsync(newStats, new PartitionKey(reaction.PostId.ToString()));
    }
}
