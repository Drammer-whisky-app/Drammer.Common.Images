using Microsoft.Extensions.DependencyInjection;

namespace Drammer.Common.Images.Storage;

public static class StorageExtensions
{
    public static IServiceCollection AddStorageServices(this IServiceCollection services)
    {
        services.AddSingleton<IStorageService, StorageService>();
        return services;
    }
}