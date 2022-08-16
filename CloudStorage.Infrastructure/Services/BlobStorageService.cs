using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CloudStorage.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace CloudStorage.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly IConfiguration _configuration;
    private readonly BlobContainerClient _containerClient;

    public BlobStorageService(BlobServiceClient blobServiceClient,
        IConfiguration configuration)
    {
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
        _containerClient = 
            _blobServiceClient.GetBlobContainerClient
            (_configuration["Microsoft:BlobStorage:ContainerName"]);
    }

    public List<string> UploadFiles(List<IFormFile> files)
    {
        var names = new List<string>();

        foreach (var file in files)
        {
            var name = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = _containerClient.GetBlobClient(name);

            using var stream = file.OpenReadStream();
            blobClient.Upload(stream, true);
            names.Add(name);
        }

        return names;
    }

    public Pageable<BlobItem> GetFiles()
        => _containerClient.GetBlobs();

    public void RemoveFiles(List<string> names)
    {
        foreach(var name in names)
        {
            var blobClient = _containerClient.GetBlobClient(name);
            blobClient.DeleteIfExists(DeleteSnapshotsOption.IncludeSnapshots);
        }
    }
}
