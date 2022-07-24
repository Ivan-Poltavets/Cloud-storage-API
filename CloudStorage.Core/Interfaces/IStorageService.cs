using Azure;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.Core.Interfaces
{
    public interface IStorageService
    {
        List<string> UploadFiles(List<IFormFile> files);
        Pageable<BlobItem> GetFiles();
        void RemoveFiles(List<string> names);
    }
}
