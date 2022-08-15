using Microsoft.AspNetCore.Http;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Core.Interfaces;

public interface IFileService
{
    Task<List<FileInfo>> AddFiles(List<IFormFile> files, string userId, Guid? currentFolderId);

    Task<List<FileInfo>> RemoveFiles(List<string> names, string userId, Guid? currentFolderId);

    Task<List<FileInfo>> GetFiles(string userId, Guid? currentFolderId);
}