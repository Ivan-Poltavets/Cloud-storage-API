using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
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
            IRepository<FileInfo> repository,
            IRepository<Folder> folderRepository)
        {
            _fileInfoRepository = repository;
            _storageService = storageService;
            _accountService = accountService;
            _folderRepository = folderRepository;
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
            await _fileInfoRepository.AddRangeAsync(fileInfos);
        }

        public async void AddFolder(FolderDto folderDto, Guid userId)
        {
            var folder = new Folder
            {
                Id = Guid.NewGuid(),
                Name = folderDto.Name,
                Path = folderDto.Path,
                UserId = userId
            };

            await _folderRepository.AddAsync(folder);
        }

        public void RemoveFiles(List<string> names, Guid userId)
        {
            var fileInfos = _fileInfoRepository
                .Where(x => names.Contains(x.Name) && x.UserId == userId);

            names.Clear();
            long size = 0;

            fileInfos.ForEach(x =>
            {
                names.Add(x.BlobName);
                size += x.Size;
            });

            _storageService.RemoveFiles(names);
            _accountService.RemoveFileFromStorage(userId, size);

            _fileInfoRepository.RemoveRange(fileInfos);
        }

        public void RemoveFolder(FolderDto folderDto, Guid userId)
        {
            var folder = _folderRepository
                .SingleOrDefault(x => 
                x.UserId == userId && x.Name == folderDto.Name)!;

            _folderRepository.Remove(folder);
        }
    }
}
