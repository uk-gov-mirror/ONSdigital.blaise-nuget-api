using System.Collections.Generic;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IFileService
    {
        IEnumerable<string> GetFiles(string filePath);

        bool DatabaseFileExists(string filePath, string instrumentName);

        void DeleteDatabaseFile(string filePath, string instrumentName);

        string CreateDatabaseFile(string metaFileName, string filePath, string instrumentName);

        void UploadFilesToBucket(string filePath, string bucketName, string folderName);
    }
}