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

        var imageFormat = Image.DetectFormat(result.Data);
        imageFormat.Should().NotBeNull();
        imageFormat.DefaultMimeType.Should().Be("image/webp");
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

        var imageFormat = Image.DetectFormat(result.Data);
        imageFormat.Should().NotBeNull();
        imageFormat.DefaultMimeType.Should().Be("image/webp");
    }

    [Fact]
    public async Task ResizeAsync_WithTargetContentType_ReturnsResizedImage()
    {
        // Arrange
        var resourceImage = TestHelpers.ReadResource(TestHelpers.ResourceName1);
        resourceImage.Should().NotBeNull();

        var service = new ImageTransformationService();

        // Act
        var result = await service.ResizeAsync(
            resourceImage,
            new ResizeOptions
            {
                Width = null,
                Height = 20,
                TargetContentType = "image/jpeg",
            });

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var imageFormat = Image.DetectFormat(result.Data);
        imageFormat.Should().NotBeNull();
        imageFormat.DefaultMimeType.Should().Be("image/jpeg");
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
            new SquareImageOptions());

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var image = Image.Load(result.Data);
        image.Width.Should().Be(expectedSize);
        image.Height.Should().Be(expectedSize);

        var imageFormat = Image.DetectFormat(result.Data);
        imageFormat.Should().NotBeNull();
        imageFormat.DefaultMimeType.Should().Be("image/webp");
    }

    [Fact]
    public async Task SquareImageAsync_WithSizeFunc_ReturnsImage()
    {
        // Arrange
        var resourceImage = TestHelpers.ReadResource(TestHelpers.ResourceName2);
        resourceImage.Should().NotBeNull();
        var options = new SquareImageOptions
        {
            SizeFunc = size => Math.Max(size.width, size.height) + (size.height / 50)
        };

        var service = new ImageTransformationService();

        // Act
        var result = await service.SquareImageAsync(
            resourceImage,
            options);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var image = Image.Load(result.Data);
        image.Width.Should().BeGreaterThan(TestHelpers.WidthFile2);
        image.Height.Should().BeGreaterThan(TestHelpers.WidthFile2);

        var imageFormat = Image.DetectFormat(result.Data);
        imageFormat.Should().NotBeNull();
        imageFormat.DefaultMimeType.Should().Be("image/webp");
    }

    [Fact]
    public async Task RotateAsync_ReturnsRotatedImage()
    {
        // Arrange
        var resourceImage = TestHelpers.ReadResource(TestHelpers.ResourceName2);
        resourceImage.Should().NotBeNull();

        var service = new ImageTransformationService();

        // Act
        var result = await service.RotateAsync(resourceImage);

        // Assert
        result.Should().NotBeNull();

        var rotatedImage = Image.Load(result);
        rotatedImage.Width.Should().Be(TestHelpers.HeightFile2);
        rotatedImage.Height.Should().Be(TestHelpers.WidthFile2);
    }
}