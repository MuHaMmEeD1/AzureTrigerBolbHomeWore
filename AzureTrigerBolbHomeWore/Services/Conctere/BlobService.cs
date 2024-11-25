using Azure.Storage.Blobs;
using AzureTrigerBolbHomeWore.Services.Abstract;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTrigerBolbHomeWore.Services.Conctere
{
    public class BlobService : IBlobService
    {

        private readonly BlobContainerClient _blobContainerClient;

        public BlobService(IConfiguration configuration)
        {
            var connectionString = configuration["Values:AzureWebJobsStorage"];
            var containerName = configuration["Values:ContainerName"];
            _blobContainerClient = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<string> UploadAsync(Stream fileStrimn, string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStrimn, true);
            return blobClient.Uri.ToString();
        }

        public async Task<bool> RemoveAsync(string fileName)
        {
            var blobClient = _blobContainerClient.GetBlobClient(fileName);

            if (await blobClient.ExistsAsync())
            {
                await blobClient.DeleteAsync();
                return true;
            }
            return false;
        }
    }
}
