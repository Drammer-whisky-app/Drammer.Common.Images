namespace Drammer.Common.Images.Transformation;

/// <summary>
/// The options for resizing an image.
/// </summary>
public sealed class ResizeOptions
{
    /// <summary>
    /// Gets the max width of the resized image.
    /// </summary>
    public required int? Width { get; init; }

    /// <summary>
    /// Gets the max height of the resized image.
    /// </summary>
    public required int? Height { get; init; }

    /// <summary>
    /// Gets the target content type.
    /// </summary>
    public string? TargetContentType { get; init; }

    /// <summary>
    /// Gets the quality of the resized image.
    /// </summary>
    public int? ImageQuality { get; init; } = 75;
}