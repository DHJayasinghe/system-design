using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Azure.Storage;
using System.Linq;

namespace AssetService;

public class SaveFunction
{
    private readonly string storageAccountName;
    private readonly string storageAccountKey;
    private const string videoContainer = "videos";
    private const string imageContainer = "images";

    public SaveFunction(IConfiguration configuration)
    {
        storageAccountName = configuration.GetValue<string>("StorageAccountName");
        storageAccountKey = configuration.GetValue<string>("StorageAccountKey");
    }

    [FunctionName(nameof(SaveFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "assets")] SaveRequest request,
        ILogger log)
    {
        log.LogInformation("{0} HTTP trigger processed a request.", nameof(SaveFunction));

        var blobServiceClient = new BlobServiceClient(new Uri($"https://{storageAccountName}.blob.core.windows.net"), new StorageSharedKeyCredential(storageAccountName, storageAccountKey));
        var sourceContainerClient = blobServiceClient.GetBlobContainerClient(UploadFunction.tempContainerName);

        var assets = new HashSet<string>();
        foreach (var imageAssetName in request.ImageAssets)
        {
            await CopyToNewContainerAsync(blobServiceClient, sourceContainerClient, imageAssetName, imageContainer);
            assets.Add($"{imageContainer}/{imageAssetName}");
        }

        foreach (var videoAssetName in request.VideoAssets)
        {
            await CopyToNewContainerAsync(blobServiceClient, sourceContainerClient, videoAssetName, videoContainer);
            assets.Add($"{videoContainer}/{videoAssetName}");
        }

        return new OkObjectResult(assets);
    }

    private static async Task CopyToNewContainerAsync(BlobServiceClient blobServiceClient, BlobContainerClient sourceContainerClient, string imageAssetName, string containerName)
    {
        var destinationContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient sourceBlobClient = sourceContainerClient.GetBlobClient(imageAssetName);
        BlobClient destinationBlobClient = destinationContainerClient.GetBlobClient(imageAssetName);
        await destinationBlobClient.StartCopyFromUriAsync(sourceBlobClient.Uri);
    }
}

public record SaveRequest
{
    private static readonly string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
    private static readonly string[] videoExtensions = { ".mp4", ".avi", ".mov", ".wmv", ".mkv" };

    public List<string> Assets { get; init; }
    public List<string> ImageAssets => Assets.Select(asset => Path.GetFileName(asset)).Where(asset => imageExtensions.Contains(Path.GetExtension(asset))).ToList();
    public List<string> VideoAssets => Assets.Select(asset => Path.GetFileName(asset)).Where(asset => videoExtensions.Contains(Path.GetExtension(asset))).ToList();
}
