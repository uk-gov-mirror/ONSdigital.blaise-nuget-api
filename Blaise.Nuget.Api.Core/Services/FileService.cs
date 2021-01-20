using System;
using System.IO;
using System.IO.Compression;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IBlaiseConfigurationProvider _configurationProvider;
        private readonly IDataInterfaceService _dataInterfaceService;
        private readonly ICaseService _caseService;

        private const string DatabaseFileNameExt = "bdix";
        private const string DatabaseSourceExt = "bdbx";
        private const string DatabaseModelExt = "bmix";

        public FileService(
            IBlaiseConfigurationProvider configurationProvider, 
            IDataInterfaceService dataInterfaceService, 
            ICaseService caseService)
        {
            _configurationProvider = configurationProvider;
            _dataInterfaceService = dataInterfaceService;
            _caseService = caseService;
        }

        public void UpdateInstrumentFileWithData(ConnectionModel connectionModel, string instrumentFile, string instrumentName, string serverParkName)
        {
            var instrumentPath = ExtractInstrumentPackage(instrumentName, instrumentFile);
            var databaseFile = GetFullFilePath(instrumentPath, instrumentName, DatabaseSourceExt);

            if (File.Exists(databaseFile))
            {
                File.Delete(databaseFile);
            }

            var cases = _caseService.GetDataSet(connectionModel, instrumentName, serverParkName);

            while (!cases.EndOfSet)
            {
                _caseService.WriteDataRecord((IDataRecord2)cases.ActiveRecord, databaseFile);

                cases.MoveNext();
            }

            CreateInstrumentPackage(instrumentPath, instrumentFile);
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
            var instrumentPath = GetTemporaryPath(instrumentName, instrumentFile);

            if (Directory.Exists(instrumentPath))
            {
                Directory.Delete(instrumentPath, true);
            }

            ZipFile.ExtractToDirectory(instrumentFile, instrumentPath);
            File.Delete(instrumentFile);

            return instrumentPath;
        }

        private static string GetTemporaryPath(string instrumentName, string instrumentFile)
        {
            return $"{Path.GetDirectoryName(instrumentFile)}\\{instrumentName}\\{Guid.NewGuid()}";
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
