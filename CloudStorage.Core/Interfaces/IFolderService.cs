using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;

namespace CloudStorage.Core.Interfaces;

public interface IFolderService
{
    Task<FolderInfo> AddFolder(FolderDto folderDto, string userId, Guid? currentFolderId);

    Task<FolderInfo> RemoveFolder(Guid id);

    Task<List<FolderInfo>> GetFolders(string userId, Guid? currentFolderId);
}