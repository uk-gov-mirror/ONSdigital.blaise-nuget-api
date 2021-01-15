using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Exceptions;
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
        private const string LibraryFileExt = "blix";

        public FileService(
            IBlaiseConfigurationProvider configurationProvider, 
            IDataInterfaceService dataInterfaceService)
        {
            _configurationProvider = configurationProvider;
            _dataInterfaceService = dataInterfaceService;
        }

        public bool DatabaseFileExists(string databaseFile, string instrumentName)
        {
            return File.Exists(Path.Combine(databaseFile, $"{instrumentName}.{DatabaseSourceExt}"));
        }

        public void DeleteDatabaseFile(string databaseFile, string instrumentName)
        {
            File.Delete(Path.Combine(databaseFile, $"{instrumentName}.{DatabaseSourceExt}"));
        }

        public string CreateDatabaseFile(string metaFileName, string outputPath, string instrumentName)
        {
            CopyLibraryFiles(_configurationProvider.LibraryDirectory, outputPath);

            Directory.CreateDirectory(outputPath);
            var fileNameToCopy = GetFullFilePath(outputPath, instrumentName, DatabaseModelExt);
            File.Copy(metaFileName, fileNameToCopy, true);

            var databaseSourceFileName = GetFullFilePath(outputPath, instrumentName, DatabaseSourceExt);
            var fileName = GetFullFilePath(outputPath, instrumentName, DatabaseFileNameExt);
            var dataModelFileName = GetFullFilePath(outputPath, instrumentName, DatabaseModelExt);
            
            var dataInterface = _dataInterfaceService.CreateFileDataInterface(databaseSourceFileName, fileName,
                dataModelFileName);

            //yuck!
            OverwriteDatabaseFileToAvoidFixedDirectoryIssue(metaFileName, outputPath, instrumentName);

            return dataInterface.FileName;
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

        //fml
        private static void OverwriteDatabaseFileToAvoidFixedDirectoryIssue(string metaFileName, string filePath, string instrumentName)
        {
            var sourcePath = Path.GetDirectoryName(metaFileName);

            if (string.IsNullOrWhiteSpace(sourcePath))
            {
                throw new ArgumentException($"Could not find the path of the file '{metaFileName}'");
            }

            var sourceDatabaseFile = GetFullFilePath(sourcePath, instrumentName, DatabaseFileNameExt);
            var destDatabaseFile = GetFullFilePath(filePath, instrumentName, DatabaseFileNameExt);

            File.Copy(sourceDatabaseFile, destDatabaseFile, true);
        }

        private static string GetFullFilePath(string filePath, string instrumentName, string extension)
        {
            return Path.Combine(filePath, $"{instrumentName}.{extension}");
        }

        private static void CopyLibraryFiles(string libraryPath, string destinationPath)
        {
            if (!Directory.Exists(libraryPath))
            {
                throw new ArgumentException($"The library path '{libraryPath}' provided in the config does not exist");
            }

            var libraryFiles = Directory.GetFiles(libraryPath, $"*.{LibraryFileExt}", SearchOption.AllDirectories);

            if (!libraryFiles.Any())
            {
                throw new DataNotFoundException($"No Library files with extension '{LibraryFileExt}' found in any directory under '{libraryPath}'");
            }

            Directory.CreateDirectory(destinationPath);

            foreach (var libraryFile in libraryFiles)
            {
                if (FileIsLocked(libraryFile))
                {
                    throw new AccessViolationException($"Could not copy file {libraryFile} as it is locked");
                }

                var destinationFile = Path.Combine(destinationPath, Path.GetFileName(libraryFile));

                if (File.Exists(destinationFile) && FileIsLocked(destinationFile))
                {
                    throw new AccessViolationException($"Could not write to file {destinationFile} as it is locked");
                }

                File.Copy(libraryFile, destinationFile, true);
            }
        }

        private static bool FileIsLocked(string filePath)
        {
            try
            {
                var file = new FileInfo(filePath);

                using (var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }
    }
}
