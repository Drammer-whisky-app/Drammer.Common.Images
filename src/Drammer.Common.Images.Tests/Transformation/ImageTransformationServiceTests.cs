using Drammer.Common.Images.Transformation;
using SixLabors.ImageSharp;

namespace Drammer.Common.Images.Tests.Transformation;

public sealed class ImageTransformationServiceTests
{
    [Theory]
    [InlineData(TestHelpers.ResourceName1, 20, 20)]
    [InlineData(TestHelpers.ResourceName2, 20, 11)]
    public async Task ResizeAsync_WithWidth_ReturnsResizedImage(string resourceName, int targetWidth, int expectedHeight)
    {
        // Arrange
        var resourceImage = TestHelpers.ReadResource(resourceName);
        resourceImage.Should().NotBeNull();

        var service = new ImageTransformationService();

        // Act
        var result = await service.ResizeAsync(
            resourceImage,
            "image.webp",
            new ResizeOptions
            {
                Width = targetWidth,
                Height = null,
            });

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var image = Image.Load(result.Data);
        image.Width.Should().Be(targetWidth);
        image.Height.Should().Be(expectedHeight);
    }

    [Theory]
    [InlineData(TestHelpers.ResourceName1, 20, 20)]
    [InlineData(TestHelpers.ResourceName2, 20, 35)]
    public async Task ResizeAsync_WithHeight_ReturnsResizedImage(string resourceName, int targetHeight, int expectedWidth)
    {
        // Arrange
        var resourceImage = TestHelpers.ReadResource(resourceName);
        resourceImage.Should().NotBeNull();

        var service = new ImageTransformationService();

        // Act
        var result = await service.ResizeAsync(
            resourceImage,
            "image.webp",
            new ResizeOptions
            {
                Width = null,
                Height = targetHeight,
            });

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var image = Image.Load(result.Data);
        image.Width.Should().Be(expectedWidth);
        image.Height.Should().Be(targetHeight);
    }

    [Theory]
    [InlineData(TestHelpers.ResourceName1, TestHelpers.WidthFile1)]
    [InlineData(TestHelpers.ResourceName2, TestHelpers.WidthFile2)]
    public async Task SquareImageAsync_ReturnsImage(string resourceName, int expectedSize)
    {
        // Arrange
        var resourceImage = TestHelpers.ReadResource(resourceName);
        resourceImage.Should().NotBeNull();

        var service = new ImageTransformationService();

        // Act
        var result = await service.SquareImageAsync(
            resourceImage,
            "image.webp",
            new SquareImageOptions());

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var image = Image.Load(result.Data);
        image.Width.Should().Be(expectedSize);
        image.Height.Should().Be(expectedSize);
    }
}