using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace CloudStorage.Core;

public static class CoreModule
{
    public static IServiceCollection AddCore(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        return services;
    }
}