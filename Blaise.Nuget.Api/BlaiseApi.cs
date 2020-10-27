using Blaise.Nuget.Api.Helpers;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
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
        private IConfigurationProvider _configurationProvider;

        private readonly IIocProvider _unityProvider;

        internal BlaiseApi(
            IDataService dataService,
            IParkService parkService,
            ISurveyService surveyService,
            IUserService userService,
            IFileService fileService,
            IConfigurationProvider configurationProvider)
        {
            _dataService = dataService;
            _parkService = parkService;
            _surveyService = surveyService;
            _userService = userService;
            _fileService = fileService;
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
            _configurationProvider = _unityProvider.Resolve<IConfigurationProvider>();
        }

        public IServerPark GetServerPark(ConnectionModel connectionModel, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _parkService.GetServerPark(connectionModel, serverParkName);
        }

        public IEnumerable<IServerPark> GetServerParks(ConnectionModel connectionModel)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");

            return _parkService.GetServerParks(connectionModel);
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

        public void CreateDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            DateTime dayBatchDate)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _surveyService.CreateDayBatch(connectionModel, instrumentName, serverParkName, dayBatchDate);
        }

        public SurveyType GetSurveyType(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetSurveyType(connectionModel, instrumentName, serverParkName);
        }

        public bool CaseExists(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, 
            string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.CaseExists(connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public string GetPrimaryKeyValue(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _dataService.GetPrimaryKeyValue(dataRecord);
        }

        public IDataSet GetDataSet(string filePath)
        {
            filePath.ThrowExceptionIfNullOrEmpty("filePath");

            return _dataService.GetDataSet(filePath);
        }

        public IDataSet GetDataSet(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetDataSet(connectionModel, instrumentName, serverParkName);
        }

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, 
            string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetDataRecord(connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public void CreateNewDataRecord(ConnectionModel connectionModel, string primaryKeyValue, Dictionary<string, string> fieldData, 
            string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.CreateNewDataRecord(connectionModel, primaryKeyValue, fieldData, instrumentName, serverParkName);
        }

        public void CreateNewDataRecord(string filePath, string primaryKeyValue, Dictionary<string, string> fieldData)
        {
            filePath.ThrowExceptionIfNullOrEmpty("filePath");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");

            _dataService.CreateNewDataRecord(filePath, primaryKeyValue, fieldData);
        }

        public void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, Dictionary<string, string> fieldData,
            string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            dataRecord.ThrowExceptionIfNull("dataRecord");
            fieldData.ThrowExceptionIfNull("fieldData");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.UpdateDataRecord(connectionModel, dataRecord, fieldData, instrumentName, serverParkName);
        }

        public bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName, 
            FieldNameType fieldNameType)
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

        public void AddUser(ConnectionModel connectionModel, string userName, string password, 
            string role, IList<string> serverParkNames, string defaultServerPark)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            userName.ThrowExceptionIfNullOrEmpty("userName");
            password.ThrowExceptionIfNullOrEmpty("password");
            role.ThrowExceptionIfNullOrEmpty("role");
            defaultServerPark.ThrowExceptionIfNullOrEmpty("DefaultServerPark");

            _userService.AddUser(connectionModel, userName, password, role, serverParkNames, defaultServerPark);
        }

        public void EditUser(ConnectionModel connectionModel, string userName, string role, 
            IList<string> serverParkNames)
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

        public IUser GetUser(ConnectionModel connectionModel, string userName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            userName.ThrowExceptionIfNullOrEmpty("userName");

            return _userService.GetUser(connectionModel, userName);
        }

        public void RemoveCase(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, 
            string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.RemoveDataRecord(connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public string BackupSurveyToFile(ConnectionModel connectionModel, string serverParkName, string instrumentName, 
            string destinationFilePath)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            destinationFilePath.ThrowExceptionIfNullOrEmpty("destinationFilePath");

            if (_fileService.DatabaseFileExists(destinationFilePath, instrumentName))
            {
                _fileService.DeleteDatabaseFile(destinationFilePath, instrumentName);
            }

            var metaFileName = _surveyService.GetMetaFileName(connectionModel, instrumentName, serverParkName);
            var databaseFile =_fileService.CreateDatabaseFile(metaFileName, destinationFilePath, instrumentName);

            var cases = _dataService.GetDataSet(connectionModel, instrumentName, serverParkName);

            while (!cases.EndOfSet)
            {
                _dataService.WriteDataRecord((IDataRecord2)cases.ActiveRecord, databaseFile);

                cases.MoveNext();
            }

            return databaseFile;
        }

        public void BackupFilesToBucket(string filePath, string bucketName, string folderName = null)
        {
            filePath.ThrowExceptionIfNullOrEmpty("filePath");
            bucketName.ThrowExceptionIfNullOrEmpty("bucketName");

            foreach (var file in _fileService.GetFiles(filePath))
            {
                _fileService.UploadFilesToBucket(file, bucketName, folderName);
            }
        }

        public int GetNumberOfCases(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            connectionModel.ThrowExceptionIfNull("connectionModel");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetNumberOfCases(connectionModel, instrumentName, serverParkName);
        }

        public int GetNumberOfCases(string filePath)
        {
            filePath.ThrowExceptionIfNullOrEmpty("filePath");

            return _dataService.GetNumberOfCases(filePath);
        }

        public ConnectionModel GetDefaultConnectionModel()
        {
            return _configurationProvider.GetConnectionModel();
        }
    }
}
