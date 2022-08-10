using CloudStorage.Core.Entities;

namespace CloudStorage.Core.Interfaces
{
    public interface IAccountService
    {
        Task<AccountStorage> AddFileToStorage(string userId, long size);

        Task<AccountStorage> RemoveFileFromStorage(string userId, long size);
    }
}
