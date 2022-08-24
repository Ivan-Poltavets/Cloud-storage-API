using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;

namespace CloudStorage.Core.Interfaces;

public interface IFolderService
{
    Task<FolderInfo> AddFolderAsync(FolderDto folderDto, string userId, string? currentFolderId);

    Task<FolderInfo> RemoveFolderAsync(string id);

    Task<List<FolderInfo>> GetFoldersAsync(string userId, string? currentFolderId);
}