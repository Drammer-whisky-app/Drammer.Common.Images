using Microsoft.Extensions.DependencyInjection;

namespace Drammer.Common.Images.Transformation;

public static class TransformationExtensions
{
    public static IServiceCollection AddImageTransformationServices(this IServiceCollection services)
    {
        services.AddSingleton<IImageTransformationService, ImageTransformationService>();
        services.AddSingleton<IWaterMarkService, WaterMarkService>();
        return services;
    }
}