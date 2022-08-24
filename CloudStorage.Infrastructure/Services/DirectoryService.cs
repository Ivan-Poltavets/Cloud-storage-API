using AutoMapper;
using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Infrastructure.Services;

public class DirectoryService : IDirectoryService
{
    private readonly IMapper _mapper;
    private readonly IFolderService _folderService;
    private readonly IFileService _fileService;

    public DirectoryService(
        IFileService fileService,
        IFolderService folderService,
        IMapper mapper)
    {
        _fileService = fileService;
        _folderService = folderService;
        _mapper = mapper;
    }

    public async Task<List<ItemDto>> GetAllInCurrentAsync(string userId, string? id)
    {
        var folders = await _folderService.GetFoldersAsync(userId, id);
        var files = await _fileService.GetFilesAsync(userId, id);

        var items = _mapper.Map<List<FolderInfo>, List<ItemDto>>(folders);
        items.AddRange(_mapper.Map<List<FileInfo>, List<ItemDto>>(files));

        return items;
    }
}
