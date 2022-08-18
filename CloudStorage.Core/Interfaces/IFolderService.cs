using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;

namespace CloudStorage.Core.Interfaces;

public interface IFolderService
{
    Task<FolderInfo> AddFolderAsync(FolderDto folderDto, string userId, Guid? currentFolderId);

    Task<FolderInfo> RemoveFolderAsync(Guid id);

    Task<List<FolderInfo>> GetFoldersAsync(string userId, Guid? currentFolderId);
}