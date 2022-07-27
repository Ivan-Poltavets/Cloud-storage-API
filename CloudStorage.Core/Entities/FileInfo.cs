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
    }
}
