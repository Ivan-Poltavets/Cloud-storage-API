using CloudStorage.Core.Dtos;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.Core.Interfaces
{
    public interface IManageService
    {
        Task AddFiles(List<IFormFile> files, string userId, Guid? currentFolderId);

        Task RemoveFiles(List<string> names, string userId, Guid? currentFolderId);

        Task AddFolder(FolderDto folderDto, string userId, Guid? currentFolderId);

        Task RemoveFolder(Guid id);
    }
}
