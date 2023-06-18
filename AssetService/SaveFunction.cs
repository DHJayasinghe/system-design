***REMOVED***
using System.IO;
***REMOVED***
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
***REMOVED***
***REMOVED***
using Azure.Storage.Blobs;
***REMOVED***
using Azure.Storage;
***REMOVED***

namespace AssetService;

public class SaveFunction
***REMOVED***
    private readonly string storageAccountName;
    private readonly string storageAccountKey;
    private const string videoContainer = "videos";
    private const string imageContainer = "images";

    public SaveFunction(IConfiguration configuration)
    ***REMOVED***
        storageAccountName = configuration.GetValue<string>("StorageAccountName");
        storageAccountKey = configuration.GetValue<string>("StorageAccountKey");
***REMOVED***

    [FunctionName(nameof(SaveFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "assets")] SaveRequest request,
        ILogger log)
    ***REMOVED***
        log.LogInformation("***REMOVED***0***REMOVED*** HTTP trigger processed a request.", nameof(SaveFunction));

        var blobServiceClient = new BlobServiceClient(new Uri($"https://***REMOVED***storageAccountName***REMOVED***.blob.core.windows.net"), new StorageSharedKeyCredential(storageAccountName, storageAccountKey));
        var sourceContainerClient = blobServiceClient.GetBlobContainerClient(UploadFunction.tempContainerName);

        var assets = new HashSet<string>();
        foreach (var imageAssetName in request.ImageAssets)
        ***REMOVED***
            await CopyToNewContainerAsync(blobServiceClient, sourceContainerClient, imageAssetName, imageContainer);
            assets.Add($"***REMOVED***imageContainer***REMOVED***/***REMOVED***imageAssetName***REMOVED***");
    ***REMOVED***

        foreach (var videoAssetName in request.VideoAssets)
        ***REMOVED***
            await CopyToNewContainerAsync(blobServiceClient, sourceContainerClient, videoAssetName, videoContainer);
            assets.Add($"***REMOVED***videoContainer***REMOVED***/***REMOVED***videoAssetName***REMOVED***");
    ***REMOVED***

        return new OkObjectResult(assets);
***REMOVED***

    private static async Task CopyToNewContainerAsync(BlobServiceClient blobServiceClient, BlobContainerClient sourceContainerClient, string imageAssetName, string containerName)
    ***REMOVED***
        var destinationContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient sourceBlobClient = sourceContainerClient.GetBlobClient(imageAssetName);
        BlobClient destinationBlobClient = destinationContainerClient.GetBlobClient(imageAssetName);
        await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);
***REMOVED***
***REMOVED***

public record SaveRequest
***REMOVED***
    private static readonly string[] imageExtensions = ***REMOVED*** ".jpg", ".jpeg", ".png", ".gif", ".bmp" ***REMOVED***;
    private static readonly string[] videoExtensions = ***REMOVED*** ".mp4", ".avi", ".mov", ".wmv", ".mkv" ***REMOVED***;

    public List<string> Assets ***REMOVED*** get; init; ***REMOVED***
    public List<string> ImageAssets => Assets.Select(asset => Path.GetFileName(asset)).Where(asset => imageExtensions.Contains(Path.GetExtension(asset))).ToList();
    public List<string> VideoAssets => Assets.Select(asset => Path.GetFileName(asset)).Where(asset => videoExtensions.Contains(Path.GetExtension(asset))).ToList();
***REMOVED***
