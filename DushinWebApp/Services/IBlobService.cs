using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DushinWebApp.Services
{
    public interface IBlobService
    {
        Task<BlobDownloadInfo> GetBlobAsync(string name);
        Task UploadFileBlobAsync(IFormFile file, string filePath);
    }
}
