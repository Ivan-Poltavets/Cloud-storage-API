using Moq;
using Xunit;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Services;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Tests.Services;

public class ManageServiceTests
{
    private readonly ManageService _sut;
    private readonly Mock<IBlobStorageService> _storageServiceMock;
    private readonly Mock<IAccountService> _accountServiceMock;
    private readonly Mock<IRepository<FolderInfo>> _folderRepoMock;
    private readonly Mock<IRepository<FileInfo>> _fileInfoRepoMock;

    public ManageServiceTests()
    {
        _storageServiceMock = new Mock<IBlobStorageService>();
        _accountServiceMock = new Mock<IAccountService>();
        _folderRepoMock = new Mock<IRepository<FolderInfo>>();
        _fileInfoRepoMock = new Mock<IRepository<FileInfo>>();
        _sut = new ManageService(
            _storageServiceMock.Object, 
            _accountServiceMock.Object, 
            _fileInfoRepoMock.Object,
            _folderRepoMock.Object);
    }

    [Fact]
    public async Task AddFiles()
    {
        
    }

    [Fact]
    public async Task RemoveFiles()
    {
        
    }

    [Fact]
    public async Task AddFolder()
    {
        
    }

    [Fact]
    public async Task RemoveFolder()
    {
        
    }
}