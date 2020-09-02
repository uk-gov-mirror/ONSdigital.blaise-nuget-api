using System.IO;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;

namespace Blaise.Nuget.Api.Core.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly IStorageClientProvider _storageClient;

        public CloudStorageService(IStorageClientProvider storageClient)
        {
            _storageClient = storageClient;
        }

        public void UploadToBucket(string filePath, string bucketName, string folderName)
        {
            var fileName = Path.GetFileName(filePath);
            var bucket = _storageClient.GetStorageClient();

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (var streamWriter = new StreamWriter(fileStream))
            {
                var objectName = folderName == null ? fileName : $"{folderName}/{fileName}";
                bucket.UploadObject(bucketName, objectName, null, streamWriter.BaseStream);
            }
        }
    }
}
