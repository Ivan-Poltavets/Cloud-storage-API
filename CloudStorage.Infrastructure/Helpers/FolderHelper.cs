using CloudStorage.Core.Constants;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;

namespace CloudStorage.Infrastructure.Helpers;

public class FolderHelper : IFolderHelper
{
    private readonly IRepository<FolderInfo> _repository;

    public FolderHelper(IRepository<FolderInfo> repository)
        => _repository = repository;

    public async Task<string> GeneratePathAsync(Guid? folderId)
    {
        string path = Constants.MainDirectory;

        if (folderId is not null)
        {
            var folder = await _repository.GetByIdAsync(folderId);
            path = Path.Combine(folder.Path, folder.Name.ToLower());
        }

        return path;
    }
}
