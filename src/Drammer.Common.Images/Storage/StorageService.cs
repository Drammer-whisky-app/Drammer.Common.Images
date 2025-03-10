using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Drammer.Common.Images.Storage;

internal sealed class StorageService : IStorageService
{
    public async Task<IReadOnlyList<BlobItem>> GetAllBlobsAsync(
        BlobContainerClient client,
        string? prefix = null,
        CancellationToken cancellationToken = default)
    {
        var result = new List<BlobItem>();
        var fileEnumerator = client.GetBlobsAsync(
            BlobTraits.None,
            prefix: prefix,
            cancellationToken: cancellationToken);
        await foreach (var blobItem in fileEnumerator)
        {
            result.Add(blobItem);
        }

        return result;
    }
}