using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Azure.Storage.Sas;
using Azure.Storage;
using Microsoft.Extensions.Configuration;

namespace AssetService;

public class UploadFunction
{
    private readonly string storageAccountName = "";
    private readonly string storageAccountKey = "";

    public UploadFunction(IConfiguration configuration)
    {
        storageAccountName = configuration.GetValue<string>("StorageAccountName");
        storageAccountKey = configuration.GetValue<string>("StorageAccountKey");
    }
    [FunctionName(nameof(UploadFunction))]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "assets/upload-link")] HttpRequest req,
        ILogger log)
    {
        log.LogInformation("C# HTTP trigger function processed a request.");
        var containerName = "sample";
        var blobSasBuilder = new BlobSasBuilder()
        {
            BlobContainerName = containerName,
            ExpiresOn = DateTime.UtcNow.AddMinutes(10)
        };
        blobSasBuilder.SetPermissions(BlobSasPermissions.Write);

        var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(storageAccountName, storageAccountKey)).ToString();

        return new OkObjectResult(new
        {
            Container = containerName,
            SasToken = $"https://{storageAccountName}.blob.core.windows.net?{sasToken}"
        });
    }
}
