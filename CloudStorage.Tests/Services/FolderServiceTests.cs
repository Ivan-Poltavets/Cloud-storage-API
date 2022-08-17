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
    private readonly Mock<IRepository<FolderInfo>> _folderRepo;

    public FolderServiceTests()
    {
        _folderHelper = new Mock<IFolderHelper>();
        _folderRepo = new Mock<IRepository<FolderInfo>>();
        _sut = new FolderService(_folderRepo.Object, _folderHelper.Object);
    }
    
    [Theory]
    [AutoDomainData]
    public async Task AddFolder_WhenCurrenFolderNull(FolderDto dto)
    {
        var data = new DataSeed();
        Guid? folderId = null;
        _folderHelper.Setup(x => x.GeneratePathAsync(folderId))
            .ReturnsAsync(Constants.MainDirectory);
        
        var result = await _sut.AddFolder(dto, data.UserId, folderId);
        
        Assert.Equal(dto.Name, result.Name);
        _folderRepo.Verify(x => x.SaveChangesAsync(), Times.Once());
    }

    [Theory]
    [AutoDomainData]
    public async Task RemoveFolder_WhenFolderInsideNull(FolderInfo folder)
    {
        _folderRepo.Setup(x => x.GetByIdAsync(folder.Id))
            .ReturnsAsync(folder);
        _folderRepo.Setup(x => x.Where(f => f.Path.StartsWith(folder.Path)))
            .Returns((List<FolderInfo>)null);
        
        await _sut.RemoveFolder(folder.Id);
        
        _folderRepo.Verify(x => x.RemoveAsync(folder), Times.Once());
        _folderRepo.Verify(x => x.SaveChangesAsync(), Times.Once());
    }
}