***REMOVED***
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using UrlShortenerService.Models;

namespace UrlShortenerService;

public static class UrlShortenFunction
***REMOVED***
    private const int ShortUrlLength = 7;

    [FunctionName(nameof(UrlShortenFunction))]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "shorten")] HttpRequest req,
        [CosmosDB(databaseName: "url-shortener-service", containerName: "shorten-url", Connection = "CosmosDBConnection", PartitionKey = "/id",CreateIfNotExists = true)] out dynamic document,
        ILogger log)
***REMOVED***
        string requestBody = new StreamReader(req.Body).ReadToEnd();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string url = data?.url;

        log.LogInformation("***REMOVED***0***REMOVED*** function processed a request for url: ***REMOVED***1***REMOVED***.", nameof(UrlShortenFunction), url);

        document = null;

        if (string.IsNullOrEmpty(url))
            return new BadRequestObjectResult("Pass a URL in the request body to return a shorten URL.");

        string shortenedUrl = GenerateShortUrl(url);

        document = new ShortenedUrl ***REMOVED*** Id = shortenedUrl, Value = url ***REMOVED***;

        return new OkObjectResult($"***REMOVED***GetAppHostUrl(req)***REMOVED***/***REMOVED***shortenedUrl***REMOVED***");
    ***REMOVED***

    private static string GetAppHostUrl(HttpRequest req) => $"***REMOVED***req.Scheme***REMOVED***://***REMOVED***req.Host.Value***REMOVED***";

    private static string GenerateShortUrl(string url)
***REMOVED***
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(url));
        var base64String = Convert.ToBase64String(hashBytes);

        StringBuilder alphanumericString = RemoveNonAlphaNumericCharacters(base64String);
        var shortUrl = alphanumericString.ToString()[..ShortUrlLength];

        return shortUrl;
    ***REMOVED***

    private static StringBuilder RemoveNonAlphaNumericCharacters(string base64String)
***REMOVED***
        var alphanumericString = new StringBuilder();
        foreach (var c in base64String)
    ***REMOVED***
            if (!char.IsLetterOrDigit(c)) continue;
            alphanumericString.Append(c);
        ***REMOVED***

        return alphanumericString;
    ***REMOVED***
***REMOVED***
