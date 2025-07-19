using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Drammer.Common.Images.Transformation;

public static class TransformationExtensions
{
    public static IServiceCollection AddImageTransformationServices(this IServiceCollection services)
    {
        services.TryAddSingleton<IImageTransformationService, ImageTransformationService>();
        services.TryAddSingleton<IWaterMarkService, WaterMarkService>();
        return services;
    }
}