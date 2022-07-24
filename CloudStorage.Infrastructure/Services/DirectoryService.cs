using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Data;

namespace CloudStorage.Infrastructure.Services
{
    public class DirectoryService : IDirectoryService
    {
        private readonly AuthDbContext _dbContext;
        public DirectoryService(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public FilesDto GetAllInCurrent(Guid userId, string currentDirectory)
        {
            return new FilesDto
            {
                FileInfos = GetFileInfosInCurrent(userId, currentDirectory),
                Folders = GetFoldersInCurrent(userId, currentDirectory)
            };
        }


        private List<Core.Entities.FileInfo> GetFileInfosInCurrent(Guid userId, string currentDirectory)
        {
            var infos = _dbContext.FileInfos
                .Where(x => x.UserId == userId && x.PathToFile == currentDirectory)
                .ToList();

            return infos;

        }

        private List<Folder> GetFoldersInCurrent(Guid userId, string currentDirectory)
        {
            var folders = _dbContext.Folders
                .Where(x => x.UserId == userId && x.Path == currentDirectory)
                .ToList();
            return folders;
        }

        
    }
}
