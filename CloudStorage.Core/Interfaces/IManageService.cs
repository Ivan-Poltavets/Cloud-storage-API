using CloudStorage.Core.Dtos;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.Core.Interfaces
{
    public interface IManageService
    {
        Task AddFiles(List<IFormFile> files, string userId, string currentDirectory);

        Task RemoveFiles(List<string> names, string userId, string currentDirectory);

        Task AddFolder(FolderDto folderDto, string userId);

        Task RemoveFolder(FolderDto folderDto, string userId);
    }
}
