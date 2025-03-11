using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Drammer.Common.Images.Transformation;

internal sealed class WaterMarkService : IWaterMarkService
{
    public async Task<byte[]> AddWaterMarkAsync(
        byte[] image,
        byte[] waterMark,
        CancellationToken cancellationToken = default)
    {
        using var img = Image.Load(image);
        using var waterMarkImage = Image.Load(waterMark);

        var location = new Point(img.Width - waterMarkImage.Size.Width - 5, img.Height - waterMarkImage.Size.Height - 5);

        // ReSharper disable once AccessToDisposedClosure
        img.Mutate(
            i => i.DrawImage(
                waterMarkImage,
                location,
                new GraphicsOptions {AlphaCompositionMode = PixelAlphaCompositionMode.SrcOver}));

        await using var targetStream = new MemoryStream();
        await img.SaveAsync(targetStream, new WebpEncoder(), cancellationToken).ConfigureAwait(false);

        return targetStream.ToArray();
    }
}