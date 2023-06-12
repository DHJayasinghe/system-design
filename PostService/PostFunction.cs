***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using System.Net.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace PostService;

public class PostFunction
***REMOVED***
    private readonly IHttpClientFactory _httpClientFactory;
    public PostFunction(IHttpClientFactory httpClientFactory)
    ***REMOVED***
        _httpClientFactory = httpClientFactory;
***REMOVED***

    [FunctionName(nameof(PostFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "posts")] PostRequest req,
        ILogger log)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** HTTP trigger processed a request.", nameof(PostFunction));
        HttpContent content = new StringContent(JsonConvert.SerializeObject(new
        ***REMOVED***
            Assets = req.Assets
    ***REMOVED***));

        using var httpClient = _httpClientFactory.CreateClient();
        content.Headers.ContentType = new MediaTypeHeaderValue("***REMOVED***lication/json");
        var response = await httpClient.PostAsync("http://localhost:8083/assets", content);
        var assets = response.Content.ReadAsAsync<HashSet<string>>();
        return new OkResult();
***REMOVED***
***REMOVED***

public record PostRequest
***REMOVED***
    public string Description ***REMOVED*** get; init; ***REMOVED***
    public List<string> Assets ***REMOVED*** get; init; ***REMOVED***
***REMOVED***
