using Blaise.Nuget.Api.Helpers;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using StatNeth.Blaise.API.DataLink;
using Blaise.Nuget.Api.Core.Interfaces.Services;
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
        private readonly IUnityProvider _unityProvider;

        internal BlaiseApi(
            IDataService dataService,
            IParkService parkService, 
            ISurveyService surveyService, 
            IUserService userService, 
            IFileService fileService,
            IUnityProvider unityProvider,
            IConfigurationProvider configurationProvider)
        {
            _dataService = dataService;
            _parkService = parkService;
            _surveyService = surveyService;
            _userService = userService;
            _fileService = fileService;
            _unityProvider = unityProvider;
            _configurationProvider = configurationProvider;
        }

        public BlaiseApi()
        {
            _configurationProvider = new ConfigurationProvider();
            var connectionModel = _configurationProvider.GetConnectionModel();

            _unityProvider = new UnityProvider();
            RegisterAndResolveDependencies(connectionModel);
        }

        private void RegisterAndResolveDependencies(ConnectionModel connectionModel)
        {
            _unityProvider.RegisterDependencies(connectionModel);

            //resolve dependencies
            _dataService = _unityProvider.Resolve<IDataService>();
            _parkService = _unityProvider.Resolve<IParkService>();
            _surveyService = _unityProvider.Resolve<ISurveyService>();
            _userService = _unityProvider.Resolve<IUserService>();
            _fileService = _unityProvider.Resolve<IFileService>();
            _configurationProvider = _unityProvider.Resolve<IConfigurationProvider>();
        }

        public void UseConnection(ConnectionModel connectionModel)
        {
            RegisterAndResolveDependencies(connectionModel);
        }

        public IEnumerable<string> GetServerParkNames()
        {
            return _parkService.GetServerParkNames();
        }

        public IEnumerable<ISurvey> GetAllSurveys()
        {
            return _surveyService.GetAllSurveys();
        }

        public IEnumerable<string> GetSurveyNames(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetSurveyNames(serverParkName);
        }

        public IEnumerable<ISurvey> GetSurveys(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetSurveys(serverParkName);
        }

        public bool ServerParkExists(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _parkService.ServerParkExists(serverParkName);
        }

        public Guid GetInstrumentId(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetInstrumentId(instrumentName, serverParkName);
        }

        public IDatamodel GetDataModel(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetDataModel(instrumentName, serverParkName);
        }

        public SurveyType GetSurveyType(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetSurveyType(instrumentName, serverParkName);
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

        public bool KeyExists(IKey key, string instrumentName, string serverParkName)
        {
            key.ThrowExceptionIfNull("key");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.KeyExists(key, instrumentName, serverParkName);
        }

        public bool CaseExists(string primaryKeyValue, string instrumentName, string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.CaseExists(primaryKeyValue, instrumentName, serverParkName);
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

        public IDataSet GetDataSet(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetDataSet(instrumentName, serverParkName);
        }

        public IDataRecord GetDataRecord(IKey key, string instrumentName, string serverParkName)
        {
            key.ThrowExceptionIfNull("key");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.GetDataRecord(key, instrumentName, serverParkName);
        }

        public IDataRecord GetDataRecord(IKey key, string filePath)
        {
            key.ThrowExceptionIfNull("key");
            filePath.ThrowExceptionIfNullOrEmpty("filePath");

            return _dataService.GetDataRecord(key, filePath);
        }

        public void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.WriteDataRecord(dataRecord, instrumentName, serverParkName);
        }

        public void WriteDataRecord(IDataRecord dataRecord, string filePath)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            filePath.ThrowExceptionIfNullOrEmpty("filePath");

            _dataService.WriteDataRecord(dataRecord, filePath);
        }

        public void CreateNewDataRecord(string primaryKeyValue, Dictionary<string, string> fieldData, string instrumentName, string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.CreateNewDataRecord(primaryKeyValue, fieldData, instrumentName, serverParkName);
        }

        public void UpdateDataRecord(IDataRecord dataRecord, Dictionary<string, string> fieldData, string instrumentName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            fieldData.ThrowExceptionIfNull("fieldData");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.UpdateDataRecord(dataRecord, fieldData, instrumentName, serverParkName);
        }

        public bool CompletedFieldExists(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.CompletedFieldExists(instrumentName, serverParkName);
        }

        public bool CaseHasBeenCompleted(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _dataService.CaseHasBeenCompleted(dataRecord);
        }

        public void MarkCaseAsComplete(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.MarkCaseAsComplete(dataRecord, instrumentName, serverParkName);
        }

        public bool ProcessedFieldExists(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.ProcessedFieldExists(instrumentName, serverParkName);
        }

        public bool CaseHasBeenProcessed(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _dataService.CaseHasBeenProcessed(dataRecord);
        }

        public void MarkCaseAsProcessed(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.MarkCaseAsProcessed(dataRecord, instrumentName, serverParkName);
        }

        public void AddUser(string userName, string password, string role, IList<string> serverParkNames)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");
            password.ThrowExceptionIfNullOrEmpty("password");
            role.ThrowExceptionIfNullOrEmpty("role");
            serverParkNames.ThrowExceptionIfNullOrEmpty("serverParkNames");

            _userService.AddUser(userName, password, role, serverParkNames);
        }

        public void EditUser(string userName, string role, IList<string> serverParkNames)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");
            role.ThrowExceptionIfNullOrEmpty("role");
            serverParkNames.ThrowExceptionIfNullOrEmpty("serverParkNames");

            _userService.EditUser(userName, role, serverParkNames);
        }

        public void ChangePassword(string userName, string password)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");
            password.ThrowExceptionIfNullOrEmpty("password");

            _userService.ChangePassword(userName, password);
        }

        public bool UserExists(string userName)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");

            return _userService.UserExists(userName);
        }

        public void RemoveUser(string userName)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");

            _userService.RemoveUser(userName);
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

            UseConnection(sourceConnectionModel);
            var caseRecord = _dataService.GetDataRecord(primaryKeyValue, sourceInstrumentName, sourceServerParkName);

            UseConnection(destinationConnectionModel);
            _dataService.WriteDataRecord(caseRecord, destinationInstrumentName, destinationServerParkName);
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

            UseConnection(sourceConnectionModel);

            if (!_fileService.DatabaseFileExists(destinationFilePath, destinationInstrumentName))
            {
                var survey = _surveyService.GetSurvey(sourceInstrumentName, sourceServerParkName);
                var metaFileName = survey.Configuration.Configurations.First().MetaFileName;
                _fileService.CreateDatabaseFile(metaFileName, destinationFilePath, destinationInstrumentName);
            }

            var caseRecord = _dataService.GetDataRecord(primaryKeyValue, sourceInstrumentName, sourceServerParkName);
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

            UseConnection(sourceConnectionModel);
            var caseRecord = _dataService.GetDataRecord(primaryKeyValue, sourceInstrumentName, sourceServerParkName);

            UseConnection(destinationConnectionModel);
            _dataService.WriteDataRecord(caseRecord, destinationInstrumentName, destinationServerParkName);

            UseConnection(sourceConnectionModel);
            RemoveCase(primaryKeyValue, sourceInstrumentName, sourceServerParkName);
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

            UseConnection(sourceConnectionModel);

            CopyCase(sourceConnectionModel, primaryKeyValue,  sourceInstrumentName, sourceServerParkName, destinationFilePath,
                destinationInstrumentName);

            RemoveCase(primaryKeyValue, sourceInstrumentName, sourceServerParkName);
        }

        public void RemoveCase(string primaryKeyValue,  string instrumentName, string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.RemoveDataRecord(primaryKeyValue, instrumentName, serverParkName);
        }

        public ConnectionModel GetDefaultConnectionModel()
        {
            return _configurationProvider.GetConnectionModel();
        }
    }
}
