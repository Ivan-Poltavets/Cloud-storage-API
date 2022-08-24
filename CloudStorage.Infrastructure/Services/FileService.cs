using CloudStorage.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Infrastructure.Services;

public class FileService : IFileService
{
    private readonly IFolderHelper _folderHelper;
    private readonly IBlobStorageService _storageService;
    private readonly IAccountService _accountService;
    private readonly IMongoRepository<FileInfo> _fileRepository;

    public FileService(
        IMongoRepository<FileInfo> fileRepository,
        IAccountService accountService,
        IBlobStorageService storageService,
        IFolderHelper folderHelper)
    {
        _fileRepository = fileRepository;
        _accountService = accountService;
        _storageService = storageService;
        _folderHelper = folderHelper;
    }
    
    public async Task<List<FileInfo>> AddFilesAsync(List<IFormFile> files, string userId, string? currentFolderId)
    {
        long size = 0;
        var infos = new List<FileInfo>();
        string path = await _folderHelper.GeneratePathAsync(currentFolderId);

        files.ForEach(x => size += x.Length);
        await _accountService.AddFileToStorageAsync(userId, size);

        var names = _storageService.UploadFiles(files);
        for(int i = 0; i < files.Count; i++)
        {
            infos.Add(new FileInfo
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Name = files[i].FileName,
                BlobName = names[i],
                Size = files[i].Length,
                Path = path
            });
        }

        await _fileRepository.AddRangeAsync(infos);
        return infos;
    }

    public async Task<List<FileInfo>> RemoveFilesAsync(List<string> ids, string userId)
    {
        long size = 0;
        var fileInfos = await _fileRepository
            .Find(x => ids.Contains(x.Id) && x.UserId == userId);
        var blobNames = new List<string>();

        fileInfos.ForEach(x =>
        {
            blobNames.Add(x.BlobName);
            size += x.Size;
        });

        _storageService.RemoveFiles(blobNames);
        
        await _accountService.RemoveFileFromStorageAsync(userId, size);
        await _fileRepository.RemoveRangeAsync(fileInfos);
        return fileInfos;
    }
    
    public async Task<List<FileInfo>> GetFilesAsync(string userId, string? currentFolderId)
    {
        string path = await _folderHelper.GeneratePathAsync(currentFolderId);
        var files = await _fileRepository
            .Find(x => x.UserId == userId && x.Path == path);
        return files;
    }
}