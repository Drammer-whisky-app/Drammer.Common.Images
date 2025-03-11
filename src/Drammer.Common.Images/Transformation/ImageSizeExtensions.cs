using System.Diagnostics;

namespace Drammer.Common.Images.Transformation;

internal static class ImageSizeExtensions
{
    public static (int Width, int Height) Resize(this ImageSize imageSize, int? width = null, int? height = null)
    {
        if (width == null && height == null)
        {
            return (imageSize.Width, imageSize.Height);
        }

        var newWidth = width;
        var newHeight = height;

        if (width.HasValue && height.HasValue)
        {
            // is landscape?
            if (imageSize.Width >= imageSize.Height)
            {
                // set height to null to maintain aspect ratio
                newHeight = null;
            }
            else
            {
                // portrait
                // set width to null
                newWidth = null;
            }
        }

        if (newWidth.HasValue)
        {
            var ratio = (double)imageSize.Width / newWidth.Value;
            return ((int)Math.Round(imageSize.Width / ratio), (int)Math.Round(imageSize.Height / ratio));
        }

        if (newHeight.HasValue)
        {
            var ratio = (double)imageSize.Height / newHeight.Value;
            return ((int)Math.Round(imageSize.Width / ratio), (int)Math.Round(imageSize.Height / ratio));
        }

        throw new UnreachableException();
    }
}