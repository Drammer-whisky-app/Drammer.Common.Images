namespace Drammer.Common.Images.Storage;

public sealed class DownloadImageResult
{
    public required string FileName { get; init; }

    public byte[]? Data { get; init; }

    public IDictionary<string, string>? Metadata { get; init; }

    public bool Success => Data != null;
}