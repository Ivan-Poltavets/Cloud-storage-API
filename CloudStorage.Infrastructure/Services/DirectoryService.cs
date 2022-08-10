using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Helpers;

namespace CloudStorage.Infrastructure.Services
{
    public class DirectoryService : IDirectoryService
    {
        private readonly IRepository<Core.Entities.FileInfo> _fileInfoRepository;
        private readonly IRepository<FolderInfo> _folderRepository;

        public DirectoryService(IRepository<Core.Entities.FileInfo> fileInfoRepository,
            IRepository<FolderInfo> folderRepository)
        {
            _fileInfoRepository = fileInfoRepository;
            _folderRepository = folderRepository;
        }

        public async Task<List<ItemDto>> GetAllInCurrent(string userId, Guid? id)
        {
            string path = await FolderHelper.GeneratePath(id, _folderRepository);

            var items = GetAll(userId);
            items = items
                .Where(x => x.Path == path)
                .ToList();

            return items;
        }

        public List<ItemDto> GetAll(string userId)
        {
            var items = new List<ItemDto>();
            items.AddRange(GetFileInfos(userId));
            items.AddRange(GetFolders(userId));

            return items;
        }

        private List<ItemDto> GetFileInfos(string userId)
        {
            var infos = _fileInfoRepository
                .Where(x => x.UserId == userId);
            var dtos = new List<ItemDto>();
            if (infos is null)
            {
                return dtos;
            }
            infos.ForEach(x => dtos.Add(new ItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Path = x.PathToFile,
                Type = nameof(Core.Entities.FileInfo)
            }));

            return dtos;
        }

        private List<ItemDto> GetFolders(string userId)
        {
            var folders = _folderRepository
                .Where(x => x.UserId == userId);
            var dtos = new List<ItemDto>();

            if(folders is null)
            {
                return dtos;
            }
            folders.ForEach(x => dtos.Add(new ItemDto
            {
                Id = x.Id,
                Name = x.Name,
                Path = x.Path,
                Type = nameof(FolderInfo)
            }));

            return dtos;
        }
    }
}
