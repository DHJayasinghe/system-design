***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using Azure.Storage.Sas;
using Azure.Storage;
***REMOVED***

namespace AssetService;

public class UploadFunction
***REMOVED***
    private readonly string storageAccountName = "";
    private readonly string storageAccountKey = "";
    public const string tempContainerName = "temp";

    public UploadFunction(IConfiguration configuration)
    ***REMOVED***
        storageAccountName = configuration.GetValue<string>("StorageAccountName");
        storageAccountKey = configuration.GetValue<string>("StorageAccountKey");
***REMOVED***

    [FunctionName(nameof(UploadFunction))]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "assets/upload-link")] HttpRequest req,
        ILogger log)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** HTTP trigger processed a request.", nameof(UploadFunction));

        var blobSasBuilder = new BlobSasBuilder()
        ***REMOVED***
            BlobContainerName = tempContainerName,
            ExpiresOn = DateTime.UtcNow.AddMinutes(10)
    ***REMOVED***;
        blobSasBuilder.SetPermissions(BlobSasPermissions.Write);

        var sasToken = blobSasBuilder.ToSasQueryParameters(new StorageSharedKeyCredential(storageAccountName, storageAccountKey)).ToString();

        return new OkObjectResult(new
        ***REMOVED***
            Container = tempContainerName,
            SasToken = $"https://***REMOVED***storageAccountName***REMOVED***.blob.core.windows.net?***REMOVED***sasToken***REMOVED***"
    ***REMOVED***);
***REMOVED***
***REMOVED***
