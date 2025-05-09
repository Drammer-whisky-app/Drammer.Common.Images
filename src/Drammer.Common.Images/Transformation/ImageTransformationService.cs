using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Drammer.Common.Images.Transformation;

/// <summary>
/// The image transformation service.
/// </summary>
public sealed class ImageTransformationService : IImageTransformationService
{
    /// <inheritdoc />
    public async Task<ResizeResult> ResizeAsync(
        byte[] imageData,
        ResizeOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(imageData);
        ArgumentNullException.ThrowIfNull(options);

        var imageFormat = Image.DetectFormat(imageData);
        var width = options.Width;
        var height = options.Height;

        using var image = Image.Load(imageData);

        // do not enlarge the image
        if (width.HasValue && width > image.Width)
        {
            width = image.Width;
        }

        if (height.HasValue && height > image.Height)
        {
            height = image.Height;
        }

        // set original image size
        if (width == null && height == null)
        {
            width = image.Width;
            height = image.Height;
        }

        var imageSize = new ImageSize(image.Width, image.Height);
        var resizeResult = imageSize.Resize(width, height);
        if (options.AutoOrient)
        {
            image.Mutate(x => x.AutoOrient().Resize(resizeResult.Width, resizeResult.Height));
        }
        else
        {
            image.Mutate(x => x.Resize(resizeResult.Width, resizeResult.Height));
        }

        var result = await SaveImageAsync(
            image,
            options.TargetContentType ?? imageFormat.DefaultMimeType,
            options.ImageQuality,
            cancellationToken).ConfigureAwait(false);

        return new ResizeResult
        {
            ContentType = result.ContentType,
            Extension = result.Extension,
            Data = result.Data
        };
    }

    /// <inheritdoc />
    public async Task<ResizeResult> SquareImageAsync(
        byte[] imageData,
        SquareImageOptions options,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(imageData);
        ArgumentNullException.ThrowIfNull(options);

        var imageFormat = Image.DetectFormat(imageData);
        using var image = Image.Load(imageData);
        var maxSize = Math.Max(image.Width, image.Height);

        if (options.SizeFunc != null)
        {
            maxSize = options.SizeFunc((image.Width, image.Height));
        }

        image.Mutate(
            x => x.Resize(
                new SixLabors.ImageSharp.Processing.ResizeOptions
                {
                    Mode = ResizeMode.BoxPad, Size = new Size(maxSize), PadColor = options.PadColor,
                }).BackgroundColor(Color.White));

        var result = await SaveImageAsync(
            image,
            options.TargetContentType ?? imageFormat.DefaultMimeType,
            options.ImageQuality,
            cancellationToken).ConfigureAwait(false);

        return new ResizeResult
        {
            ContentType = result.ContentType,
            Extension = result.Extension,
            Data = result.Data
        };
    }

    /// <inheritdoc />
    public async Task<byte[]> RotateAsync(
        byte[] imageData,
        int degrees = 90,
        CancellationToken cancellationToken = default)
    {
        var imageFormat = Image.DetectFormat(imageData);
        var image = Image.Load(imageData);
        image.Mutate(x => x.Rotate(degrees));
        using var ms = new MemoryStream();
        await image.SaveAsync(ms, imageFormat, cancellationToken).ConfigureAwait(false);
        return ms.ToArray();
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