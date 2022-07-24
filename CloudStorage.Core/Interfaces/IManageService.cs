using CloudStorage.Core.Dtos;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.Core.Interfaces
{
    public interface IManageService
    {
        void AddFiles(List<IFormFile> files, Guid userId, string currentDirectory);
        void RemoveFiles(List<Guid> ids);
        void AddFolder(FolderDto folderDto);
        void RemoveFolder(FolderDto folderDto);
    }
}
