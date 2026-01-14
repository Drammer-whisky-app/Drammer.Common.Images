using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Drammer.Common.Images.Storage;

/// <summary>
/// The storage service.
/// </summary>
public interface IStorageService
{
    /// <summary>
    /// Lists all blob items in a container.
    /// </summary>
    /// <param name="client">The container client.</param>
    /// <param name="traits">The blob traits.</param>
    /// <param name="states">The blob states.</param>
    /// <param name="prefix">The prefix (optional).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="BlobItem"/>.</returns>
    Task<IReadOnlyList<BlobItem>> GetAllBlobsAsync(
        BlobContainerClient client,
        BlobTraits traits = BlobTraits.None,
        BlobStates states = BlobStates.None,
        string? prefix = null,
        CancellationToken cancellationToken = default);

    Task<BlobClient> UploadImageAsync(
        BlobContainerClient client,
        byte[] data,
        string fileName,
        string contentType,
        string? cacheControl = null,
        bool overwrite = true,
        Dictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default);

    Task<DownloadImageResult> DownloadFileAsync(
        BlobContainerClient client,
        string fileName,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteFileAsync(
        BlobContainerClient client,
        string fileName,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteFileAsync(
        BlobContainerClient client,
        Uri uri,
        CancellationToken cancellationToken = default);
}