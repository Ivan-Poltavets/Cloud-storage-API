using CloudStorage.Core.Entities;
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
    private readonly IMongoRepository<Blob> _blobRepository;

    public FileService(
        IMongoRepository<FileInfo> fileRepository,
        IAccountService accountService,
        IBlobStorageService storageService,
        IFolderHelper folderHelper,
        IMongoRepository<Blob> blobRepository)
    {
        _fileRepository = fileRepository;
        _accountService = accountService;
        _storageService = storageService;
        _folderHelper = folderHelper;
        _blobRepository = blobRepository;
    }
    
    public async Task<List<FileInfo>> AddFilesAsync(List<IFormFile> files, string userId, string? currentFolderId)
    {
        long size = 0;
        var infos = new List<FileInfo>();
        string path = await _folderHelper.GeneratePathAsync(currentFolderId);

        files.ForEach(x => size += x.Length);
        await _accountService.AddFileToStorageAsync(userId, size);

        var blobs = await _storageService.UploadFiles(files, userId);
        for(int i = 0; i < files.Count; i++)
        {
            infos.Add(new FileInfo
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Name = files[i].FileName,
                BlobName = blobs[i].Name,
                Size = files[i].Length,
                Path = path
            });
        }

        await _fileRepository.AddRangeAsync(infos);
        return infos;
    }

    public async Task<List<FileInfo>> RemoveFilesAsync(List<string> ids, string userId)
    {
        var blobs = await _blobRepository
            .FindAsync(x => ids.Contains(x.Id) && x.UserId == userId);
        var fileInfos = await _fileRepository
            .FindAsync(x => blobs.Find(b => b.Name == x.BlobName) != null);

        long size = 0;
        fileInfos.ForEach(x => size += x.Size);

        await _storageService.RemoveFiles(blobs);
        await _accountService.RemoveFileFromStorageAsync(userId, size);
        await _fileRepository.RemoveRangeAsync(fileInfos);
        return fileInfos;
    }
    
    public async Task<List<FileInfo>> GetFilesAsync(string userId, string? currentFolderId)
    {
        string path = await _folderHelper.GeneratePathAsync(currentFolderId);
        var files = await _fileRepository
            .FindAsync(x => x.UserId == userId && x.Path == path);
        return files;
    }
}