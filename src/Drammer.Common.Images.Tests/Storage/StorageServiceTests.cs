using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Drammer.Common.Images.Storage;

namespace Drammer.Common.Images.Tests.Storage;

public sealed class StorageServiceTests
{
    [Fact]
    public async Task GetAllBlobsAsync_ReturnsList()
    {
        // Arrange
        var mockBlobContainerClient = new Mock<BlobContainerClient>();
        var blobItems = new List<BlobItem>
        {
            BlobsModelFactory.BlobItem("blob1.txt"),
            BlobsModelFactory.BlobItem("blob2.txt")
        };

        var page = Page<BlobItem>.FromValues(blobItems, null, Mock.Of<Response>());
        var pageableBlobList = AsyncPageable<BlobItem>.FromPages([page]);

        mockBlobContainerClient
            .Setup(c => c.GetBlobsAsync(
                It.IsAny<BlobTraits>(),
                It.IsAny<BlobStates>(),
                It.IsAny<string?>(),
                It.IsAny<CancellationToken>()))
            .Returns(pageableBlobList);

        var service = new StorageService();

        // Act
        var result = await service.GetAllBlobsAsync(mockBlobContainerClient.Object);

        // Assert
        result.Should().NotBeNull();
        result.Count.Should().Be(2);
        result.Should().Contain(b => b.Name == "blob1.txt");
        result.Should().Contain(b => b.Name == "blob2.txt");
    }

    [Fact]
    public async Task UploadImageAsync_UploadsImage()
    {
        // Arrange
        var mockBlobContainerClient = new Mock<BlobContainerClient>();
        var mockBlobClient = new Mock<BlobClient>();
        var service = new StorageService();
        var imageData = TestHelpers.ReadResource(TestHelpers.ResourceName);
        var response = Response.FromValue(Mock.Of<BlobContentInfo>(), Mock.Of<Response>());

        mockBlobContainerClient
            .Setup(c => c.GetBlobClient(It.IsAny<string>()))
            .Returns(mockBlobClient.Object);

        mockBlobClient.Setup(
                x => x.UploadAsync(
                    It.IsAny<BinaryData>(),
                    It.IsAny<BlobUploadOptions>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        await service.UploadImageAsync(mockBlobContainerClient.Object, imageData!, "image.webp", "image/webp");

        // Assert
        mockBlobClient.VerifyAll();
    }
}