using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Data.Tables;
using NotificationService.Models;
using Azure;
using SharedKernal;
using System.Collections.Generic;

namespace NotificationService.API;

public class GetNotificationsFunction
{
    private readonly ICurrentUser _currentUser;

    public GetNotificationsFunction(ICurrentUser currentUser) => _currentUser = currentUser;

    [FunctionName(nameof(GetNotificationsFunction))]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "notifications")] HttpRequest req,
        [Table("notification")] TableClient tableClient,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(GetNotificationsFunction));

        var key = NotificationActivity.GetKey(_currentUser.Id);
        Pageable<NotificationActivity> queryResults = tableClient.Query<NotificationActivity>(filter: $"PartitionKey eq '{key}'");

        List<NotificationActivityResponse> activities = new();
        foreach (var entity in queryResults) activities.Add(new NotificationActivityResponse(entity));

        return new OkObjectResult(activities);
    }
}
