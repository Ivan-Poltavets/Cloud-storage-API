using CloudStorage.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.Core.Interfaces;

public interface IBlobStorageService
{
    Task<List<Blob>> UploadFiles(List<IFormFile> files, string userId);

    Task<List<Blob>> GetFiles(string userId);

    Task RemoveFiles(List<Blob> blobs);
}
