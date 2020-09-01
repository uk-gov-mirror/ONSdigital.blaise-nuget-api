﻿using Blaise.Nuget.Api.Helpers;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using System.Collections.Generic;
using System.IO;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using StatNeth.Blaise.API.DataLink;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Interfaces;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api
{
    public class BlaiseApi : IBlaiseApi
    {
        private IDataService _dataService;
        private IParkService _parkService;
        private ISurveyService _surveyService;
        private IUserService _userService;
        private IFileService _fileService;
        private ICloudStorageService _cloudStorageService;
        private IConfigurationProvider _configurationProvider;
        private readonly IIocProvider _unityProvider;

        internal BlaiseApi(
            IDataService dataService,
            IParkService parkService,
            ISurveyService surveyService,
            IUserService userService,
            IFileService fileService,
            ICloudStorageService cloudStorageService,
            IIocProvider unityProvider,
            IConfigurationProvider configurationProvider)
        {
            _dataService = dataService;
            _parkService = parkService;
            _surveyService = surveyService;
            _userService = userService;
            _fileService = fileService;
            _cloudStorageService = cloudStorageService;
            _unityProvider = unityProvider;
            _configurationProvider = configurationProvider;
        }

        public BlaiseApi()
        {
            _configurationProvider = new ConfigurationProvider();

            _unityProvider = new UnityProvider();
            RegisterAndResolveDependencies();
        }

        private void RegisterAndResolveDependencies()
        {
            _unityProvider.RegisterDependencies();

            //resolve dependencies
            _dataService = _unityProvider.Resolve<IDataService>();
            _parkService = _unityProvider.Resolve<IParkService>();
            _surveyService = _unityProvider.Resolve<ISurveyService>();
            _userService = _unityProvider.Resolve<IUserService>();
            _fileService = _unityProvider.Resolve<IFileService>();
            _cloudStorageService = _unityProvider.Resolve<ICloudStorageService>();
            _configurationProvider = _unityProvider.Resolve<IConfigurationProvider>();
        }

        public IEnumerable<string> GetServerParkNames(ConnectionModel connectionModel)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            return _parkService.GetServerParkNames(connectionModel);
        }

        public IEnumerable<ISurvey> GetAllSurveys(ConnectionModel connectionModel)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            return _surveyService.GetAllSurveys(connectionModel);
        }

        public IEnumerable<string> GetSurveyNames(ConnectionModel connectionModel, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetSurveyNames(connectionModel, serverParkName);
        }

        public IEnumerable<ISurvey> GetSurveys(ConnectionModel connectionModel, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetSurveys(connectionModel, serverParkName);
        }

        public bool ServerParkExists(ConnectionModel connectionModel, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _parkService.ServerParkExists(connectionModel, serverParkName);
        }

        public bool SurveyExists(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.SurveyExists(connectionModel, instrumentName, serverParkName);
        }

        public Guid GetInstrumentId(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);
        }

        public IDatamodel GetDataModel(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetDataModel(connectionModel, instrumentName, serverParkName);
        }

        public SurveyType GetSurveyType(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetSurveyType(connectionModel, instrumentName, serverParkName);
        }

        public IKey GetKey(IDatamodel dataModel, string keyName)
        {
            dataModel.ThrowExceptionIfNull("dataModel");
            keyName.ThrowExceptionIfNullOrEmpty("keyName");

            return _dataService.GetKey(dataModel, keyName);
        }

        public IKey GetPrimaryKey(IDatamodel dataModel)
        {
            dataModel.ThrowExceptionIfNull("dataModel");

            return _dataService.GetPrimaryKey(dataModel);
        }

        public bool KeyExists(ConnectionModel connectionModel, IKey key, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            key.ThrowExceptionIfNull("key");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.KeyExists(connectionModel, key, instrumentName, serverParkName);
        }

        public bool CaseExists(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.CaseExists(connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public bool CaseExists(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            dataRecord.ThrowExceptionIfNull("dataRecord");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var primaryKeyValue = GetPrimaryKeyValue(dataRecord);

            return _dataService.CaseExists(connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public string GetPrimaryKeyValue(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _dataService.GetPrimaryKeyValue(dataRecord);
        }

        public void AssignPrimaryKeyValue(IKey key, string primaryKeyValue)
        {
            key.ThrowExceptionIfNull("key");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKey");

            _dataService.AssignPrimaryKeyValue(key, primaryKeyValue);
        }

        public IDataSet GetDataSet(string filePath)
        {
            filePath.ThrowExceptionIfNullOrEmpty("filePath");

            return _dataService.GetDataSet(filePath);
        }

        public IDataRecord GetDataRecord(IDatamodel dataModel)
        {
            dataModel.ThrowExceptionIfNull("dataModel");

            return _dataService.GetDataRecord(dataModel);
        }

        public IDataSet GetDataSet(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetDataSet(connectionModel, instrumentName, serverParkName);
        }

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, IKey key, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            key.ThrowExceptionIfNull("key");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetDataRecord(connectionModel, key, instrumentName, serverParkName);
        }

        public IDataRecord GetDataRecord(IKey key, string filePath)
        {
            key.ThrowExceptionIfNull("key");
            filePath.ThrowExceptionIfNullOrEmpty("filePath");

            return _dataService.GetDataRecord(key, filePath);
        }

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetDataRecord(connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            dataRecord.ThrowExceptionIfNull("dataRecord");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public void WriteDataRecord(IDataRecord dataRecord, string filePath)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            filePath.ThrowExceptionIfNullOrEmpty("filePath");

            _dataService.WriteDataRecord(dataRecord, filePath);
        }

        public void CreateNewDataRecord(ConnectionModel connectionModel, string primaryKeyValue, Dictionary<string, string> fieldData, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.CreateNewDataRecord(connectionModel, primaryKeyValue, fieldData, instrumentName, serverParkName);
        }

        public void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, Dictionary<string, string> fieldData, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            dataRecord.ThrowExceptionIfNull("dataRecord");
            fieldData.ThrowExceptionIfNull("fieldData");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.UpdateDataRecord(connectionModel, dataRecord, fieldData, instrumentName, serverParkName);
        }

        public bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName, FieldNameType fieldNameType)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.FieldExists(connectionModel, instrumentName, serverParkName, fieldNameType);
        }

        public bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _dataService.FieldExists(dataRecord, fieldNameType);
        }

        public bool CaseHasBeenCompleted(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _dataService.CaseHasBeenCompleted(dataRecord);
        }

        public void MarkCaseAsComplete(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            dataRecord.ThrowExceptionIfNull("dataRecord");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.MarkCaseAsComplete(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public bool CaseHasBeenProcessed(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _dataService.CaseHasBeenProcessed(dataRecord);
        }

        public IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _dataService.GetFieldValue(dataRecord, fieldNameType);
        }

        public IDataValue GetFieldValue(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName,
            string serverParkName, FieldNameType fieldNameType)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var dataRecord = _dataService.GetDataRecord(connectionModel, primaryKeyValue, instrumentName, serverParkName);

            return GetFieldValue(dataRecord, fieldNameType);
        }

        public void MarkCaseAsProcessed(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            dataRecord.ThrowExceptionIfNull("dataRecord");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.MarkCaseAsProcessed(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public void AddUser(ConnectionModel connectionModel, string userName, string password, string role, IList<string> serverParkNames)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            userName.ThrowExceptionIfNullOrEmpty("userName");
            password.ThrowExceptionIfNullOrEmpty("password");
            role.ThrowExceptionIfNullOrEmpty("role");

            _userService.AddUser(connectionModel, userName, password, role, serverParkNames);
        }

        public void EditUser(ConnectionModel connectionModel, string userName, string role, IList<string> serverParkNames)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            userName.ThrowExceptionIfNullOrEmpty("userName");
            role.ThrowExceptionIfNullOrEmpty("role");

            _userService.EditUser(connectionModel, userName, role, serverParkNames);
        }

        public void ChangePassword(ConnectionModel connectionModel, string userName, string password)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            userName.ThrowExceptionIfNullOrEmpty("userName");
            password.ThrowExceptionIfNullOrEmpty("password");

            _userService.ChangePassword(connectionModel, userName, password);
        }

        public bool UserExists(ConnectionModel connectionModel, string userName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            userName.ThrowExceptionIfNullOrEmpty("userName");

            return _userService.UserExists(connectionModel, userName);
        }

        public void RemoveUser(ConnectionModel connectionModel, string userName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            userName.ThrowExceptionIfNullOrEmpty("userName");

            _userService.RemoveUser(connectionModel, userName);
        }

        public void CopyCase(ConnectionModel sourceConnectionModel, string primaryKeyValue, string sourceInstrumentName, string sourceServerParkName,
            ConnectionModel destinationConnectionModel, string destinationInstrumentName, string destinationServerParkName)
        {
            sourceConnectionModel.ThrowExceptionIfNull("sourceConnectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            sourceInstrumentName.ThrowExceptionIfNullOrEmpty("sourceInstrumentName");
            sourceServerParkName.ThrowExceptionIfNullOrEmpty("sourceServerParkName");
            destinationConnectionModel.ThrowExceptionIfNull("destinationConnectionModel");
            destinationInstrumentName.ThrowExceptionIfNullOrEmpty("destinationInstrumentName");
            destinationServerParkName.ThrowExceptionIfNullOrEmpty("destinationServerParkName");

            var caseRecord = _dataService.GetDataRecord(sourceConnectionModel, primaryKeyValue, sourceInstrumentName, sourceServerParkName);

            _dataService.WriteDataRecord(destinationConnectionModel, caseRecord, destinationInstrumentName, destinationServerParkName);
        }

        public void CopyCase(ConnectionModel sourceConnectionModel, string primaryKeyValue, string sourceInstrumentName, string sourceServerParkName,
            string destinationFilePath, string destinationInstrumentName)
        {
            sourceConnectionModel.ThrowExceptionIfNull("sourceConnectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            sourceInstrumentName.ThrowExceptionIfNullOrEmpty("sourceInstrumentName");
            sourceServerParkName.ThrowExceptionIfNullOrEmpty("sourceServerParkName");
            destinationFilePath.ThrowExceptionIfNullOrEmpty("destinationFilePath");
            destinationInstrumentName.ThrowExceptionIfNullOrEmpty("destinationInstrumentName");

            if (!_fileService.DatabaseFileExists(destinationFilePath, destinationInstrumentName))
            {
                var metaFileName = _surveyService.GetMetaFileName(sourceConnectionModel, sourceInstrumentName, sourceServerParkName);
                _fileService.CreateDatabaseFile(metaFileName, destinationFilePath, destinationInstrumentName);
            }

            var caseRecord = _dataService.GetDataRecord(sourceConnectionModel, primaryKeyValue, sourceInstrumentName, sourceServerParkName);
            var fullFilePath = _fileService.GetDatabaseFileName(destinationFilePath, destinationInstrumentName);

            _dataService.WriteDataRecord(caseRecord, fullFilePath);
        }

        public void MoveCase(ConnectionModel sourceConnectionModel, string primaryKeyValue, string sourceInstrumentName, string sourceServerParkName,
            ConnectionModel destinationConnectionModel, string destinationInstrumentName, string destinationServerParkName)
        {
            sourceConnectionModel.ThrowExceptionIfNull("sourceConnectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            sourceInstrumentName.ThrowExceptionIfNullOrEmpty("sourceInstrumentName");
            sourceServerParkName.ThrowExceptionIfNullOrEmpty("sourceServerParkName");
            destinationConnectionModel.ThrowExceptionIfNull("destinationConnectionModel");
            destinationInstrumentName.ThrowExceptionIfNullOrEmpty("destinationInstrumentName");
            destinationServerParkName.ThrowExceptionIfNullOrEmpty("destinationServerParkName");

            var caseRecord = _dataService.GetDataRecord(sourceConnectionModel, primaryKeyValue, sourceInstrumentName, sourceServerParkName);

            _dataService.WriteDataRecord(destinationConnectionModel, caseRecord, destinationInstrumentName, destinationServerParkName);

            RemoveCase(sourceConnectionModel, primaryKeyValue, sourceInstrumentName, sourceServerParkName);
        }

        public void MoveCase(ConnectionModel sourceConnectionModel, string primaryKeyValue, string sourceInstrumentName, string sourceServerParkName,
            string destinationFilePath, string destinationInstrumentName)
        {
            sourceConnectionModel.ThrowExceptionIfNull("sourceConnectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            sourceInstrumentName.ThrowExceptionIfNullOrEmpty("sourceInstrumentName");
            sourceServerParkName.ThrowExceptionIfNullOrEmpty("sourceServerParkName");
            destinationFilePath.ThrowExceptionIfNullOrEmpty("destinationFilePath");
            destinationInstrumentName.ThrowExceptionIfNullOrEmpty("destinationInstrumentName");


            CopyCase(sourceConnectionModel, primaryKeyValue, sourceInstrumentName, sourceServerParkName, destinationFilePath,
                destinationInstrumentName);

            RemoveCase(sourceConnectionModel, primaryKeyValue, sourceInstrumentName, sourceServerParkName);
        }

        public void RemoveCase(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.RemoveDataRecord(connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public void BackupSurveyToFile(ConnectionModel connectionModel, string serverParkName, string instrumentName, string destinationFilePath)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            destinationFilePath.ThrowExceptionIfNullOrEmpty("destinationFilePath");

            var dataFilePath = _surveyService.GetDataFileName(connectionModel, instrumentName, serverParkName);
            var metaFilePath = _surveyService.GetMetaFileName(connectionModel, instrumentName, serverParkName);
            var backupFilePath = Path.Combine(destinationFilePath, instrumentName);

            _fileService.BackupDatabaseFile(dataFilePath, metaFilePath, backupFilePath);
        }

        public void BackupSurveyToBucket(ConnectionModel connectionModel, string serverParkName, string instrumentName,
            string bucketName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            bucketName.ThrowExceptionIfNullOrEmpty("bucketName");

            var dataFilePath = _surveyService.GetDataFileName(connectionModel, instrumentName, serverParkName);
            var metaFilePath = _surveyService.GetMetaFileName(connectionModel, instrumentName, serverParkName);
            var databaseSourceFilePath = _fileService.GetDatabaseSourceFile(metaFilePath);
            
            _cloudStorageService.UploadToBucket(dataFilePath, bucketName, serverParkName);
            _cloudStorageService.UploadToBucket(metaFilePath, bucketName, serverParkName);
            _cloudStorageService.UploadToBucket(databaseSourceFilePath, bucketName, serverParkName);
        }

        public ConnectionModel GetDefaultConnectionModel()
        {
            return _configurationProvider.GetConnectionModel();
        }
    }
}
