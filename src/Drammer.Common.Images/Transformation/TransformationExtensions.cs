using Microsoft.Extensions.DependencyInjection;

namespace Drammer.Common.Images.Transformation;

public static class TransformationExtensions
{
    public static IServiceCollection AddStorageServices(this IServiceCollection services)
    {
        services.AddSingleton<IImageTransformationService, ImageTransformationService>();
        return services;
    }
}