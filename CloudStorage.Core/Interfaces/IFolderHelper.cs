namespace CloudStorage.Core.Interfaces;

public interface IFolderHelper
{
    public Task<string> GeneratePathAsync(string? folderId);
}
