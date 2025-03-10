using Drammer.Common.Images.Transformation;

namespace Drammer.Common.Images.Tests.Transformation;

public sealed class FileNameExtensionsTests
{
    [Theory]
    [InlineData("image.jpg", "image/jpeg", ".jpg")]
    [InlineData("IMAGE.JPEG", "image/jpeg", ".jpeg")]
    [InlineData("image.png", "image/png", ".png")]
    [InlineData("image.webp", "image/webp", ".webp")]
    public void GetContentType_ReturnsContentType(string fileName, string expectedContentType, string expectedExtension)
    {
        // Act
        var result = fileName.GetContentType();

        // Assert
        result.ContentType.Should().Be(expectedContentType);
        result.Extension.Should().Be(expectedExtension);
    }
}