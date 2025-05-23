﻿using System.Diagnostics.CodeAnalysis;

namespace Drammer.Common.Images.Transformation;

public sealed class ResizeResult
{
    /// <summary>
    /// Gets the file data.
    /// </summary>
    public byte[]? Data { get; init; }

    /// <summary>
    /// Gets the content type.
    /// </summary>
    public required string ContentType { get; init; }

    /// <summary>
    /// Gets the file extension (with the dot).
    /// </summary>
    public required string Extension { get; init; }

    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    [MemberNotNullWhen(true, nameof(Data))]
    public bool Success => Data != null;
}