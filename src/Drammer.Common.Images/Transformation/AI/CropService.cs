using Azure;
using Azure.AI.Vision.ImageAnalysis;
using Microsoft.Extensions.Options;

namespace Drammer.Common.Images.Transformation.AI;

public sealed class CropService
{
    private readonly IOptions<AIOptions> _options;

    public CropService(IOptions<AIOptions> options)
    {
        _options = options;
    }

    public async Task<SmartCropsResult> SmartCropAsync(byte[] imageData)
    {
        var client = new ImageAnalysisClient(
            new Uri(_options.Value.Endpoint),
            new AzureKeyCredential(_options.Value.Key));

        var result = await client.AnalyzeAsync(
            BinaryData.FromBytes(imageData),
            VisualFeatures.SmartCrops);

        return result.Value.SmartCrops;
    }
}