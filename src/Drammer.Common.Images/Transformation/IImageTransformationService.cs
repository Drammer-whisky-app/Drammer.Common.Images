﻿namespace Drammer.Common.Images.Transformation;

/// <summary>
/// The image resize service.
/// </summary>
public interface IImageTransformationService
{
    /// <summary>
    /// Resizes an image to a smaller format, maintaining the aspect ratio.
    /// </summary>
    /// <param name="imageData">The image data.</param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResizeResult> ResizeAsync(
        byte[] imageData,
        ResizeOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a squared version of the image.
    /// </summary>
    /// <param name="imageData"></param>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ResizeResult> SquareImageAsync(
        byte[] imageData,
        SquareImageOptions options,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Rotates an image.
    /// </summary>
    /// <param name="imageData"></param>
    /// <param name="degrees"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<byte[]> RotateAsync(byte[] imageData, int degrees = 90, CancellationToken cancellationToken = default);
}