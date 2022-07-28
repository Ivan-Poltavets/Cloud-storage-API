using CloudStorage.Core.Dtos;

namespace CloudStorage.Core.Interfaces
{
    public interface IDirectoryService
    {
        List<FileDto> GetAll(string userId);
        Task<List<FileDto>> GetAllInCurrent(string userId, Guid? id);
    }
}
