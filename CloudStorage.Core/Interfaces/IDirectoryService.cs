using CloudStorage.Core.Dtos;

namespace CloudStorage.Core.Interfaces
{
    public interface IDirectoryService
    {
        FilesDto GetAllInCurrent(Guid userId, string currentDirectory);
    }
}
