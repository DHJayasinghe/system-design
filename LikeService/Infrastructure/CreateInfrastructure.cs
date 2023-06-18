***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using Microsoft.Azure.Cosmos;
***REMOVED***
using LikeService.Models;
using Azure.Messaging.ServiceBus.Administration;
***REMOVED***
using LikeService.Configs;

namespace LikeService.Infrastructure;

public class CreateInfrastructure
***REMOVED***
    private readonly IConfiguration _configuration;

    public CreateInfrastructure(IConfiguration configuration) => _configuration = configuration;

    [FunctionName(nameof(CreateInfrastructure))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "reactions/infrastructure")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    ***REMOVED***
        log.LogInformation("C# HTTP trigger function processed a request.");

        var tasks = new List<Task>();
        await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosDbConfigs.DatabaseName);
        tasks.Add(cosmosClient.GetDatabase(CosmosDbConfigs.DatabaseName)
            .DefineContainer(name: CosmosDbConfigs.ContainerName, partitionKeyPath: $"/***REMOVED***nameof(Reaction.PostId)***REMOVED***")
            .WithUniqueKey()
                .Path($"/***REMOVED***nameof(Reaction.UserId)***REMOVED***")
                .Path($"/***REMOVED***nameof(Reaction.CommentId)***REMOVED***")
            .Attach()
            .CreateIfNotExistsAsync());
        tasks.Add(cosmosClient.GetDatabase(CosmosDbConfigs.DatabaseName)
           .DefineContainer(name: CosmosDbConfigs.ContainerName2, partitionKeyPath: $"/***REMOVED***nameof(ReactionCount.PostId)***REMOVED***")
           .WithUniqueKey()
               .Path($"/***REMOVED***nameof(ReactionCount.CommentId)***REMOVED***")
           .Attach()
           .CreateIfNotExistsAsync());

        await Task.WhenAll(tasks);

        var serviceBusClient = new ServiceBusAdministrationClient(_configuration.GetConnectionString(ServiceBusConfigs.ConnectionName));
        if (!await serviceBusClient.TopicExistsAsync(ServiceBusConfigs.TopicName))
            await serviceBusClient.CreateTopicAsync(new CreateTopicOptions(ServiceBusConfigs.TopicName)
            ***REMOVED***
                DefaultMessageTimeToLive = System.TimeSpan.FromDays(7),

        ***REMOVED***);
        if (!await serviceBusClient.SubscriptionExistsAsync(ServiceBusConfigs.TopicName, ServiceBusConfigs.SubscriptionName))
        ***REMOVED***
            await serviceBusClient.CreateSubscriptionAsync(new CreateSubscriptionOptions(ServiceBusConfigs.TopicName, ServiceBusConfigs.SubscriptionName)
            ***REMOVED***
                RequiresSession = true,
                DefaultMessageTimeToLive = System.TimeSpan.FromDays(7)
        ***REMOVED***);
    ***REMOVED***

        return new OkResult();
***REMOVED***
***REMOVED***