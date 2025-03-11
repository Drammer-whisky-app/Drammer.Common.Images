namespace Drammer.Common.Images.Transformation;

public static class FileNameExtensions
{
    /// <summary>
    /// Gets the content type and extension of a file.
    /// </summary>
    /// <param name="filename">The file name.</param>
    /// <returns>The content type and extension (including dot).</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="NotSupportedException"></exception>
    public static (string ContentType, string Extension) GetContentType(this string? filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
        {
            throw new ArgumentNullException(nameof(filename));
        }

        var f = new FileInfo(filename.Trim());
        var extension = f.Extension.ToLower();
        if (!extension.StartsWith("."))
        {
            extension = "." + extension;
        }

        switch (extension)
        {
            case ".jpg":
            case ".jpeg":
                return ("image/jpeg", f.Extension.ToLower());
            case ".png":
                return ("image/png", f.Extension.ToLower());
            case ".webp":
                return ("image/webp", f.Extension.ToLower());
            default:
                throw new NotSupportedException($"Extension {f.Extension} is not supported");
        }
    }
}