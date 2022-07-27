namespace CloudStorage.Core.Entities
{
    public class Folder
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
    }
}
