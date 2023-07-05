using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UserService.Models;
using System.Collections.Generic;
using Azure.Data.Tables;
using Azure;

namespace UserService;

public static class GetUsersFunction
{
    [FunctionName(nameof(GetUsersFunction)+"ById")]
    public static IActionResult GetUsersById(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users/{id}")] HttpRequest req,
        [Table("user", "{id}", "{id}")] UserEntity profile,
        ILogger log,
        string id)
    {
        log.LogInformation("{0} function processed a request", nameof(GetUsersFunction));

        if (profile == null) return new BadRequestObjectResult($"Profile not found with Id: {id}");

        return new OkObjectResult(profile.ToUser());
    }

    [FunctionName(nameof(GetUsersFunction))]
    public static IActionResult GetUsers(
       [HttpTrigger(AuthorizationLevel.Function, "get", Route = "users")] HttpRequest req,
       [Table("user")] TableClient tableClient,
       ILogger log)
    {
        log.LogInformation("{0} function processed a request", nameof(GetUsersFunction));

        Pageable<UserEntity> queryResults = tableClient.Query<UserEntity>();
        List<User> profiles = new();
        foreach (var entity in queryResults)
        {
            profiles.Add(entity.ToUser());
        }

        return new OkObjectResult(profiles);
    }
}
