using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Drammer.Common.Images.Storage;

public static class StorageExtensions
{
    public static IServiceCollection AddStorageServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IStorageService, StorageService>();
        return services;
    }
}