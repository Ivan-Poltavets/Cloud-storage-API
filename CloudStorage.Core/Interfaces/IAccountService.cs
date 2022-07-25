namespace CloudStorage.Core.Interfaces
{
    public interface IAccountService
    {
        void AddFileToStorage(Guid userId, long size);
        void RemoveFileFromStorage(Guid userId, long size);
    }
}
