namespace CloudStorage.Core.Dtos
{
    public class FolderDto
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = "New Folder";
        public string Path { get; set; } = string.Empty;
    }
}
