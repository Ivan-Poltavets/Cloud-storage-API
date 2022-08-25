using AutoFixture;
using AutoFixture.AutoMoq;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Services;
using CloudStorage.Tests.Base;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Tests.Services;

public class FileServiceTests
{
    public Fixture Fixture { get; set; }
    
    private readonly FileService _sut;
    private readonly Mock<IBlobStorageService> _storageService;
    private readonly Mock<IAccountService> _accountService;
    private readonly Mock<IMongoRepository<FileInfo>> _fileRepository;
    private readonly Mock<IFolderHelper> _folderHelper;
    private readonly Mock<IMongoRepository<Blob>> _blobRepository;

    public FileServiceTests()
    {
        Fixture = new Fixture();
        Fixture.Customize(new AutoMoqCustomization());

        _storageService = new Mock<IBlobStorageService>();
        _accountService = new Mock<IAccountService>();
        _fileRepository = new Mock<IMongoRepository<FileInfo>>();
        _folderHelper = new Mock<IFolderHelper>();
        _blobRepository = new Mock<IMongoRepository<Blob>>();
        _sut = new FileService(
            _fileRepository.Object,
            _accountService.Object,
            _storageService.Object,
            _folderHelper.Object,
            _blobRepository.Object);
    }

    [Theory]
    [AutoDomainData]
    public async Task AddFiles(List<Blob> blobs, string userId)
    {
        var folder = new DataSeed().FolderInfosTest.SingleOrDefault(x => x.Name == "second");
        var files = Fixture.Create<List<IFormFile>>();
        
        _folderHelper.Setup(x => x.GeneratePathAsync(folder.Id))
            .ReturnsAsync(folder.Path);
        _storageService.Setup(x => x.UploadFiles(files, userId))
            .ReturnsAsync(blobs);
        
        var result = await _sut.AddFilesAsync(files, userId,folder.Id);
        
        Assert.Equal(typeof(List<FileInfo>), result.GetType());
        result.ForEach(x => Assert.True(x.Path == folder.Path));
    }

    [Theory]
    [AutoDomainData]
    public async Task RemoveFiles(List<Blob> blobs)
    {
        var data = new DataSeed();
        var files = data.FileInfosTest.Where(x => x.Path == "~").ToList();
        var userId = data.UserId;
        var deleteIds = new List<string>();
        files.ForEach(x => deleteIds.Add(x.Id));
        _blobRepository.Setup(x => x.FindAsync(x => deleteIds.Contains(x.Id) && x.UserId == userId))
            .ReturnsAsync(blobs);
        _fileRepository.Setup(x => x.FindAsync(x => blobs.Find(b => b.Name == x.BlobName) != null))
            .ReturnsAsync(files);
        
        
        await _sut.RemoveFilesAsync(deleteIds, userId);
        
        _fileRepository.Verify(x => x.RemoveRangeAsync(files), Times.Once());
    }
}