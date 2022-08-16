using AutoFixture.Xunit2;
using AutoMapper;
using CloudStorage.Core.Dtos;
using Moq;
using Xunit;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Services;
using CloudStorage.Tests.Base;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Tests.Services;

public class DirectoryServiceTests
{
    private readonly DirectoryService _sut;
    private readonly Mock<IFolderService> _folderServiceMock;
    private readonly Mock<IFileService> _fileServiceMock;
    private readonly Mock<IMapper> _mapperMock;

    public DirectoryServiceTests()
    {
        _mapperMock = new Mock<IMapper>();
        _fileServiceMock = new Mock<IFileService>();
        _folderServiceMock = new Mock<IFolderService>();
        _sut = new DirectoryService(
            _fileServiceMock.Object,
            _folderServiceMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllInCurrent_WithoutFolderId()
    {
        var data = new DataSeed();
        string userId = data.UserId;
        var expectedFiles = new List<ItemDto>();
        var fileServiceReturn = data.FileInfosTest.Where(f => f.Path == "~").ToList();
        var folderServiceReturn = data.FolderInfosTest.Where(f => f.Path == "~").ToList();
        fileServiceReturn.ForEach(x => expectedFiles.Add(new ItemDto()
        {
            Id = x.Id,
            Name = x.Name,
            Path = x.Path,
            Type = nameof(FileInfo)
        }));
        var expectedFolders = new List<ItemDto>();
        folderServiceReturn.ForEach(x => expectedFolders.Add(new ItemDto()
        {
            Id = x.Id,
            Name = x.Name,
            Path = x.Path,
            Type = nameof(FolderInfo)
        }));

        _fileServiceMock.Setup(x => x.GetFiles(userId, null))
            .ReturnsAsync(fileServiceReturn);
        _folderServiceMock.Setup(x => x.GetFolders(userId, null))
            .ReturnsAsync(folderServiceReturn);
        _mapperMock.Setup(x => x.Map<List<FolderInfo>, List<ItemDto>>(folderServiceReturn))
            .Returns(expectedFolders);
        _mapperMock.Setup(x => x.Map<List<FileInfo>, List<ItemDto>>(fileServiceReturn))
            .Returns(expectedFiles);
        
        var result = await _sut.GetAllInCurrent(userId, null);
        
        Assert.Equal(typeof(List<ItemDto>), result.GetType());
        result.ForEach(x => Assert.True(x.Path == "~"));
        Assert.Equal(3,result.Count);
    }

    [Fact]
    public async Task GetAllInCurrent_WithFolderId()
    {
        var data = new DataSeed();
        string userId = data.UserId;
        var expectedFiles = new List<ItemDto>();
        var folder = data.FolderInfosTest.Single(x => x.Name == "folder");
        var fileServiceReturn = data.FileInfosTest.Where(f => f.Path == folder.Path).ToList();
        var folderServiceReturn = data.FolderInfosTest.Where(f => f.Path == folder.Path).ToList();
        fileServiceReturn.ForEach(x => expectedFiles.Add(new ItemDto()
        {
            Id = x.Id,
            Name = x.Name,
            Path = x.Path,
            Type = nameof(FileInfo)
        }));
        var expectedFolders = new List<ItemDto>();
        folderServiceReturn.ForEach(x => expectedFolders.Add(new ItemDto()
        {
            Id = x.Id,
            Name = x.Name,
            Path = x.Path,
            Type = nameof(FolderInfo)
        }));
        
        _fileServiceMock.Setup(x => x.GetFiles(userId, folder.Id))
            .ReturnsAsync(fileServiceReturn);
        _folderServiceMock.Setup(x => x.GetFolders(userId, folder.Id))
            .ReturnsAsync(folderServiceReturn);
        _mapperMock.Setup(x => x.Map<List<FolderInfo>, List<ItemDto>>(folderServiceReturn))
            .Returns(expectedFolders);
        _mapperMock.Setup(x => x.Map<List<FileInfo>, List<ItemDto>>(fileServiceReturn))
            .Returns(expectedFiles);
        
        var result = await _sut.GetAllInCurrent(userId, folder.Id);
        
        Assert.Equal(typeof(List<ItemDto>), result.GetType());
        Assert.Equal(3, result.Count);
    }
}