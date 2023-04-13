using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace UrlShortnerService;

public static class ShortenUrlFunction
{
    private const int ShortUrlLength = 7;

    [FunctionName(nameof(ShortenUrlFunction))]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "shorten")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("{0} function processed a request.", nameof(ShortenUrlFunction));

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);

        string url = data?.url;
        if (string.IsNullOrEmpty(url))
            return new BadRequestObjectResult("Pass a URL in the request body to return a shorten URL.");

        string shortenedUrl = GenerateShortUrl(url);

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
