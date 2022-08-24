using Microsoft.AspNetCore.Http;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Core.Interfaces;

public interface IFileService
{
    Task<List<FileInfo>> AddFilesAsync(List<IFormFile> files, string userId, string? currentFolderId);

    Task<List<FileInfo>> RemoveFilesAsync(List<string> ids, string userId);

    Task<List<FileInfo>> GetFilesAsync(string userId, string? currentFolderId);
}