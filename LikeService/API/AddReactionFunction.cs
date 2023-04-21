using System.IO;
***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using Newtonsoft.Json;
using LikeService.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.WebJobs.ServiceBus;
using Azure.Messaging.ServiceBus;
***REMOVED***
using LikeService.Events;

namespace LikeService;

public static class AddReactionFunction
***REMOVED***
    [FunctionName(nameof(AddReactionFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "reaction")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        [ServiceBus(ServiceBusConfigs.TopicName, Connection = ServiceBusConfigs.ConnectionName, EntityType = ServiceBusEntityType.Topic)] IAsyncCollector<ServiceBusMessage> serviceBusClient,
        ILogger log)
    ***REMOVED***
        string requestBody = new StreamReader(req.Body).ReadToEnd();
        var currentReaction = JsonConvert.DeserializeObject<Reaction>(requestBody).WithDefaults();

        if (!Enum.IsDefined(currentReaction.ReactionType))
            return new BadRequestObjectResult("Provided reaction type is not valid");

        log.LogInformation("***REMOVED***0***REMOVED*** function processed a request for post: ***REMOVED***1***REMOVED*** from user: ***REMOVED***2***REMOVED***.", nameof(AddReactionFunction), currentReaction.PostId, currentReaction.UserId);

        var existingReaction = await GetReactionByIdAsync(cosmosClient, currentReaction);

        var result = await cosmosClient
            .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName)
            .UpsertItemAsync(currentReaction, new PartitionKey(currentReaction.PostId.ToString()));

        await RaiseIntegrationEvent(serviceBusClient, currentReaction, existingReaction);

        return new OkResult();
***REMOVED***

    private static async Task<Reaction> GetReactionByIdAsync(CosmosClient cosmosClient, Reaction data)
    ***REMOVED***
***REMOVED***
        ***REMOVED***
            return (await cosmosClient
            .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName)
            .ReadItemAsync<Reaction>(data.Id, new PartitionKey(data.PostId.ToString())))
            .Resource;
    ***REMOVED***
***REMOVED***
        ***REMOVED***
            if (!ex.Message.Contains("NotFound (404)")) throw;
            return null;
    ***REMOVED***
***REMOVED***

    private static async Task RaiseIntegrationEvent(IAsyncCollector<ServiceBusMessage> serviceBusClient, Reaction curentState, Reaction previousState)
    ***REMOVED***
        if (curentState.ReactionType == previousState?.ReactionType) return;

        var integrationEvent = new ReactionChangedIntegrationEvent()
        ***REMOVED***
            Id = curentState.Id,
            PostId = curentState.PostId,
            UserId = curentState.UserId,
            CommentId = curentState.CommentId,
            ReactionType = curentState.ReactionType,
            PreviousReactionType = previousState?.ReactionType,
            State = previousState is null ? State.Added : State.Modified,
    ***REMOVED***;
        await serviceBusClient.AddAsync(new ServiceBusMessage(JsonConvert.SerializeObject(integrationEvent))
        ***REMOVED***
            CorrelationId = Guid.NewGuid().ToString(),
            ContentType = "***REMOVED***lication/json",
            SessionId = curentState.PostId
    ***REMOVED***);
***REMOVED***
***REMOVED***