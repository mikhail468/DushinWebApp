using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DushinWebApp.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public string BlobStorageURL
        {
            get
            {
                return "https://dushintravelappstorage.blob.core.windows.net/travelappcontainer/";
            }
        }

        public BlobService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<BlobDownloadInfo> GetBlobAsync(string name)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("travelappcontainer");
            var blobClient = containerClient.GetBlobClient(name);
            return await blobClient.DownloadAsync();
        }

        public async Task UploadFileBlobAsync(IFormFile file, string filePath)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient("travelappcontainer");

            try
            {
                var blobClient = containerClient.GetBlobClient(filePath);
                using (var fileStream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(fileStream);
                }
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
