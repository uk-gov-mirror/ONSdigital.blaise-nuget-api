namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IFileService
    {
        bool DatabaseFileExists(string databaseFile, string instrumentName);

        void DeleteDatabaseFile(string databaseFile, string instrumentName);

        string CreateDatabaseFile(string metaFileName, string outputPath, string instrumentName);

        void UpdateInstrumentPackageWithSqlConnection(string instrumentName,
            string instrumentFile);
    }
}