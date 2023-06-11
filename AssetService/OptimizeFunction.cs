using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace AssetService
{
    public class OptimizeFunction
    {
        [FunctionName(nameof(OptimizeFunction))]
        public void Run(
            [BlobTrigger("images/{name}", Connection = "AzureWebJobsStorage")] Stream imageStream,
            [Blob("optimized-images/800x600/{name}", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream outputImageStream800x600,
            [Blob("optimized-images/400x300/{name}", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream outputImageStream400x300,
            [Blob("optimized-images/200x150/{name}", FileAccess.Write, Connection = "AzureWebJobsStorage")] Stream outputImageStream200x150,
            string name,
            ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {imageStream.Length} Bytes");

            try
            {
                using (Image image = Image.Load(imageStream))
                {
                    ResizeImage(image, 800, 600);
                    image.Save(outputImageStream800x600, new JpegEncoder());
                    ResizeImage(image, 400, 300);
                    image.Save(outputImageStream400x300, new JpegEncoder());
                    ResizeImage(image, 200, 150);
                    image.Save(outputImageStream200x150, new JpegEncoder());
                }

                log.LogInformation($"Image processing complete: {name}");
            }
            catch (Exception ex)
            {
                log.LogError($"Error processing image: {name}. Error: {ex.Message}");
            }
        }

        private static void ResizeImage(Image image, int width, int height)
        {
            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(width, height),
                Mode = ResizeMode.Max
            }));
        }
    }
}
