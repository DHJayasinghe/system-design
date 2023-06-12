***REMOVED***
using System.IO;
using Microsoft.Azure.WebJobs;
***REMOVED***
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Tiff;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace AssetService;

public class OptimizeFunction
***REMOVED***
    [FunctionName(nameof(OptimizeFunction))]
    public void Run(
        [BlobTrigger("images/***REMOVED***name***REMOVED***", Connection = "AzureWebJobsStorage")] Stream imageStream,
        [Blob("optimized-images/800x600/***REMOVED***name***REMOVED***", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream outputImageStream800x600,
        [Blob("optimized-images/400x300/***REMOVED***name***REMOVED***", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream outputImageStream400x300,
        [Blob("optimized-images/200x150/***REMOVED***name***REMOVED***", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream outputImageStream200x150,
        string name,
        ILogger log)
    ***REMOVED***
        log.LogInformation($"***REMOVED***nameof(OptimizeFunction)***REMOVED*** Blob trigger Processed blob\n Name:***REMOVED***name***REMOVED*** \n Size: ***REMOVED***imageStream.Length***REMOVED*** Bytes");

***REMOVED***
        ***REMOVED***
            string extension = Path.GetExtension(name);

            IImageEncoder imageEncoder = extension switch
            ***REMOVED***
                ".jpeg" => new JpegEncoder(),
                ".jpg" => new JpegEncoder(),
                ".png" => new PngEncoder(),
                ".bmp" => new BmpEncoder(),
                ".tiff" => new TiffEncoder(),
                ".webp" => new WebpEncoder(),
                ".gif" => new GifEncoder(),
                _ => throw new ArgumentOutOfRangeException($"unsupported extension ***REMOVED***extension***REMOVED***")
        ***REMOVED***;
            using Image image = Image.Load(imageStream);
            ResizeImage(image, 800, 600);
            image.Save(outputImageStream800x600, imageEncoder);
            ResizeImage(image, 400, 300);
            image.Save(outputImageStream400x300, imageEncoder);
            ResizeImage(image, 200, 150);
            image.Save(outputImageStream200x150, imageEncoder);

            log.LogInformation($"Image processing complete: ***REMOVED***name***REMOVED***");
    ***REMOVED***
***REMOVED***
        ***REMOVED***
            log.LogError($"Error processing image: ***REMOVED***name***REMOVED***. Error: ***REMOVED***ex.Message***REMOVED***");
    ***REMOVED***
***REMOVED***

    private static void ResizeImage(Image image, int width, int height)
    ***REMOVED***
        image.Mutate(x => x.Resize(new ResizeOptions
        ***REMOVED***
            Size = new Size(width, height),
            Mode = ResizeMode.Max
    ***REMOVED***));
***REMOVED***
***REMOVED***
