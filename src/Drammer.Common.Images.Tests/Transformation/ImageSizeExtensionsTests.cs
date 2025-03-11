using Drammer.Common.Images.Transformation;

namespace Drammer.Common.Images.Tests.Transformation;

public sealed class ImageSizeExtensionsTests
{
    [Theory]
    [InlineData(50, 25)]
    [InlineData(100, 50)]
    [InlineData(200, 100)]
    public void Resize_WithTargetWith_ReturnsResizedResult(int height, int expectedHeight)
    {
        // Arrange
        var originalDimensions = new ImageSize(100, height);
        const int TargetWidth = 50;

        // Act
        var result = originalDimensions.Resize(TargetWidth);

        // Assert
        result.Width.Should().Be(TargetWidth);
        result.Height.Should().Be(expectedHeight);
    }

    [Fact]
    public void Resize_WithTargetHeight_ReturnsResizedResult()
    {
        // Arrange
        var originalDimensions = new ImageSize(200, 100);
        const int TargetHeight = 50;

        // Act
        var result = originalDimensions.Resize(null, TargetHeight);

        // Assert
        result.Width.Should().Be(100);
        result.Height.Should().Be(TargetHeight);
    }
}