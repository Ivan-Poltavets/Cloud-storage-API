using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Helpers;

namespace CloudStorage.Infrastructure.Services;

public class FolderService : IFolderService
{
    private readonly IRepository<FolderInfo> _folderRepository;

    public FolderService(IRepository<FolderInfo> folderRepository)
        => _folderRepository = folderRepository;
    
    public async Task<FolderInfo> AddFolder(FolderDto folderDto, string userId, Guid? currentFolderId)
    {
        string path = await FolderHelper.GeneratePath(currentFolderId, _folderRepository);
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
    
    public async Task<FolderInfo> RemoveFolder(Guid id)
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

    public async Task<List<FolderInfo>> GetFolders(string userId, Guid? currentFolderId)
    {
        string path = await FolderHelper.GeneratePath(currentFolderId, _folderRepository);
        var folders = _folderRepository
            .Where(x => x.UserId == userId && x.Path == path);
        return folders;
    }
}