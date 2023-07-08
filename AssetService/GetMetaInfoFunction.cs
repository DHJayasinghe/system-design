using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AssetService;

public class GetMetaInfoFunction
{
    private readonly IConfiguration _configuration;
    public GetMetaInfoFunction(IConfiguration configuration) => _configuration = configuration;

    [FunctionName(nameof(GetMetaInfoFunction))]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assets/meta-info")] HttpRequest req) => new OkObjectResult(new
    {
        AssetsBaseUrl = $"https://{ _configuration["StorageAccountName"]}.blob.core.windows.net",
        OptimizedImageContainer = OptimizeFunction.OptimizedImageContainer,
        Resolutions = OptimizeFunction.Resolutions
    });
}
