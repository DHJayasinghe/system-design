***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using Microsoft.Azure.Cosmos;
***REMOVED***
using Azure.Messaging.ServiceBus.Administration;
***REMOVED***
using LikeService.Configs;

namespace LikeService.Infrastructure;

public class RemoveInfrastructure
***REMOVED***
    private readonly IConfiguration _configuration;

    public RemoveInfrastructure(IConfiguration configuration) => _configuration = configuration;

    [FunctionName(nameof(RemoveInfrastructure))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "infrastructure")] HttpRequest req,
        [CosmosDB(databaseName: CosmosDbConfigs.DatabaseName, containerName: CosmosDbConfigs.ContainerName, Connection = CosmosDbConfigs.ConnectionName)] CosmosClient client,
        ILogger log)
    ***REMOVED***
        log.LogInformation("C# HTTP trigger function processed a request.");

        var tasksToProcess = new List<Task>();
        tasksToProcess.Add(client.GetDatabase(CosmosDbConfigs.DatabaseName).DeleteAsync());

        var serviceBusClient = new ServiceBusAdministrationClient(_configuration.GetConnectionString(ServiceBusConfigs.ConnectionName));
        if (await serviceBusClient.TopicExistsAsync(ServiceBusConfigs.TopicName))
            tasksToProcess.Add(serviceBusClient.DeleteTopicAsync(ServiceBusConfigs.TopicName));

        await Task.WhenAll(tasksToProcess);

        return new OkResult();
***REMOVED***
***REMOVED***
