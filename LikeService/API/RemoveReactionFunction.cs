using System.IO;
***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using Newtonsoft.Json;
using Microsoft.Azure.Cosmos;
using LikeService.Models;
***REMOVED***
using Microsoft.Azure.WebJobs.ServiceBus;
using Azure.Messaging.ServiceBus;
using LikeService.Events;

namespace LikeService;

public static class RemoveReactionFunction
***REMOVED***
    [FunctionName(nameof(RemoveReactionFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "reaction")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        [ServiceBus(ServiceBusConfigs.TopicName, Connection = ServiceBusConfigs.ConnectionName, EntityType = ServiceBusEntityType.Topic)] IAsyncCollector<ServiceBusMessage> serviceBusClient,
        ILogger log)
    ***REMOVED***
        string requestBody = new StreamReader(req.Body).ReadToEnd();
        var data = JsonConvert.DeserializeObject<Reaction>(requestBody).WithDefaults();

        log.LogInformation("***REMOVED***0***REMOVED*** function processed a request for post: ***REMOVED***1***REMOVED*** from user: ***REMOVED***2***REMOVED***.", nameof(RemoveReactionFunction), data.PostId, data.UserId);

        var existingReaction = await GetReactionByIdAsync(cosmosClient, data);
        if (existingReaction is null) return new BadRequestObjectResult("Reaction does not exist");

        await cosmosClient
            .GetContainer(CosmosDbConfigs.DatabaseName, CosmosDbConfigs.ContainerName)
            .DeleteItemAsync<Reaction>(data.Id, new PartitionKey(data.PostId.ToString()));

        await RaiseIntegrationEvent(serviceBusClient, existingReaction);

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

    private static async Task RaiseIntegrationEvent(IAsyncCollector<ServiceBusMessage> serviceBusClient, Reaction curentState)
    ***REMOVED***
        var integrationEvent = new ReactionChangedIntegrationEvent()
        ***REMOVED***
            Id = curentState.Id,
            PostId = curentState.PostId,
            UserId = curentState.UserId,
            CommentId = curentState.CommentId,
            ReactionType = curentState.ReactionType,
            State = State.Removed,
    ***REMOVED***;
        await serviceBusClient.AddAsync(new ServiceBusMessage(JsonConvert.SerializeObject(integrationEvent))
        ***REMOVED***
            CorrelationId = Guid.NewGuid().ToString(),
            ContentType = "***REMOVED***lication/json",
            SessionId = curentState.PostId
    ***REMOVED***);
***REMOVED***
***REMOVED***
