using CloudStorage.Core.Entities;

namespace CloudStorage.Core.Dtos
{
    public class FilesDto
    {
        public List<Core.Entities.FileInfo> FileInfos { get; set; }
        public List<Folder> Folders { get; set; }
    }
}
