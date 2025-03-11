using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Drammer.Common.Images.Storage;

internal sealed class StorageService : IStorageService
{
    public async Task<IReadOnlyList<BlobItem>> GetAllBlobsAsync(
        BlobContainerClient client,
        BlobTraits traits = BlobTraits.None,
        string? prefix = null,
        CancellationToken cancellationToken = default)
    {
        var result = new List<BlobItem>();
        var fileEnumerator = client.GetBlobsAsync(
            traits,
            prefix: prefix,
            cancellationToken: cancellationToken);
        await foreach (var blobItem in fileEnumerator)
        {
            result.Add(blobItem);
        }

        return result;
    }

    public async Task<BlobClient> UploadImageAsync(
        BlobContainerClient client,
        byte[] data,
        string fileName,
        string contentType,
        string? cacheControl = null,
        bool overwrite = true,
        Dictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        var blobFile = client.GetBlobClient(fileName);
        var blobOptions = CreateBlobUploadOptions(contentType, cacheControl);

        if (!overwrite)
        {
            blobOptions.Conditions = new BlobRequestConditions {IfNoneMatch = ETag.All};
        }

        _ = await blobFile.UploadAsync(BinaryData.FromBytes(data), blobOptions, cancellationToken)
            .ConfigureAwait(false);

        if (metadata != null)
        {
            _ = await blobFile.SetMetadataAsync(metadata, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        return blobFile;
    }

    public async Task<DownloadImageResult> DownloadFileAsync(
        BlobContainerClient client,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var blobFile = client.GetBlobClient(fileName);
        if (!await blobFile.ExistsAsync(cancellationToken).ConfigureAwait(false))
        {
            return new DownloadImageResult
            {
                FileName = fileName
            };
        }

        var metaData = await blobFile.GetPropertiesAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        await using var sourceStream = new MemoryStream();
        _ = await blobFile.DownloadToAsync(sourceStream, cancellationToken).ConfigureAwait(false);

        return new DownloadImageResult
        {
            FileName = fileName,
            Data = sourceStream.ToArray(),
            Metadata = metaData.Value.Metadata.AsReadOnly(),
        };
    }

    public async Task<bool> DeleteFileAsync(
        BlobContainerClient client,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
        var blobClient = client.GetBlobClient(fileName);
        var result = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
        return result.Value;
    }

    public Task<bool> DeleteFileAsync(
        BlobContainerClient client,
        Uri uri,
        CancellationToken cancellationToken = default) =>
        DeleteFileAsync(client, new BlobClient(uri).Name, cancellationToken);

    private static BlobUploadOptions CreateBlobUploadOptions(string contentType, string? cacheControl)
    {
        var headers = new BlobHttpHeaders {ContentType = contentType, CacheControl = cacheControl};
        return new BlobUploadOptions {HttpHeaders = headers};
    }
}