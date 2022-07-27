using CloudStorage.Core.Entities;
using CloudStorage.Core.Interfaces;
using CloudStorage.Infrastructure.Data;
using CloudStorage.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudStorage.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(config =>
            {
                config.AddBlobServiceClient(configuration["Microsoft:BlobStorage:ConnectionString"]);
            });

            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("AuthContext"));
            });

            services.AddDbContext<CosmosDbContext>(options =>
            {
                options.UseCosmos(configuration["Microsoft:CosmosDB:ConnectionString"], configuration["Microsoft:CosmosDB:DatabaseName"]);
            });

            services.AddScoped<IRepository<Folder>, Repository<Folder>>();
            services.AddScoped<IRepository<Core.Entities.FileInfo>, Repository<Core.Entities.FileInfo>>();
            services.AddScoped<IRepository<AccountStorage>, Repository<AccountStorage>>();

            services.AddTransient<IStorageService, StorageService>();
            services.AddTransient<IManageService, ManageService>();
            services.AddTransient<IDirectoryService, DirectoryService>();
            services.AddTransient<IAccountService, AccountService>();
        }
    }
}
