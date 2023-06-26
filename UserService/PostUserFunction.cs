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
***REMOVED***
    [FunctionName(nameof(PostUserFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "users")] RegisterUserAccountRequest req,
        [Table("user")] IAsyncCollector<UserEntity> profile,
        [Table("user")] TableClient existingProfiles,
        ILogger log)
***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** function processed a request", nameof(PostUserFunction));

        var entitiy = req.ToEntity();

        if (RecordExist(existingProfiles, entitiy))
            return new OkObjectResult(entitiy.PartitionKey);

        await profile.AddAsync(entitiy);

        return new OkObjectResult(entitiy.PartitionKey);
    ***REMOVED***

    private static bool RecordExist(TableClient existingProfiles, UserEntity entitiy)
***REMOVED***
        ***REMOVED***
    ***REMOVED***
            _ = existingProfiles.GetEntity<UserEntity>(entitiy.PartitionKey, entitiy.PartitionKey);
            return true;
        ***REMOVED***
        catch (RequestFailedException ex)
    ***REMOVED***
            if (ex.ErrorCode == "ResourceNotFound") return false;
            return true;
        ***REMOVED***
    ***REMOVED***
***REMOVED***

public record RegisterUserAccountRequest
***REMOVED***
    public string NameIdentifier ***REMOVED*** get; init; ***REMOVED***
    public string Email ***REMOVED*** get; init; ***REMOVED***
    public string Name ***REMOVED*** get; init; ***REMOVED***
    public string GivenName ***REMOVED*** get; init; ***REMOVED***
    public string Surname ***REMOVED*** get; init; ***REMOVED***
    public string Provider ***REMOVED*** get; init; ***REMOVED***
***REMOVED***
