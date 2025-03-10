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
    /// <param name="prefix">The prefix (optional).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A list of <see cref="BlobItem"/>.</returns>
    Task<IReadOnlyList<BlobItem>> GetAllBlobsAsync(
        BlobContainerClient client,
        string? prefix = null,
        CancellationToken cancellationToken = default);
}