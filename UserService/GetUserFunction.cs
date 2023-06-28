using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UserService.Models;

namespace UserService;

public static class GetUserFunction
{
    [FunctionName(nameof(GetUserFunction))]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{id}")] HttpRequest req,
        [Table("user", "{id}", "{id}")] UserEntity profile,
        ILogger log, 
        string id)
    {
        log.LogInformation("{0} function processed a request", nameof(GetUserFunction));

        if (profile == null) return new BadRequestObjectResult($"Profile not found with Id: {id}");

        return new OkObjectResult(profile.ToUser());
    }
}
