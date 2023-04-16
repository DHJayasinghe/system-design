using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using Newtonsoft.Json;
using UserService.Models;
***REMOVED***
***REMOVED***
using Azure;

namespace UserService;

public static class PostUserFunction
***REMOVED***
    [FunctionName(nameof(PostUserFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "user")] HttpRequest req,
        [Table("user")] IAsyncCollector<UserEntity> profile,
        [Table("user")] TableClient existingProfiles,
        ILogger log)
    ***REMOVED***
        string requestBody = new StreamReader(req.Body).ReadToEnd();
        dynamic data = JsonConvert.DeserializeObject(requestBody);

        log.LogInformation("***REMOVED***0***REMOVED*** function processed a request", nameof(PostUserFunction));

        var user = new User()
        ***REMOVED***
            FirstName = data?.firstName,
            Surname = data?.surname,
            Gender = data?.gender,
            Email = data?.email,
            PhoneNumber = data?.phoneNumber
    ***REMOVED***;
        var entitiy = user.ToEntity();

        if (RecordExist(existingProfiles, entitiy))
            return new BadRequestObjectResult($"User account already exist with username: ***REMOVED***entitiy.Username***REMOVED***");

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
