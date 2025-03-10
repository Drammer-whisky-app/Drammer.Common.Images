namespace Drammer.Common.Images.Transformation;

internal static class FileNameExtensions
{
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