using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace CloudStorage.Core.Entities;

public class Blob : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string FileName { get; set; }
    public string Extension { get; set; }
    public byte[] Data { get; set; }
    
}
