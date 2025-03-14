﻿using Azure;
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
        var imageData = TestHelpers.ReadResource(TestHelpers.ResourceName1);
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
        result.Should().NotBeNull();
        mockBlobClient.VerifyAll();
    }

    [Fact]
    public async Task DownloadImageAsync_ReturnsObject()
    {
        // Arrange
        var mockBlobContainerClient = new Mock<BlobContainerClient>();
        var mockBlobClient = new Mock<BlobClient>();
        var service = new StorageService();
        var imageData = TestHelpers.ReadResource(TestHelpers.ResourceName1);

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

        mockBlobClient.Setup(x => x.GetPropertiesAsync(It.IsAny<BlobRequestConditions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(Mock.Of<BlobProperties>(), Mock.Of<Response>()));

        // Act
        var result = await service.DownloadFileAsync(mockBlobContainerClient.Object, "image.webp");

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Data!.Length.Should().Be(imageData!.Length);
        result.FileName.Should().Be("image.webp");
    }

    [Fact]
    public async Task DeleteImageAsync_ReturnsTrue()
    {
        // Arrange
        var mockBlobContainerClient = new Mock<BlobContainerClient>();
        var mockBlobClient = new Mock<BlobClient>();
        var service = new StorageService();

        mockBlobContainerClient
            .Setup(c => c.GetBlobClient(It.IsAny<string>()))
            .Returns(mockBlobClient.Object);

        mockBlobClient.Setup(x => x.DeleteIfExistsAsync(It.IsAny<DeleteSnapshotsOption>(), It.IsAny<BlobRequestConditions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(true, Mock.Of<Response>()));

        // Act
        var result = await service.DeleteFileAsync(mockBlobContainerClient.Object, "image.webp");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteImageAsync_FromUri_ReturnsTrue()
    {
        // Arrange
        var mockBlobContainerClient = new Mock<BlobContainerClient>();
        var mockBlobClient = new Mock<BlobClient>();
        var service = new StorageService();
        var uri = new Uri("https://localhost.com/container/image.webp");

        mockBlobContainerClient
            .Setup(c => c.GetBlobClient(It.IsAny<string>()))
            .Returns(mockBlobClient.Object);

        mockBlobClient.Setup(x => x.DeleteIfExistsAsync(It.IsAny<DeleteSnapshotsOption>(), It.IsAny<BlobRequestConditions>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Response.FromValue(true, Mock.Of<Response>()));

        // Act
        var result = await service.DeleteFileAsync(mockBlobContainerClient.Object, uri);

        // Assert
        result.Should().BeTrue();
    }
}