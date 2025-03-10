namespace Drammer.Common.Images.Transformation;

public interface IWaterMarkService
{
    Task<byte[]> AddWaterMarkAsync(
        byte[] image,
        byte[] waterMark,
        CancellationToken cancellationToken = default);
}