using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;

namespace LikeService.Infrastructure;

public class CreateInfrastructure
{
    private readonly IConfiguration _configuration;

    public CreateInfrastructure(IConfiguration configuration) => _configuration = configuration;

    [FunctionName(nameof(CreateInfrastructure))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "infrastructure")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        var client = new CosmosClient(_configuration.GetConnectionString("CosmosDBConnection"));
        await client.CreateDatabaseIfNotExistsAsync("like-service");
        await client.GetDatabase("like-service")
            .DefineContainer(name: "reaction", partitionKeyPath: "/postId")
            .WithUniqueKey()
                .Path("/userId")
            .Attach()
            .CreateIfNotExistsAsync();
        return new OkResult();
    }
}