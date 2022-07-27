namespace CloudStorage.Core.Interfaces
{
    public interface IAccountService
    {
        Task AddFileToStorage(string userId, long size);

        Task RemoveFileFromStorage(string userId, long size);
    }
}
