using System.Diagnostics.CodeAnalysis;

namespace Drammer.Common.Images.Storage;

/// <summary>
/// The result of downloading an image.
/// </summary>
public sealed class DownloadImageResult
{
    /// <summary>
    /// Gets the file name.
    /// </summary>
    public required string FileName { get; init; }

    /// <summary>
    /// Gets the data.
    /// </summary>
    public byte[]? Data { get; init; }

    /// <summary>
    /// Gets the metadata.
    /// </summary>
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();

    /// <summary>
    /// Gets a value indicating whether the download was successful.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Data))]
    public bool Success => Data != null;
}