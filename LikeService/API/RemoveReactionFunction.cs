using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using LikeService.Models;
using System;
using Microsoft.Azure.WebJobs.ServiceBus;
using Azure.Messaging.ServiceBus;
using LikeService.Events;

namespace LikeService;

public static class RemoveReactionFunction
{
    [FunctionName(nameof(RemoveReactionFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "reaction")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        [ServiceBus(ServiceBusConfigs.TopicName, Connection = ServiceBusConfigs.ConnectionName, EntityType = ServiceBusEntityType.Topic)] IAsyncCollector<ServiceBusMessage> serviceBusClient,
        ILogger log)
    {
        string requestBody = new StreamReader(req.Body).ReadToEnd();
        var data = JsonConvert.DeserializeObject<Reaction>(requestBody).WithDefaults();

        log.LogInformation("{0} function processed a request for post: {1} from user: {2}.", nameof(RemoveReactionFunction), data.PostId, data.UserId);

        var existingReaction = await GetReactionByIdAsync(cosmosClient, data);
        if (existingReaction is null) return new BadRequestObjectResult("Reaction does not exist");

        await cosmosClient
            .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName)
            .DeleteItemAsync<Reaction>(data.Id, new PartitionKey(data.PostId.ToString()));

        await RaiseIntegrationEvent(serviceBusClient, existingReaction);

        return new OkResult();
    }


    private static async Task<Reaction> GetReactionByIdAsync(CosmosClient cosmosClient, Reaction data)
    {
        try
        {
            return (await cosmosClient
            .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName)
            .ReadItemAsync<Reaction>(data.Id, new PartitionKey(data.PostId.ToString())))
            .Resource;
        }
        catch (Exception ex)
        {
            if (!ex.Message.Contains("NotFound (404)")) throw;
            return null;
        }
    }

    private static async Task RaiseIntegrationEvent(IAsyncCollector<ServiceBusMessage> serviceBusClient, Reaction curentState)
    {
        var integrationEvent = new ReactionChangedIntegrationEvent()
        {
            Id = curentState.Id,
            PostId = curentState.PostId,
            UserId = curentState.UserId,
            CommentId = curentState.CommentId,
            ReactionType = curentState.ReactionType,
            State = State.Removed,
        };
        await serviceBusClient.AddAsync(new ServiceBusMessage(JsonConvert.SerializeObject(integrationEvent))
        {
            CorrelationId = Guid.NewGuid().ToString(),
            ContentType = "application/json",
            SessionId = curentState.PostId
        });
    }
}
