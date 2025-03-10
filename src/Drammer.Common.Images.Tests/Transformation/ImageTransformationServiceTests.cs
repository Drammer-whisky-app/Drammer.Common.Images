using Drammer.Common.Images.Transformation;
using SixLabors.ImageSharp;

namespace Drammer.Common.Images.Tests.Transformation;

public sealed class ImageTransformationServiceTests
{
    [Fact]
    public async Task ResizeAsync_WithWidth_ReturnsResizedImage()
    {
        // Arrange
        var resourceImage = TestHelpers.ReadResource(TestHelpers.ResourceName);
        resourceImage.Should().NotBeNull();
        const int TargetWidth = 20;

        var service = new ImageTransformationService();

        // Act
        var result = await service.ResizeAsync(
            resourceImage,
            TestHelpers.FileName,
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
        var resourceImage = TestHelpers.ReadResource(TestHelpers.ResourceName);
        resourceImage.Should().NotBeNull();
        const int TargetHeight = 20;

        var service = new ImageTransformationService();

        // Act
        var result = await service.ResizeAsync(
            resourceImage,
            TestHelpers.FileName,
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
        var resourceImage = TestHelpers.ReadResource(TestHelpers.ResourceName);
        resourceImage.Should().NotBeNull();

        var service = new ImageTransformationService();

        // Act
        var result = await service.SquareImageAsync(
            resourceImage,
            TestHelpers.FileName,
            new SquareImageOptions());

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();

        var image = Image.Load(result.Data);
        image.Width.Should().Be(TestHelpers.Width);
        image.Height.Should().Be(TestHelpers.Height);
    }
}