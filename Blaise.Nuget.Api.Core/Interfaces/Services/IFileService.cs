namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IFileService
    {
        string GetDatabaseFileName(string filePath, string instrumentName);
        bool DatabaseFileExists(string filePath, string instrumentName);
        void CreateDatabaseFile(string metaFileName, string filePath, string instrumentName);
    }
}