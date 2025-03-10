using System.Reflection;
using Drammer.Common.Images.Transformation;
using SixLabors.ImageSharp;

namespace Drammer.Common.Images.Tests.Transformation;

public sealed class ImageTransformationServiceTests
{
    private const string FileName = "DALL·E 2025-03-10 16.25.30.webp";
    private const string ResourceName = $"Drammer.Common.Images.Tests.Resources.{FileName}";
    private const int Width = 1024;
    private const int Height = 1024;

    [Fact]
    public async Task ResizeAsync_WithWidth_ReturnsResizedImage()
    {
        // Arrange
        var resourceImage = ReadResource(ResourceName);
        resourceImage.Should().NotBeNull();
        const int TargetWidth = 20;

        var service = new ImageTransformationService();

        // Act
        var result = await service.ResizeAsync(
            resourceImage,
            FileName,
            new ResizeOptions
            {
                Width = TargetWidth,
                Height = null,
            });

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var image = Image.Load(result.Data);
        image.Width.Should().Be(TargetWidth);
        image.Height.Should().Be(TargetWidth);
    }

    [Fact]
    public async Task ResizeAsync_WithHeight_ReturnsResizedImage()
    {
        // Arrange
        var resourceImage = ReadResource(ResourceName);
        resourceImage.Should().NotBeNull();
        const int TargetHeight = 20;

        var service = new ImageTransformationService();

        // Act
        var result = await service.ResizeAsync(
            resourceImage,
            FileName,
            new ResizeOptions
            {
                Width = null,
                Height = TargetHeight,
            });

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var image = Image.Load(result.Data);
        image.Width.Should().Be(TargetHeight);
        image.Height.Should().Be(TargetHeight);
    }

    [Fact]
    public async Task SquareImageAsync_ReturnsImage()
    {
        // Arrange
        var resourceImage = ReadResource(ResourceName);
        resourceImage.Should().NotBeNull();

        var service = new ImageTransformationService();

        // Act
        var result = await service.SquareImageAsync(
            resourceImage,
            FileName,
            new SquareImageOptions());

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var image = Image.Load(result.Data);
        image.Width.Should().Be(Width);
        image.Height.Should().Be(Height);
    }

    private static byte[]? ReadResource(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        using var resFilestream = assembly.GetManifestResourceStream(fileName);
        if (resFilestream == null)
        {
            return null;
        }

        var ba = new byte[resFilestream.Length];
        resFilestream.ReadExactly(ba, 0, ba.Length);
        return ba;
    }
}