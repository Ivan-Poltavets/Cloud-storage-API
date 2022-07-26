using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;

namespace CloudStorage.Infrastructure.Services
{
    public class DirectoryService : IDirectoryService
    {
        private readonly IRepository<Core.Entities.FileInfo> _fileInfoRepository;
        private readonly IRepository<Folder> _folderRepository;

        public DirectoryService(IRepository<Core.Entities.FileInfo> fileInfoRepository,
            IRepository<Folder> folderRepository)
        {
            _fileInfoRepository = fileInfoRepository;
            _folderRepository = folderRepository;
        }

        public List<FileDto> GetAllInCurrent(Guid userId, string currentDirectory)
        {
            var list = new List<FileDto>();
            list.AddRange(GetFileInfosInCurrent(userId, currentDirectory));
            list.AddRange(GetFoldersInCurrent(userId, currentDirectory));
            return list;
        }

        private List<FileDto> GetFileInfosInCurrent(Guid userId, string currentDirectory)
        {
            var infos = _fileInfoRepository
                .Where(x => x.UserId == userId && x.PathToFile == currentDirectory);
            var dtos = new List<FileDto>();
            
            infos.ForEach(x => dtos.Add(new FileDto
            {
                Name = x.Name,
                Type = nameof(File)
            }));

            return dtos;

        }

        private List<FileDto> GetFoldersInCurrent(Guid userId, string currentDirectory)
        {
            var folders = _folderRepository
                .Where(x => x.UserId == userId && x.Path == currentDirectory);

            var dtos = new List<FileDto>();

            folders.ForEach(x => dtos.Add(new FileDto
            {
                Name = x.Name,
                Type = nameof(Folder)
            }));

            return dtos;
        }

        
    }
}
