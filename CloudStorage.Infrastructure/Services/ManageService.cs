using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Helpers;
using Microsoft.AspNetCore.Http;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Infrastructure.Services
{
    public class ManageService : IManageService
    {
        private readonly IStorageService _storageService;
        private readonly IAccountService _accountService;
        private readonly IRepository<Folder> _folderRepository;
        private readonly IRepository<FileInfo> _fileInfoRepository;

        public ManageService(IStorageService storageService,
            IAccountService accountService,
            IRepository<FileInfo> fileInfosRepository,
            IRepository<Folder> folderRepository)
        {
            _fileInfoRepository = fileInfosRepository;
            _storageService = storageService;
            _accountService = accountService;
            _folderRepository = folderRepository;
        }

        public async Task AddFiles(List<IFormFile> files, string userId, Guid? currentFolderId)
        {
            long size = 0;
            string currentDirectory = await FolderHelper.GeneratePath(currentFolderId, _folderRepository);
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
                size += files[i].Length;
            }

            await _accountService.AddFileToStorage(userId, size);

            await _fileInfoRepository.AddRangeAsync(fileInfos);
            await _fileInfoRepository.SaveChangesAsync();
        }

        public async Task AddFolder(FolderDto folderDto, string userId, Guid? currentFolderId)
        {
            string path = await FolderHelper.GeneratePath(currentFolderId, _folderRepository);
            var folder = new Folder
            {
                Id = Guid.NewGuid(),
                Name = folderDto.Name,
                Path = path,
                UserId = userId
            };

            await _folderRepository.AddAsync(folder);
            await _folderRepository.SaveChangesAsync();
        }

        public async Task RemoveFiles(List<string> names, string userId, Guid? currentFolderId)
        {
            string path = await FolderHelper.GeneratePath(currentFolderId, _folderRepository);
            var fileInfos = _fileInfoRepository
                .Where(x => names.Contains(x.Name) && x.UserId == userId && x.PathToFile == path);

            names.Clear();
            long size = 0;

            fileInfos.ForEach(x =>
            {
                names.Add(x.BlobName);
                size += x.Size;
            });

            _storageService.RemoveFiles(names);
            await _accountService.RemoveFileFromStorage(userId, size);

            await _fileInfoRepository.RemoveRangeAsync(fileInfos);
            await _fileInfoRepository.SaveChangesAsync();
        }

        public async Task RemoveFolder(Guid id)
        {
            var folder = await _folderRepository
                .GetByIdAsync(id);

            var foldersInside = _folderRepository.Where(x => x.Path.StartsWith(folder.Path));
            if(foldersInside != null)
            {
                await _folderRepository.RemoveRangeAsync(foldersInside);
            }

            await _folderRepository.RemoveAsync(folder);
            await _folderRepository.SaveChangesAsync();
        }

        
    }
}
