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
        var result = await service.UploadImageAsync(mockBlobContainerClient.Object, imageData!, "image.webp", "image/webp");

        // Assert
        result.Should().BeFalse();
        mockBlobClient.VerifyAll();
    }

    [Fact]
    public async Task DownloadImageAsync_ReturnsByteArray()
    {
        // Arrange
        var mockBlobContainerClient = new Mock<BlobContainerClient>();
        var mockBlobClient = new Mock<BlobClient>();
        var service = new StorageService();
        var imageData = TestHelpers.ReadResource(TestHelpers.ResourceName);

        mockBlobContainerClient
            .Setup(c => c.GetBlobClient(It.IsAny<string>()))
            .Returns(mockBlobClient.Object);

        mockBlobClient.Setup(
                x => x.ExistsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(true, Mock.Of<Response>()));

        mockBlobClient.Setup(
            x => x.DownloadToAsync(
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>())).Callback<Stream, CancellationToken>(
            (destination, cancellationToken) =>
            {
                destination.Write(imageData!, 0, imageData!.Length);
            }).ReturnsAsync(Mock.Of<Response>());

        // Act
        var result = await service.DownloadImageAsync(mockBlobContainerClient.Object, "image.webp");

        // Assert
        result.Should().NotBeNull();
    }
}