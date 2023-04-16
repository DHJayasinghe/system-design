***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using UserService.Models;

namespace UserService;

public static class GetUserFunction
***REMOVED***
    [FunctionName(nameof(GetUserFunction))]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/***REMOVED***id***REMOVED***")] HttpRequest req,
        [Table("user", "***REMOVED***id***REMOVED***", "***REMOVED***id***REMOVED***")] UserEntity profile,
        ILogger log, 
        string id)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** function processed a request", nameof(GetUserFunction));

        if (profile == null) return new BadRequestObjectResult($"Profile not found with Id: ***REMOVED***id***REMOVED***");

        return new OkObjectResult(profile.ToUser());
***REMOVED***
***REMOVED***
