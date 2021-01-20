using System;
using System.IO;
using System.IO.Compression;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;

namespace Blaise.Nuget.Api.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IBlaiseConfigurationProvider _configurationProvider;
        private readonly IDataInterfaceService _dataInterfaceService;

        private const string DatabaseFileNameExt = "bdix";
        private const string DatabaseSourceExt = "bdbx";
        private const string DatabaseModelExt = "bmix";

        public FileService(
            IBlaiseConfigurationProvider configurationProvider, 
            IDataInterfaceService dataInterfaceService)
        {
            _configurationProvider = configurationProvider;
            _dataInterfaceService = dataInterfaceService;
        }

        public bool DatabaseFileExists(string filePath, string instrumentName)
        {
            return File.Exists(GetFullFilePath(filePath, instrumentName, DatabaseSourceExt));
        }

        public void DeleteDatabaseFile(string filePath, string instrumentName)
        {
            File.Delete(GetFullFilePath(filePath, instrumentName, DatabaseSourceExt));
        }

        public string GetDatabaseFilePath(string outputPath, string instrumentName)
        {
            Directory.CreateDirectory(outputPath);

            return GetFullFilePath(outputPath, instrumentName, DatabaseSourceExt);
        }

        public void UpdateInstrumentPackageWithSqlConnection(string instrumentName, string instrumentFile)
        {
            var instrumentPath = ExtractInstrumentPackage(instrumentName, instrumentFile);
            var databaseConnectionString = _configurationProvider.DatabaseConnectionString;
            var fileName = GetFullFilePath(instrumentPath, instrumentName, DatabaseFileNameExt);
            var dataModelFileName = GetFullFilePath(instrumentPath, instrumentName, DatabaseModelExt);

            _dataInterfaceService.CreateSqlDataInterface(databaseConnectionString, fileName,
                dataModelFileName);

            CreateInstrumentPackage(instrumentPath, instrumentFile);
        }

        private static string ExtractInstrumentPackage(string instrumentName, string instrumentFile)
        {
            var instrumentPath = $"{Path.GetDirectoryName(instrumentFile)}\\{instrumentName}\\{Guid.NewGuid()}";

            if (Directory.Exists(instrumentPath))
            {
                Directory.Delete(instrumentPath, true);
            }

            ZipFile.ExtractToDirectory(instrumentFile, instrumentPath);
            File.Delete(instrumentFile);

            return instrumentPath;
        }

        private static void CreateInstrumentPackage(string instrumentPath, string instrumentFile)
        {
            ZipFile.CreateFromDirectory(instrumentPath, instrumentFile);
            Directory.Delete(instrumentPath, true);
        }

        private static string GetFullFilePath(string filePath, string instrumentName, string extension)
        {
            return Path.Combine(filePath, $"{instrumentName}.{extension}");
        }
    }
}
