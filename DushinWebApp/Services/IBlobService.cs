using Azure.Storage.Blobs.Models;
using System.Threading.Tasks;

namespace DushinWebApp.Services
{
    public interface IBlobService
    {
        Task<BlobDownloadInfo> GetBlobAsync(string name);
        Task UploadFileBlobAsync(string filePath, string fileName);
    }
}
