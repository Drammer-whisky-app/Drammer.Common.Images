using System.Net;
using Azure;
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

    public async Task<bool> UploadImageAsync(
        BlobContainerClient client,
        byte[] data,
        string fileName,
        string contentType,
        string? cacheControl = null,
        bool overwrite = true,
        CancellationToken cancellationToken = default)
    {
        var blobFile = client.GetBlobClient(fileName);
        var blobOptions = CreateBlobUploadOptions(contentType, cacheControl);

        if (!overwrite)
        {
            blobOptions.Conditions = new BlobRequestConditions {IfNoneMatch = ETag.All};
        }

        var result = await blobFile.UploadAsync(BinaryData.FromBytes(data), blobOptions, cancellationToken);
        return result.GetRawResponse().Status == (int) HttpStatusCode.Created;
    }

    public async Task<byte[]?> DownloadImageAsync(
        BlobContainerClient client,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        var blobFile = client.GetBlobClient(fileName);
        if (!await blobFile.ExistsAsync(cancellationToken))
        {
            return null;
        }

        await using var sourceStream = new MemoryStream();
        await blobFile.DownloadToAsync(sourceStream, cancellationToken);
        sourceStream.Seek(0, SeekOrigin.Begin);
        return sourceStream.ToArray();
    }

    private static BlobUploadOptions CreateBlobUploadOptions(string contentType, string? cacheControl)
    {
        var headers = new BlobHttpHeaders {ContentType = contentType, CacheControl = cacheControl};
        return new BlobUploadOptions {HttpHeaders = headers};
    }
}