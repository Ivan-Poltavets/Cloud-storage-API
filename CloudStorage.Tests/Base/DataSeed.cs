
using CloudStorage.Core.Entities;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Tests.Base;

public class DataSeed
{
    public string UserId;

    public List<FileInfo> FileInfosTest;

    public List<FolderInfo> FolderInfosTest;
    
    public DataSeed()
    {
        UserId = Guid.NewGuid().ToString();
        FolderInfosTest = new List<FolderInfo>()
        {
            new FolderInfo(UserId, "folder", "~"),
            new FolderInfo(UserId, "second", "~\\folder")
        };
        FileInfosTest = new List<FileInfo>()
        {
            new FileInfo(UserId, "fileInfo 1", "~", 4214),
            new FileInfo(UserId, "fileInfo 2", "~", 4214),
            new FileInfo(UserId, "fileInfo 3", "~\\folder", 4214),
            new FileInfo(UserId, "fileInfo 4", "~\\folder", 4214),
            new FileInfo(UserId, "fileInfo 1", "~\\folder\\second", 4214),
        };
    }
}