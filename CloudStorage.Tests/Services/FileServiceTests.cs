using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Services;
using CloudStorage.Tests.Base;
using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;
using Xunit;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Tests.Services;

public class FileServiceTests
{
    public Fixture Fixture { get; set; }
    
    private readonly FileService _sut;
    private readonly Mock<IBlobStorageService> _storageService;
    private readonly Mock<IAccountService> _accountService;
    private readonly Mock<IRepository<FileInfo>> _fileRepository;
    private readonly Mock<IFolderHelper> _folderHelper;
    
    public FileServiceTests()
    {
        Fixture = new Fixture();
        Fixture.Customize(new AutoMoqCustomization());

        _storageService = new Mock<IBlobStorageService>();
        _accountService = new Mock<IAccountService>();
        _fileRepository = new Mock<IRepository<FileInfo>>();
        _folderHelper = new Mock<IFolderHelper>();
        _sut = new FileService(
            _fileRepository.Object,
            _accountService.Object,
            _storageService.Object,
            _folderHelper.Object);
    }

    [Theory]
    [AutoDomainData]
    public async Task AddFiles(List<string> names, string userId)
    {
        var folder = new DataSeed().FolderInfosTest.SingleOrDefault(x => x.Name == "second");
        var files = Fixture.Create<List<IFormFile>>();
        var expectedFiles = new List<FileInfo>();
        for (int i = 0; i < files.Count; i++)
        {
            expectedFiles.Add(new FileInfo
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = files[i].FileName,
                BlobName = names[i],
                Size = files[i].Length,
                Path = folder.Path
            });
        }
        _folderHelper.Setup(x => x.GeneratePathAsync(folder.Id))
            .ReturnsAsync(folder.Path);
        _storageService.Setup(x => x.UploadFiles(files))
            .Returns(names);
        
        var result = await _sut.AddFiles(files, userId,folder.Id);
        
        Assert.Equal(typeof(List<FileInfo>), result.GetType());
        result.ForEach(x => Assert.True(x.Path == folder.Path));
        _fileRepository.Verify(x => x.SaveChangesAsync(), Times.Once());
    }

    [Fact]
    public async Task RemoveFiles()
    {
        var data = new DataSeed();
        var files = data.FileInfosTest.Where(x => x.Path == "~").ToList();
        var userId = data.UserId;
        var deleteIds = new List<Guid>();
        files.ForEach(x => deleteIds.Add(x.Id));
        _fileRepository.Setup(x => x.Where(x => deleteIds.Contains(x.Id) && x.UserId == userId))
            .Returns(files);
        
        await _sut.RemoveFiles(deleteIds, userId);
        
        _fileRepository.Verify(x => x.RemoveRangeAsync(files), Times.Once());
        _fileRepository.Verify(x => x.SaveChangesAsync(), Times.Once());
    }
}