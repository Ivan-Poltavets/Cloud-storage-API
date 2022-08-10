using Moq;
using Xunit;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Services;
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

    [Fact]
    public async Task GetAllInCurrent_WithoutFolderId()
    {
        
    }

    [Fact]
    public async Task GetAllInCurrent_WithFolderId()
    {
        
    }
}