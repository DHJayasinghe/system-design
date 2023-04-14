using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using UrlShortenerService.Models;

namespace UrlShortenerService;

public static class UrlShortenFunction
{
    private const int ShortUrlLength = 7;

    [FunctionName(nameof(UrlShortenFunction))]
    public static IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "shorten")] HttpRequest req,
        [CosmosDB(databaseName: "url-shortener-service", containerName: "shorten-url", Connection = "CosmosDBConnection", PartitionKey = "/id",CreateIfNotExists = true)] out dynamic document,
        ILogger log)
    {
        string requestBody = new StreamReader(req.Body).ReadToEnd();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string url = data?.url;

        log.LogInformation("{0} function processed a request for url: {1}.", nameof(UrlShortenFunction), url);

        document = null;

        if (string.IsNullOrEmpty(url))
            return new BadRequestObjectResult("Pass a URL in the request body to return a shorten URL.");

        string shortenedUrl = GenerateShortUrl(url);

        document = new ShortenedUrl { Id = shortenedUrl, Value = url };

        return new OkObjectResult($"{GetAppHostUrl(req)}/{shortenedUrl}");
    }

    private static string GetAppHostUrl(HttpRequest req) => $"{req.Scheme}://{req.Host.Value}";

    private static string GenerateShortUrl(string url)
    {
        using var sha256 = SHA256.Create();
        var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(url));
        var base64String = Convert.ToBase64String(hashBytes);

        StringBuilder alphanumericString = RemoveNonAlphaNumericCharacters(base64String);
        var shortUrl = alphanumericString.ToString()[..ShortUrlLength];

        return shortUrl;
    }

    private static StringBuilder RemoveNonAlphaNumericCharacters(string base64String)
    {
        var alphanumericString = new StringBuilder();
        foreach (var c in base64String)
        {
            if (!char.IsLetterOrDigit(c)) continue;
            alphanumericString.Append(c);
        }

        return alphanumericString;
    }
}
