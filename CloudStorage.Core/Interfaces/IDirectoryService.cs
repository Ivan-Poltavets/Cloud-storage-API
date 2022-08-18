using CloudStorage.Core.Dtos;

namespace CloudStorage.Core.Interfaces
{
    public interface IDirectoryService
    {
        Task<List<ItemDto>> GetAllInCurrentAsync(string userId, Guid? id);
    }
}
