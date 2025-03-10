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
}