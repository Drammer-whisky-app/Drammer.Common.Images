using SixLabors.ImageSharp;

namespace Drammer.Common.Images.Transformation;

/// <summary>
/// The options for resizing an image.
/// </summary>
public sealed class SquareImageOptions
{
    /// <summary>
    /// Gets the target content type.
    /// </summary>
    public string? TargetContentType { get; init; }

    /// <summary>
    /// Gets the quality of the resized image.
    /// </summary>
    public int? ImageQuality { get; init; } = 75;

    /// <summary>
    /// Gets the padding color.
    /// </summary>
    public Color PadColor { get; init; } = Color.Transparent;

    /// <summary>
    /// Gets a func to determine the size of the squared image.
    /// The input is the original image size and the output is the size (widht and height) of the squared image.
    /// </summary>
    public Func<(int width, int height), int>? SizeFunc { get; init; }
}