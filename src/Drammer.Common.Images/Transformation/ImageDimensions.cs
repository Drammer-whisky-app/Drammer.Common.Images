namespace Drammer.Common.Images.Transformation;

internal sealed class ImageDimensions
{
    public ImageDimensions(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }
}