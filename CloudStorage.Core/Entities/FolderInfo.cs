using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CloudStorage.Core.Entities;

public class FolderInfo : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;

    public FolderInfo()
    {
            
    }

    public FolderInfo(string userId, string name, string path)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        Name = name;
        Path = path;
    }
}
