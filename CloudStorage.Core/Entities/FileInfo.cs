namespace CloudStorage.Core.Entities
{
    public class FileInfo
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string BlobName { get; set; } = string.Empty;
        public string PathToFile { get; set; } = string.Empty;
        public long Size { get; set; }

        public FileInfo()
        {
            
        }
        
        public FileInfo(string userId, string name, string pathToFile, long size)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Name = name;
            BlobName = Guid.NewGuid().ToString();
            PathToFile = pathToFile;
            Size = size;
        }
    }
}
