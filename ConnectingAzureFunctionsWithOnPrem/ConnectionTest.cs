using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace ConnectingAzureFunctionsWithOnPrem;

public static class ConnectionTest
{
    [FunctionName(nameof(ConnectionTest))]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "connection/test")] HttpRequest req,
        [Sql(
            commandText: "SELECT TOP (1000) [CustomerID] ,[CustomerName] ,[WebsiteURL] FROM [WideWorldImporters].[Sales].[Customers]",
            commandType: System.Data.CommandType.Text,
            connectionStringSetting: "SqlConnectionString")]
        IEnumerable<Customer> customers,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        return new OkObjectResult(customers);
    }
}

public record Customer
{
    public int CustomerID { get; init; }
    public string CustomerName { get; init; }
    public string WebsiteURL { get; init; }
}
