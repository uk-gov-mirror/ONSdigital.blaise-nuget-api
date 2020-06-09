﻿
using System;
using System.IO;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Services
{
    public class FileService : IFileService
    {
        private readonly IConfigurationProvider _configurationProvider;

        private readonly string DatabaseFileNameExt = "bdix";
        private readonly string DatabaseSourceExt = "bdbx";
        private readonly string DatabaseModelExt = "bmix";
        private readonly string LibraryFileExt = "blix";

        public FileService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public string GetDatabaseFileName(string filePath, string instrumentName)
        {
            return GetFullFilePath(filePath, instrumentName, DatabaseFileNameExt);
        }

        public bool DatabaseFileExists(string filePath, string instrumentName)
        {
            return File.Exists(Path.Combine(filePath, $"{instrumentName}.{DatabaseSourceExt}"));
        }

        public void CreateDatabaseFile(string metaFileName, string filePath, string instrumentName)
        {
            CopyLibraryFiles(_configurationProvider.LibraryDirectory, filePath);

            Directory.CreateDirectory(filePath);
            var fileNameToCopy = GetFullFilePath(filePath, instrumentName, DatabaseModelExt);
            File.Copy(metaFileName, fileNameToCopy, true);

            var di = DataInterfaceManager.GetDataInterface();
            di.ConnectionInfo.DataSourceType = DataSourceType.Blaise;
            di.ConnectionInfo.DataProviderType = DataProviderType.BlaiseDataProviderForDotNET;
            di.DataPartitionType = DataPartitionType.Stream;
            var csb = DataInterfaceManager.GetBlaiseConnectionStringBuilder();
            csb.DataSource = GetFullFilePath(filePath, instrumentName, DatabaseSourceExt);
            di.ConnectionInfo.SetConnectionString(csb.ConnectionString);
            di.DatamodelFileName = GetFullFilePath(filePath, instrumentName, DatabaseModelExt);
            di.FileName = GetFullFilePath(filePath, instrumentName, DatabaseFileNameExt);
            di.CreateTableDefinitions();
            di.CreateDatabaseObjects(null, true);
            di.SaveToFile(true);
        }

        private static string GetFullFilePath(string filePath, string instrumentName, string extension)
        {
            return Path.Combine(filePath, $"{instrumentName}.{extension}");
        }

        private void CopyLibraryFiles(string libraryPath, string destinationPath)
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

            foreach (var libraryFile in libraryFiles)
            {
                if (FileIsLocked(libraryFile))
                {
                    return;
                }

                var destinationFile = Path.Combine(destinationPath, Path.GetFileName(libraryFile));

                if (File.Exists(destinationFile) && FileIsLocked(destinationFile))
                {
                    return;
                }

                File.Copy(libraryFile, destinationFile, true);
            }
        }

        private static bool FileIsLocked(string filePath)
        {
            try
            {
                var file = new FileInfo(filePath);

                using (FileStream stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None))
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
