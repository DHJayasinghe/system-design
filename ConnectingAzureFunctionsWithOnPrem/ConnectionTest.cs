using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
***REMOVED***

namespace ConnectingAzureFunctionsWithOnPrem;

public static class ConnectionTest
***REMOVED***
    [FunctionName(nameof(ConnectionTest))]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "connection/test")] HttpRequest req,
        [Sql(
            commandText: "SELECT TOP (1000) [CustomerID] ,[CustomerName] ,[WebsiteURL] FROM [WideWorldImporters].[Sales].[Customers]",
            commandType: System.Data.CommandType.Text,
            connectionStringSetting: "SqlConnectionString")]
        IEnumerable<Customer> customers,
        ILogger log)
    ***REMOVED***
        log.LogInformation("C# HTTP trigger function processed a request.");

        return new OkObjectResult(customers);
***REMOVED***
***REMOVED***

public record Customer
***REMOVED***
    public int CustomerID ***REMOVED*** get; init; ***REMOVED***
    public string CustomerName ***REMOVED*** get; init; ***REMOVED***
    public string WebsiteURL ***REMOVED*** get; init; ***REMOVED***
***REMOVED***
