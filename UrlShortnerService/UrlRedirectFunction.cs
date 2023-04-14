using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using UrlShortenerService.Models;

namespace UrlShortenerService;

public static class UrlRedirectFunction
***REMOVED***
    [FunctionName(nameof(UrlRedirectFunction))]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "***REMOVED***url***REMOVED***")] HttpRequest req,
        [CosmosDB(databaseName: "url-shortener-service", containerName: "shorten-url", Connection = "CosmosDBConnection", Id = "***REMOVED***url***REMOVED***", PartitionKey = "***REMOVED***url***REMOVED***")] ShortenedUrl document,
        string url,
        ILogger log)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** function processed a request for url: ***REMOVED***1***REMOVED***.", nameof(UrlRedirectFunction), url);
        return new RedirectResult(document.Value, true);
***REMOVED***
***REMOVED***