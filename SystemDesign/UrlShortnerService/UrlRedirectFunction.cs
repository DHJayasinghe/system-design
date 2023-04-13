using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace UrlShortnerService
{
    public static class UrlRedirectFunction
    {
        [FunctionName(nameof(UrlRedirectFunction))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{url}")] HttpRequest req,
            string url,
            ILogger log)
        {
            log.LogInformation("{0} function processed a request for url: {1}.", nameof(UrlRedirectFunction), url);

            return new OkObjectResult(url);
        }
    }
}
