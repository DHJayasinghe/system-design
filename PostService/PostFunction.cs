using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PostService;

public class PostFunction
{
    private readonly IHttpClientFactory _httpClientFactory;
    public PostFunction(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [FunctionName(nameof(PostFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "posts")] PostRequest req,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(PostFunction));
        HttpContent content = new StringContent(JsonConvert.SerializeObject(new
        {
            Assets = req.Assets
        }));

        using var httpClient = _httpClientFactory.CreateClient();
        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var response = await httpClient.PostAsync("http://localhost:8083/assets", content);
        var assets = response.Content.ReadAsAsync<HashSet<string>>();
        return new OkResult();
    }
}

public record PostRequest
{
    public string Description { get; init; }
    public List<string> Assets { get; init; }
}
