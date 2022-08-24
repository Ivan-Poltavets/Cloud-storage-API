using CloudStorage.Core.Constants;
using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Services;
using CloudStorage.Tests.Base;
using Moq;
using Xunit;

namespace CloudStorage.Tests.Services;

public class FolderServiceTests
{
    private readonly FolderService _sut;
    private readonly Mock<IFolderHelper> _folderHelper;
    private readonly Mock<IMongoRepository<FolderInfo>> _folderRepo;

    public FolderServiceTests()
    {
        _folderHelper = new Mock<IFolderHelper>();
        _folderRepo = new Mock<IMongoRepository<FolderInfo>>();
        _sut = new FolderService(_folderRepo.Object, _folderHelper.Object);
    }
    
    [Theory]
    [AutoDomainData]
    public async Task AddFolder_WhenCurrenFolderNull(FolderDto dto)
    {
        var data = new DataSeed();
        string? folderId = null;
        _folderHelper.Setup(x => x.GeneratePathAsync(folderId))
            .ReturnsAsync(Constants.MainDirectory);
        
        var result = await _sut.AddFolderAsync(dto, data.UserId, folderId);
        
        Assert.Equal(dto.Name, result.Name);
        _folderRepo.Verify(x => x.AddAsync(result), Times.Once());
    }

    [Theory]
    [AutoDomainData]
    public async Task RemoveFolder_WhenFolderInsideNull(FolderInfo folder)
    {
        _folderRepo.Setup(x => x.GetByIdAsync(folder.Id))
            .ReturnsAsync(folder);
        _folderRepo.Setup(x => x.Find(f => f.Path.StartsWith(folder.Path)))
            .ReturnsAsync((List<FolderInfo>)null);
        
        await _sut.RemoveFolderAsync(folder.Id);
        
        _folderRepo.Verify(x => x.RemoveAsync(folder.Id), Times.Once());
    }
}