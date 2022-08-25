using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;

namespace CloudStorage.Infrastructure.Services;

public class FolderService : IFolderService
{
    private readonly IMongoRepository<FolderInfo> _folderRepository;
    private readonly IFolderHelper _folderHelper;

    public FolderService(IMongoRepository<FolderInfo> folderRepository,
        IFolderHelper folderHelper)
    {
        _folderRepository = folderRepository;
        _folderHelper = folderHelper;
    }
    
    public async Task<FolderInfo> AddFolderAsync(FolderDto folderDto, string userId, string? currentFolderId)
    {
        string path = await _folderHelper.GeneratePathAsync(currentFolderId);
        var folder = new FolderInfo
        {
            Id = Guid.NewGuid().ToString(),
            Name = folderDto.Name,
            Path = path,
            UserId = userId
        };

        await _folderRepository.AddAsync(folder);
        return folder;
    }
    
    public async Task<FolderInfo> RemoveFolderAsync(string id)
    {
        var folder = await _folderRepository
            .GetByIdAsync(id);
        var foldersInside = await _folderRepository
            .FindAsync(x => x.Path.StartsWith(folder.Path));

        if(foldersInside is not null)
        {
            await _folderRepository.RemoveRangeAsync(foldersInside);
        }

        await _folderRepository.RemoveAsync(folder.Id);
        return folder;
    }

    public async Task<List<FolderInfo>> GetFoldersAsync(string userId, string? currentFolderId)
    {
        string path = await _folderHelper.GeneratePathAsync(currentFolderId);
        var folders = await _folderRepository
            .FindAsync(x => x.UserId == userId && x.Path == path);
        return folders;
    }
}