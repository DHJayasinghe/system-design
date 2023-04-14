using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using UrlShortenerService.Models;

namespace UrlShortenerService;

public static class UrlRedirectFunction
{
    [FunctionName(nameof(UrlRedirectFunction))]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "{url}")] HttpRequest req,
        [CosmosDB(databaseName: "url-shortener-service", containerName: "shorten-url", Connection = "CosmosDBConnection", Id = "{url}", PartitionKey = "{url}")] ShortenedUrl document,
        string url,
        ILogger log)
    {
        log.LogInformation("{0} function processed a request for url: {1}.", nameof(UrlRedirectFunction), url);
        return new RedirectResult(document.Value, true);
    }
}