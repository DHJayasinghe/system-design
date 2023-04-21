using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using LikeService.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs.ServiceBus;
using Azure.Messaging.ServiceBus;
using System;
using LikeService.Events;

namespace LikeService;

public static class AddReactionFunction
{
    [FunctionName(nameof(AddReactionFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "reaction")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        [ServiceBus(ServiceBusConfigs.TopicName, Connection = ServiceBusConfigs.ConnectionName, EntityType = ServiceBusEntityType.Topic)] IAsyncCollector<ServiceBusMessage> serviceBusClient,
        ILogger log)
    {
        string requestBody = new StreamReader(req.Body).ReadToEnd();
        var currentReaction = JsonConvert.DeserializeObject<Reaction>(requestBody).WithDefaults();

        if (!Enum.IsDefined(currentReaction.ReactionType))
            return new BadRequestObjectResult("Provided reaction type is not valid");

        log.LogInformation("{0} function processed a request for post: {1} from user: {2}.", nameof(AddReactionFunction), currentReaction.PostId, currentReaction.UserId);

        var existingReaction = await GetReactionByIdAsync(cosmosClient, currentReaction);

        var result = await cosmosClient
            .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName)
            .UpsertItemAsync(currentReaction, new PartitionKey(currentReaction.PostId.ToString()));

        await RaiseIntegrationEvent(serviceBusClient, currentReaction, existingReaction);

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

    private static async Task RaiseIntegrationEvent(IAsyncCollector<ServiceBusMessage> serviceBusClient, Reaction curentState, Reaction previousState)
    {
        if (curentState.ReactionType == previousState?.ReactionType) return;

        var integrationEvent = new ReactionChangedIntegrationEvent()
        {
            Id = curentState.Id,
            PostId = curentState.PostId,
            UserId = curentState.UserId,
            CommentId = curentState.CommentId,
            ReactionType = curentState.ReactionType,
            PreviousReactionType = previousState?.ReactionType,
            State = previousState is null ? State.Added : State.Modified,
        };
        await serviceBusClient.AddAsync(new ServiceBusMessage(JsonConvert.SerializeObject(integrationEvent))
        {
            CorrelationId = Guid.NewGuid().ToString(),
            ContentType = "application/json",
            SessionId = curentState.PostId
        });
    }
}