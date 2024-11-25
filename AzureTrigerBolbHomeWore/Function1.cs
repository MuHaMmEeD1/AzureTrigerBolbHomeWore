using System.IO;
using System.Threading.Tasks;
using AzureTrigerBolbHomeWore.Models;
using AzureTrigerBolbHomeWore.Services.Abstract;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SixLabors.ImageSharp.Processing;

namespace AzureTriggerBlobHomeWork
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        private readonly IBlobService _blobService;

        public Function1(ILogger<Function1> logger, IBlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
        }

        [Function(nameof(Function1))]
        public async Task Run([BlobTrigger("blob/{name}", Connection = "AzureWebJobsStorage")] System.IO.Stream stream, string name, FunctionContext context)
        {
            if (name.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase))
            {
                var convertedStream = await ConvertPngToJpgAsync(stream);

                string newFileName = Path.ChangeExtension(name, ".jpg");

                await _blobService.RemoveAsync(name);

                var uploadedUrl = await _blobService.UploadAsync(convertedStream, newFileName);

                _logger.LogInformation($"ok: {uploadedUrl}");
            }
            else
            {
                _logger.LogInformation($"no ok: {name}");
            }
        }

        private async Task<Stream> ConvertPngToJpgAsync(Stream inputStream)
        {
            var outputStream = new MemoryStream();
            using (var image = await SixLabors.ImageSharp.Image.LoadAsync(inputStream))
            {
                image.Mutate(x => x.BackgroundColor(SixLabors.ImageSharp.Color.White));

                var jpegEncoder = new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder { Quality = 75 };
                await image.SaveAsync(outputStream, jpegEncoder);
            }

            outputStream.Position = 0;
            return outputStream;
        }
    }
}
