using Microsoft.AspNetCore.Http;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Core.Interfaces;

public interface IFileService
{
    Task<List<FileInfo>> AddFilesAsync(List<IFormFile> files, string userId, Guid? currentFolderId);

    Task<List<FileInfo>> RemoveFilesAsync(List<Guid> ids, string userId);

    Task<List<FileInfo>> GetFilesAsync(string userId, Guid? currentFolderId);
}