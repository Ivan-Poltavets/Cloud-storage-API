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

        public async Task AddFiles(List<IFormFile> files, string userId, string currentDirectory)
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

            await _accountService.AddFileToStorage(userId, size);

            await _fileInfoRepository.AddRangeAsync(fileInfos);
            await _fileInfoRepository.SaveChangesAsync();
        }

        public async Task AddFolder(FolderDto folderDto, string userId)
        {
            var folder = new Folder
            {
                Id = Guid.NewGuid(),
                Name = folderDto.Name,
                Path = folderDto.Path,
                UserId = userId
            };

            await _folderRepository.AddAsync(folder);
            await _folderRepository.SaveChangesAsync();
        }

        public async Task RemoveFiles(List<string> names, string userId, string currentDirectory)
        {
            var fileInfos = _fileInfoRepository
                .Where(x => names.Contains(x.Name) && x.UserId == userId && x.PathToFile == currentDirectory);

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

        public async Task RemoveFolder(FolderDto folderDto, string userId)
        {
            var folder = await _folderRepository
                .SingleOrDefaultAsync(x => 
                x.UserId == userId && x.Name == folderDto.Name)!;

            await _folderRepository.RemoveAsync(folder);
            await _folderRepository.SaveChangesAsync();
        }
    }
}
