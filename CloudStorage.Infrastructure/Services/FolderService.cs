using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;

namespace CloudStorage.Infrastructure.Services;

public class FolderService : IFolderService
{
    private readonly IRepository<FolderInfo> _folderRepository;
    private readonly IFolderHelper _folderHelper;

    public FolderService(IRepository<FolderInfo> folderRepository,
        IFolderHelper folderHelper)
    {
        _folderRepository = folderRepository;
        _folderHelper = folderHelper;
    }
    
    public async Task<FolderInfo> AddFolderAsync(FolderDto folderDto, string userId, Guid? currentFolderId)
    {
        string path = await _folderHelper.GeneratePathAsync(currentFolderId);
        var folder = new FolderInfo
        {
            Id = Guid.NewGuid(),
            Name = folderDto.Name,
            Path = path,
            UserId = userId
        };

        await _folderRepository.AddAsync(folder);
        await _folderRepository.SaveChangesAsync();
        return folder;
    }
    
    public async Task<FolderInfo> RemoveFolderAsync(Guid id)
    {
        var folder = await _folderRepository
            .GetByIdAsync(id);
        var foldersInside = _folderRepository
            .Where(x => x.Path.StartsWith(folder.Path));

        if(foldersInside is not null)
        {
            await _folderRepository.RemoveRangeAsync(foldersInside);
        }

        await _folderRepository.RemoveAsync(folder);
        await _folderRepository.SaveChangesAsync();
        return folder;
    }

    public async Task<List<FolderInfo>> GetFoldersAsync(string userId, Guid? currentFolderId)
    {
        string path = await _folderHelper.GeneratePathAsync(currentFolderId);
        var folders = _folderRepository
            .Where(x => x.UserId == userId && x.Path == path);
        return folders;
    }
}