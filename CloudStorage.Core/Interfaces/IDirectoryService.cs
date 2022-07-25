using CloudStorage.Core.Dtos;

namespace CloudStorage.Core.Interfaces
{
    public interface IDirectoryService
    {
        List<FileDto> GetAllInCurrent(Guid userId, string currentDirectory);
    }
}
