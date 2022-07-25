using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Infrastructure.Services
{
    public class ManageService : IManageService
    {
        private readonly IStorageService _storageService;
        private readonly IAccountService _accountService;
        private readonly AuthDbContext _dbContext;
        public ManageService(IStorageService storageService,
            IAccountService accountService,
            AuthDbContext dbContext)
        {
            _storageService = storageService;
            _accountService = accountService;
            _dbContext = dbContext;
        }

        public async void AddFiles(List<IFormFile> files, Guid userId, string currentDirectory)
        {
            var fileInfos = new List<FileInfo>();
            var names = _storageService.UploadFiles(files);
            long size = 0;

            for(int i = 0; i < files.Count; i++)
            {
                fileInfos.Add(new FileInfo
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Name = files[i].FileName,
                    BlobName = names[i],
                    Size = files[i].Length,
                    PathToFile = currentDirectory
                });
                size += files[i].Length;
            }

            _accountService.AddFileToStorage(userId, size);

            await _dbContext.FileInfos.AddRangeAsync(fileInfos);
            await _dbContext.SaveChangesAsync();
        }

        public void AddFolder(FolderDto folderDto)
        {
            //map from FolderDto to Folder class

            //temp
            var folder = new Folder
            {
                Id = Guid.NewGuid(),
                Name = folderDto.Name,
                Path = folderDto.Path,
                UserId = folderDto.UserId
            };

            _dbContext.Folders.Add(folder);
            _dbContext.SaveChanges();
        }

        public async void RemoveFiles(List<string> names, Guid userId)
        {
            var fileInfos = _dbContext.FileInfos
                .Where(x => names.Contains(x.Name) && x.UserId == userId)
                .ToList();

            names.Clear();
            long size = 0;

            fileInfos.ForEach(x =>
            {
                names.Add(x.BlobName);
                size += x.Size;
            });

            _storageService.RemoveFiles(names);
            _accountService.RemoveFileFromStorage(userId, size);

            _dbContext.FileInfos.RemoveRange(fileInfos);
            await _dbContext.SaveChangesAsync();
        }

        public void RemoveFolder(FolderDto folderDto)
        {
            var folder = _dbContext.Folders
                .SingleOrDefault(x => 
                x.UserId == folderDto.UserId&& x.Name == folderDto.Name)!;

            _dbContext.Folders.Remove(folder);
            _dbContext.SaveChanges();
        }
    }
}
