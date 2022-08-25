using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CloudStorage.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly IMongoRepository<Blob> _repository;

    public BlobStorageService(IMongoRepository<Blob> repository)
    {
        _repository = repository;
    }

    public async Task<List<Blob>> UploadFiles(List<IFormFile> files, string userId)
    {
        var blobs = new List<Blob>();

        foreach(var file in files)
        {
            using(var stream = file.OpenReadStream())
            {
                byte[] data = new byte[file.Length];
                stream.Read(data, 0, (int)file.Length);
                blobs.Add(new Blob
                {
                    UserId = userId,
                    Name = Guid.NewGuid().ToString(),
                    Extension = Path.GetExtension(file.FileName),
                    Data = data
                });
            }
        }
        await _repository.AddRangeAsync(blobs);
        return blobs;
    }

    public async Task<List<Blob>> GetFiles(string userId)
        => await _repository.FindAsync(x => x.UserId == userId);

    public async Task RemoveFiles(List<Blob> blobs)
        => await _repository.RemoveRangeAsync(blobs);
}
