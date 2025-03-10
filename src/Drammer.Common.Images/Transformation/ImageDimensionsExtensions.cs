using System.Diagnostics;

namespace Drammer.Common.Images.Transformation;

internal static class ImageDimensionsExtensions
{
    public static (int Width, int Height) Resize(this ImageDimensions imageDimensions, int? width = null, int? height = null)
    {
        if (width == null && height == null)
        {
            return (imageDimensions.Width, imageDimensions.Height);
        }

        var newWidth = width;
        var newHeight = height;

        if (width.HasValue && height.HasValue)
        {
            // is landscape?
            if (imageDimensions.Width >= imageDimensions.Height)
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
            var ratio = (double)imageDimensions.Width / newWidth.Value;
            return ((int)Math.Round(imageDimensions.Width / ratio), (int)Math.Round(imageDimensions.Height / ratio));
        }

        if (newHeight.HasValue)
        {
            var ratio = (double)imageDimensions.Height / newHeight.Value;
            return ((int)Math.Round(imageDimensions.Width / ratio), (int)Math.Round(imageDimensions.Height / ratio));
        }

        throw new UnreachableException();
    }
}