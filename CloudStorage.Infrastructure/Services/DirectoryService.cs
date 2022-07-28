using CloudStorage.Core.Constants;
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

        public async Task<List<FileDto>> GetAllInCurrent(string userId, Guid? id)
        {
            string path = Constants.MainDirectory;
            
            if (id != null)
            {
                var folder = await _folderRepository.GetByIdAsync(id);
                path = Path.Combine(folder.Path, folder.Name);
            }

            var items = GetAll(userId);
            items = items
                .Where(x => x.Path == path)
                .ToList();

            return items;
        }

        public List<FileDto> GetAll(string userId)
        {
            var items = new List<FileDto>();

            items.AddRange(GetFileInfos(userId));
            items.AddRange(GetFolders(userId));

            return items;
        }

        private List<FileDto> GetFileInfos(string userId)
        {
            var infos = _fileInfoRepository
                .Where(x => x.UserId == userId);

            var dtos = new List<FileDto>();

            if (infos == null)
            {
                return dtos;
            }

            infos.ForEach(x => dtos.Add(new FileDto
            {
                Id = x.Id,
                Name = x.Name,
                Path = x.PathToFile,
                Type = nameof(File)
            }));

            return dtos;

        }

        private List<FileDto> GetFolders(string userId)
        {
            var folders = _folderRepository
                .Where(x => x.UserId == userId);

            var dtos = new List<FileDto>();

            if(folders == null)
            {
                return dtos;
            }

            folders.ForEach(x => dtos.Add(new FileDto
            {
                Id = x.Id,
                Name = x.Name,
                Path = x.Path,
                Type = nameof(Folder)
            }));

            return dtos;
        }
    }
}
