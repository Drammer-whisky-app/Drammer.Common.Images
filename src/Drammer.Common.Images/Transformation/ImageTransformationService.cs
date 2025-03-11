using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Drammer.Common.Images.Transformation;

internal sealed class ImageTransformationService : IImageTransformationService
{
    public async Task<ResizeResult> ResizeAsync(
        byte[] imageData,
        string originalFileName,
        ResizeOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(imageData);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(originalFileName);

        var (contentType, extension) = originalFileName.GetContentType();

        var width = options.Width;
        var height = options.Height;

        using var image = Image.Load(imageData);

        var imageDimensions = new ImageDimensions(image.Width, image.Height);

        // do not enlarge the image
        if (width.HasValue && width > image.Width)
        {
            width = image.Width;
        }

        if (height.HasValue && height > image.Height)
        {
            height = image.Height;
        }

        var resizeResult = imageDimensions.Resize(width, height);

        image.Mutate(x => x.Resize(resizeResult.Width, resizeResult.Height));

        var result = await SaveImageAsync(
            image,
            options.TargetContentType ?? contentType,
            options.ImageQuality,
            cancellationToken).ConfigureAwait(false);

        return new ResizeResult
        {
            ContentType = result.ContentType,
            Extension = result.Extension,
            Data = result.Data
        };
    }

    public async Task<ResizeResult> SquareImageAsync(
        byte[] imageData,
        string originalFileName,
        SquareImageOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(imageData);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentException.ThrowIfNullOrWhiteSpace(originalFileName);

        var (contentType, extension) = originalFileName.GetContentType();

        using var image = Image.Load(imageData);
        var maxSize = Math.Max(image.Width, image.Height);

        image.Mutate(
            x => x.Resize(
                new SixLabors.ImageSharp.Processing.ResizeOptions
                {
                    Mode = ResizeMode.BoxPad, Size = new Size(maxSize), PadColor = Color.White,
                }).BackgroundColor(Color.White));

        var result = await SaveImageAsync(
            image,
            options.TargetContentType ?? contentType,
            options.ImageQuality,
            cancellationToken).ConfigureAwait(false);

        return new ResizeResult
        {
            ContentType = result.ContentType,
            Extension = result.Extension,
            Data = result.Data
        };
    }

    private static async Task<ResizeResult> SaveImageAsync(
        Image image,
        string targetContentType,
        int? imageQuality,
        CancellationToken cancellationToken = default)
    {
        using var ms = new MemoryStream();
        string extension;
        byte[]? resizeBytes;
        if (targetContentType is "image/jpg" or "image/jpeg")
        {
            extension = ".jpg";
            await image.SaveAsync(ms, new JpegEncoder {Quality = imageQuality}, cancellationToken).ConfigureAwait(false);
            resizeBytes = ms.ToArray();
            targetContentType = "image/jpeg";
        }
        else if (targetContentType == "image/webp")
        {
            extension = ".webp";
            await image.SaveAsync(
                ms,
                new WebpEncoder
                    {Quality = imageQuality ?? 75, TransparentColorMode = WebpTransparentColorMode.Preserve},
                cancellationToken).ConfigureAwait(false);
            resizeBytes = ms.ToArray();
        }
        else if (targetContentType == "image/png")
        {
            extension = ".png";
            await image.SaveAsync(ms, new PngEncoder(), cancellationToken).ConfigureAwait(false);
            resizeBytes = ms.ToArray();
        }
        else
        {
            throw new NotSupportedException($"Target content type {targetContentType} is not supported");
        }

        return new ResizeResult {ContentType = targetContentType, Extension = extension, Data = resizeBytes};
    }
}