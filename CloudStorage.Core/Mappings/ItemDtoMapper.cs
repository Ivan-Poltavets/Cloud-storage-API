using AutoMapper;
using CloudStorage.Core.Dtos;
using CloudStorage.Core.Entities;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Core.Mappings;

public class ItemDtoMapper : Profile
{
    public ItemDtoMapper()
    {
        CreateMap<FileInfo, ItemDto>()
            .ForMember(x => x.Type, 
                opt => opt.MapFrom(nameof(FileInfo)));
        CreateMap<FolderInfo, ItemDto>()
            .ForMember(x => x.Type, 
                opt => opt.MapFrom(nameof(FolderInfo)));
        CreateMap<List<FileInfo>, List<ItemDto>>();
        CreateMap<List<FolderInfo>, List<ItemDto>>();
    }
}