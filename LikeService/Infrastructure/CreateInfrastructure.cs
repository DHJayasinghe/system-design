using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using LikeService.Models;
using Azure.Messaging.ServiceBus.Administration;
using System.Collections.Generic;
using LikeService.Configs;

namespace LikeService.Infrastructure;

public class CreateInfrastructure
{
    private readonly IConfiguration _configuration;

    public CreateInfrastructure(IConfiguration configuration) => _configuration = configuration;

    [FunctionName(nameof(CreateInfrastructure))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "infrastructure")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        var tasks = new List<Task>();
        await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosDbConfigs.DatabaseName);
        tasks.Add(cosmosClient.GetDatabase(CosmosDbConfigs.DatabaseName)
            .DefineContainer(name: CosmosDbConfigs.ContainerName, partitionKeyPath: $"/{nameof(Reaction.PostId)}")
            .WithUniqueKey()
                .Path($"/{nameof(Reaction.UserId)}")
                .Path($"/{nameof(Reaction.CommentId)}")
            .Attach()
            .CreateIfNotExistsAsync());
        tasks.Add(cosmosClient.GetDatabase(CosmosDbConfigs.DatabaseName)
           .DefineContainer(name: CosmosDbConfigs.ContainerName2, partitionKeyPath: $"/{nameof(ReactionCount.PostId)}")
           .WithUniqueKey()
               .Path($"/{nameof(ReactionCount.CommentId)}")
           .Attach()
           .CreateIfNotExistsAsync());

        await Task.WhenAll(tasks);

        var serviceBusClient = new ServiceBusAdministrationClient(_configuration.GetConnectionString(ServiceBusConfigs.ConnectionName));
        if (!await serviceBusClient.TopicExistsAsync(ServiceBusConfigs.TopicName))
            await serviceBusClient.CreateTopicAsync(new CreateTopicOptions(ServiceBusConfigs.TopicName)
            {
                DefaultMessageTimeToLive = System.TimeSpan.FromDays(7),

            });
        if (!await serviceBusClient.SubscriptionExistsAsync(ServiceBusConfigs.TopicName, ServiceBusConfigs.SubscriptionName))
        {
            await serviceBusClient.CreateSubscriptionAsync(new CreateSubscriptionOptions(ServiceBusConfigs.TopicName, ServiceBusConfigs.SubscriptionName)
            {
                RequiresSession = true,
                DefaultMessageTimeToLive = System.TimeSpan.FromDays(7)
            });
        }

        return new OkResult();
    }
}