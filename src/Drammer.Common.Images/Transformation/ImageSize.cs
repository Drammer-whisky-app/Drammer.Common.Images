namespace Drammer.Common.Images.Transformation;

internal sealed class ImageSize
{
    public ImageSize(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public int Width { get; }

    public int Height { get; }
}