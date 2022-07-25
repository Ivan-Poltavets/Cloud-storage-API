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

        public List<FileDto> GetAllInCurrent(Guid userId, string currentDirectory)
        {
            var list = new List<FileDto>();
            list.AddRange(GetFileInfosInCurrent(userId, currentDirectory));
            list.AddRange(GetFoldersInCurrent(userId, currentDirectory));
            return list;
        }

        private List<FileDto> GetFileInfosInCurrent(Guid userId, string currentDirectory)
        {
            var infos = _dbContext.FileInfos
                .Where(x => x.UserId == userId && x.PathToFile == currentDirectory)
                .ToList();
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
            var folders = _dbContext.Folders
                .Where(x => x.UserId == userId && x.Path == currentDirectory)
                .ToList();

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
