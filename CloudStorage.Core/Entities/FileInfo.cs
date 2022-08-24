using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CloudStorage.Core.Entities;

public class FileInfo : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string BlobName { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public long Size { get; set; }

    public FileInfo()
    {
        
    }
    
    public FileInfo(string userId, string name, string path, long size)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        Name = name;
        BlobName = Guid.NewGuid().ToString();
        Path = path;
        Size = size;
    }
}
