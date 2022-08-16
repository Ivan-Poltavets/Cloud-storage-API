namespace CloudStorage.Core.Interfaces;

public interface IFolderHelper
{
    public Task<string> GeneratePathAsync(Guid? folderId);
}
