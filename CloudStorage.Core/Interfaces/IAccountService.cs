using CloudStorage.Core.Entities;

namespace CloudStorage.Core.Interfaces;

public interface IAccountService
{
    Task<AccountExtension> AddFileToStorageAsync(string userId, long size);

    Task<AccountExtension> RemoveFileFromStorageAsync(string userId, long size);
}
