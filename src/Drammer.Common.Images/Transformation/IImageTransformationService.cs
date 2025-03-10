namespace Drammer.Common.Images.Transformation;

/// <summary>
/// The image resize service.
/// </summary>
public interface IImageTransformationService
{
    /// <summary>
    /// Resizes an image to a smaller format, maintaining the aspect ratio.
    /// </summary>
    /// <param name="imageData">The image data.</param>
    /// <param name="originalFileName">The original file name.</param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResizeResult> ResizeAsync(
        byte[] imageData,
        string originalFileName,
        ResizeOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a squared version of the image.
    /// </summary>
    /// <param name="imageData"></param>
    /// <param name="originalFileName"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResizeResult> SquareImageAsync(
        byte[] imageData,
        string originalFileName,
        SquareImageOptions options,
        CancellationToken cancellationToken = default);
}