using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IBlobStorageService _storageService;
    private readonly IAccountService _accountService;
    private readonly IRepository<FileInfo> _fileRepository;
    private readonly IRepository<FolderInfo> _folderRepository;

    public FileService(
        IRepository<FileInfo> fileRepository,
        IAccountService accountService,
        IBlobStorageService storageService,
        IRepository<FolderInfo> folderRepository)
    {
        _fileRepository = fileRepository;
        _accountService = accountService;
        _storageService = storageService;
        _folderRepository = folderRepository;
    }
    
    public async Task<List<FileInfo>> AddFiles(List<IFormFile> files, string userId, Guid? currentFolderId)
    {
        long size = 0;
        var fileInfos = new List<FileInfo>();
        string path = await FolderHelper.GeneratePath(currentFolderId, _folderRepository);

        files.ForEach(x => size += x.Length);
        await _accountService.AddFileToStorage(userId, size);

        var names = _storageService.UploadFiles(files);
        for(int i = 0; i < files.Count; i++)
        {
            fileInfos.Add(new FileInfo
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = files[i].FileName,
                BlobName = names[i],
                Size = files[i].Length,
                Path = path
            });
        }

        await _fileRepository.AddRangeAsync(fileInfos);
        await _fileRepository.SaveChangesAsync();
        return fileInfos;
    }

    public async Task<List<FileInfo>> RemoveFiles(List<string> names, string userId, Guid? currentFolderId)
    {
        long size = 0;
        string path = await FolderHelper.GeneratePath(currentFolderId, _folderRepository);
        var fileInfos = _fileRepository
            .Where(x => names.Contains(x.Name) && x.UserId == userId && x.Path == path);
        names.Clear();

        fileInfos.ForEach(x =>
        {
            names.Add(x.BlobName);
            size += x.Size;
        });

        _storageService.RemoveFiles(names);
        
        await _accountService.RemoveFileFromStorage(userId, size);
        await _fileRepository.RemoveRangeAsync(fileInfos);
        await _fileRepository.SaveChangesAsync();
        return fileInfos;
    }
    
    public async Task<List<FileInfo>> GetFiles(string userId, Guid? currentFolderId)
    {
        string path = await FolderHelper.GeneratePath(currentFolderId, _folderRepository);
        var infos = _fileRepository
            .Where(x => x.UserId == userId && x.Path == path);
        return infos;
    }
}