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
    private readonly Mock<IRepository<FileInfo>>_fileInfoRepoMock;
    private readonly Mock<IRepository<FolderInfo>> _folderRepoMock;

    public DirectoryServiceTests()
    {
        _fileInfoRepoMock = new Mock<IRepository<FileInfo>>();
        _folderRepoMock = new Mock<IRepository<FolderInfo>>();
        _sut = new DirectoryService(_fileInfoRepoMock.Object, _folderRepoMock.Object);
    }

    [Theory]
    [AutoDomainData]
    public async Task GetAllInCurrent_WithoutFolderId(string userId)
    {
        var data = new DataSeed(userId);
        var expected = new List<ItemDto>();
        data.FileInfosTest.ForEach(x => expected.Add(new ItemDto()
        {
            Id = x.Id,
            Name = x.Name,
            Path = x.PathToFile,
            Type = nameof(FileInfo)
        }));
        data.FolderInfosTest.ForEach(x => expected.Add(new ItemDto()
        {
            Id = x.Id,
            Name = x.Name,
            Path = x.Path,
            Type = nameof(FolderInfo)
        }));

        _fileInfoRepoMock.Setup(x => x.AddRangeAsync(data.FileInfosTest))
            .ReturnsAsync(data.FileInfosTest);
        _folderRepoMock.Setup(x => x.AddRangeAsync(data.FolderInfosTest))
            .ReturnsAsync(data.FolderInfosTest);
        _folderRepoMock.Setup(x => x.Where(x => x.UserId == userId))
            .Returns(data.FolderInfosTest);
        _fileInfoRepoMock.Setup(x => x.Where(x => x.UserId == userId))
            .Returns(data.FileInfosTest);
        
        await _fileInfoRepoMock.Object.AddRangeAsync(data.FileInfosTest);
        await _folderRepoMock.Object.AddRangeAsync(data.FolderInfosTest);
        var result = await _sut.GetAllInCurrent(userId, null);
        
        Assert.Equal(typeof(List<ItemDto>), result.GetType());
        result.ForEach(x => Assert.True(x.Path == "~"));
        Assert.Equal(3,result.Count);
    }

    [Theory]
    [AutoDomainData]
    public async Task GetAllInCurrent_WithFolderId(string userId)
    {
        var data = new DataSeed(userId);
        var expected = new List<ItemDto>();
        data.FileInfosTest.ForEach(x => expected.Add(new ItemDto()
        {
            Id = x.Id,
            Name = x.Name,
            Path = x.PathToFile,
            Type = nameof(FileInfo)
        }));
        data.FolderInfosTest.ForEach(x => expected.Add(new ItemDto()
        {
            Id = x.Id,
            Name = x.Name,
            Path = x.Path,
            Type = nameof(FolderInfo)
        }));
        var folder = data.FolderInfosTest.Single(x => x.Name == "folder");
        _fileInfoRepoMock.Setup(x => x.AddRangeAsync(data.FileInfosTest))
            .ReturnsAsync(data.FileInfosTest);
        _folderRepoMock.Setup(x => x.AddRangeAsync(data.FolderInfosTest))
            .ReturnsAsync(data.FolderInfosTest);
        _folderRepoMock.Setup(x => x.Where(x => x.UserId == userId))
            .Returns(data.FolderInfosTest);
        _fileInfoRepoMock.Setup(x => x.Where(x => x.UserId == userId))
            .Returns(data.FileInfosTest);
        _folderRepoMock.Setup(x => x.GetByIdAsync(folder.Id))
            .ReturnsAsync(folder);
        
        await _fileInfoRepoMock.Object.AddRangeAsync(data.FileInfosTest);
        await _folderRepoMock.Object.AddRangeAsync(data.FolderInfosTest);
        var result = await _sut.GetAllInCurrent(userId, folder.Id);
        
        Assert.Equal(typeof(List<ItemDto>), result.GetType());
        Assert.Equal(3, result.Count);
    }
}