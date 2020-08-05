namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ICloudStorageService
    {
        void UploadToBucket(string filePath, string bucketName);
    }
}