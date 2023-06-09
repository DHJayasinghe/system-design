using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using PostService.Configs;
using PostService.Models;
using Azure.Messaging.ServiceBus.Administration;

namespace PostService.Infrastructure;

public class CreateInfrastructure
{
    private readonly IConfiguration _configuration;

    public CreateInfrastructure(IConfiguration configuration) => _configuration = configuration;

    [FunctionName(nameof(CreateInfrastructure))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "posts/infrastructure")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.PostsContainer, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient cosmosClient,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        var tasks = new List<Task>();
        await cosmosClient.CreateDatabaseIfNotExistsAsync(CosmosDbConfigs.DatabaseName);
        tasks.Add(cosmosClient.GetDatabase(CosmosDbConfigs.DatabaseName)
            .DefineContainer(name: CosmosDbConfigs.PostsContainer, partitionKeyPath: $"/{nameof(Post.PostId)}")
            .WithUniqueKey()
                .Path($"/{nameof(Post.AuthorId)}")
            .Attach()
            .CreateIfNotExistsAsync());
        tasks.Add(cosmosClient.GetDatabase(CosmosDbConfigs.DatabaseName)
            .DefineContainer(name: CosmosDbConfigs.TimelineContainer, partitionKeyPath: $"/{nameof(TimelineActivity.Key)}")
            .CreateIfNotExistsAsync());
        tasks.Add(cosmosClient.GetDatabase(CosmosDbConfigs.DatabaseName)
            .DefineContainer(name: CosmosDbConfigs.CommentContainer, partitionKeyPath: $"/{nameof(Comment.PostId)}")
            .WithUniqueKey()
                .Path($"/{nameof(Comment.CommentId)}")
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
                DefaultMessageTimeToLive = System.TimeSpan.FromDays(7)
            });
        }

        return new OkResult();
    }
}