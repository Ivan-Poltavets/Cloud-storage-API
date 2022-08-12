
using CloudStorage.Core.Entities;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Tests.Base;

public class DataSeed
{
    public static string UserId;

    public DataSeed()
    {
        UserId = Guid.NewGuid().ToString();
    }

    public DataSeed(string userId)
    {
        UserId = userId;
    }
    
    public List<FileInfo> FileInfosTest = new List<FileInfo>()
    {
        new FileInfo(UserId, "fileInfo 1", "~", 4214),
        new FileInfo(UserId, "fileInfo 2", "~", 4214),
        new FileInfo(UserId, "fileInfo 3", "~\\folder", 4214),
        new FileInfo(UserId, "fileInfo 4", "~\\folder", 4214),
        new FileInfo(UserId, "fileInfo 1", "~\\folder\\second", 4214),
    };

    public List<FolderInfo> FolderInfosTest = new List<FolderInfo>()
    {
        new FolderInfo(UserId, "folder", "~"),
        new FolderInfo(UserId, "second", "~\\folder")
    };
}