using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Data;
using CloudStorage.Infrastructure.Helpers;
using CloudStorage.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FileInfo = CloudStorage.Core.Entities.FileInfo;

namespace CloudStorage.Infrastructure;

public static class InfrastructureModule
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AuthDbContext>(options =>
        {
            options.UseNpgsql(Environment.GetEnvironmentVariable("PostgreSQL:Connection"));
        });

        services.AddSingleton<IMongoRepository<FolderInfo>, MongoRepository<FolderInfo>>();
        services.AddSingleton<IMongoRepository<FileInfo>, MongoRepository<FileInfo>>();
        services.AddSingleton<IMongoRepository<AccountExtension>, MongoRepository<AccountExtension>>();
        services.AddSingleton<IMongoRepository<Blob>, MongoRepository<Blob>>();

        services.AddTransient<IBlobStorageService, BlobStorageService>();
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IFolderService, FolderService>();
        services.AddTransient<IDirectoryService, DirectoryService>();
        services.AddTransient<IAccountService, AccountService>();

        services.AddTransient<IFolderHelper, FolderHelper>();

        return services;
    }
}
