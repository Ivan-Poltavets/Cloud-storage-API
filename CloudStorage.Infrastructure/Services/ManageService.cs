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
        private readonly AuthDbContext _dbContext;
        public ManageService(IStorageService storageService,
            AuthDbContext dbContext)
        {
            _storageService = storageService;
            _dbContext = dbContext;
        }

        public async void AddFiles(List<IFormFile> files, Guid userId, string currentDirectory)
        {
            var fileInfos = new List<FileInfo>();
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
                    PathToFile = currentDirectory
                });
            }

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

        public async void RemoveFiles(List<Guid> ids)
        {
            var fileInfos = _dbContext.FileInfos
                .Where(x => ids.IndexOf(x.Id) > 0)
                .ToList();

            var names = new List<string>();
            fileInfos.ForEach(x => names.Add(x.BlobName));
            _storageService.RemoveFiles(names);

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
