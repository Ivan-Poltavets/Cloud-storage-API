using CloudStorage.Core.Dtos;

namespace CloudStorage.Core.Interfaces
{
    public interface IDirectoryService
    {
        Task<List<ItemDto>> GetAllInCurrent(string userId, Guid? id);
    }
}
