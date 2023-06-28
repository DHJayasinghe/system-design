using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using LikeService.Models;
using System;
using Microsoft.Azure.WebJobs.ServiceBus;
using Azure.Messaging.ServiceBus;
using LikeService.Events;
using LikeService.Configs;

namespace LikeService.API;

public static class RemoveReactionFunction
{
    [FunctionName(nameof(RemoveReactionFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "reactions")] RemoveReactionRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        [ServiceBus(ServiceBusConfigs.TopicName, Connection = ServiceBusConfigs.ConnectionName, EntityType = ServiceBusEntityType.Topic)] IAsyncCollector<ServiceBusMessage> serviceBusClient,
        ILogger log)
    {
        var request = req.Map().WithDefaults();

        log.LogInformation("{0} function processed a request for post: {1} from user: {2}.", nameof(RemoveReactionFunction), request.PostId, request.UserId);

        var existingReaction = await GetReactionByIdAsync(cosmosClient, request);
        if (existingReaction is null) return new BadRequestObjectResult("Reaction does not exist");

        await cosmosClient
            .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName)
            .DeleteItemAsync<Reaction>(request.Id, new PartitionKey(request.PostId.ToString()));

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

public record RemoveReactionRequest
{
    public string PostId { get; init; }
    public string CommentId { get; init; }
    public string UserId { get; init; }
}