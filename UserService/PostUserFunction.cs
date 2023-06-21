using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using UserService.Models;
using System.Threading.Tasks;
using Azure.Data.Tables;
using Azure;

namespace UserService;

public static class PostUserFunction
{
    [FunctionName(nameof(PostUserFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "users")] RegisterUserAccountRequest req,
        [Table("user")] IAsyncCollector<UserEntity> profile,
        [Table("user")] TableClient existingProfiles,
        ILogger log)
    {
        log.LogInformation("{0} function processed a request", nameof(PostUserFunction));

        var entitiy = req.ToEntity();

        if (RecordExist(existingProfiles, entitiy))
            return new OkObjectResult(entitiy.PartitionKey);

        await profile.AddAsync(entitiy);

        return new OkObjectResult(entitiy.PartitionKey);
    }

    private static bool RecordExist(TableClient existingProfiles, UserEntity entitiy)
    {
        try
        {
            _ = existingProfiles.GetEntity<UserEntity>(entitiy.PartitionKey, entitiy.PartitionKey);
            return true;
        }
        catch (RequestFailedException ex)
        {
            if (ex.ErrorCode == "ResourceNotFound") return false;
            return true;
        }
    }
}

public record RegisterUserAccountRequest
{
    public string NameIdentifier { get; init; }
    public string Email { get; init; }
    public string Name { get; init; }
    public string GivenName { get; init; }
    public string Surname { get; init; }
    public string Provider { get; init; }
}
