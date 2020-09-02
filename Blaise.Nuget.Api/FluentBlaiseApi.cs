using Blaise.Nuget.Api.Contracts.Interfaces;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Enums;
using StatNeth.Blaise.API.ServerManager;
using Unity;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api
{
    public class FluentBlaiseApi : IFluentBlaiseApi, IFluentBlaiseCaseApi, IFluentBlaiseSurveyApi, IFluentBlaiseUserApi, IFluentBlaiseHandler
    {
        private readonly IBlaiseApi _blaiseApi;

        private string _serverParkName;
        private string _defaultServerPark;
        private string _toServerParkName;
        private string _instrumentName;
        private string _toInstrumentName;
        private string _primaryKeyValue;
        private ConnectionModel _sourceConnectionModel;
        private ConnectionModel _destinationConnectionModel;
        private string _filePath;
        private string _toFilePath;
        private string _userName;
        private string _password;
        private string _role;
        private string _destinationPath;
        private string _bucketName;

        private Dictionary<string, string> _caseData;
        private IList<string> _serverParkNames;
        private IDataRecord _caseDataRecord;

        private CaseStatusType _statusType;
        private LastActionType _lastActionType;
        private HandleType _handleType;


        private void InitialiseSettings()
        {
            _serverParkName = null;
            _defaultServerPark = null;
            _toServerParkName = null;
            _toServerParkName = null;
            _instrumentName = null;
            _toInstrumentName = null;
            _primaryKeyValue = null;
            _filePath = null;
            _toFilePath = null;
            _destinationPath = null;
            _bucketName = null;
            _userName = null;
            _password = null;
            _role = null;
            _serverParkNames = new List<string>();
            _caseData = new Dictionary<string, string>();
            _sourceConnectionModel = null;
            _destinationConnectionModel = null;
            _caseDataRecord = null;
            _statusType = CaseStatusType.NotSpecified;
            _lastActionType = LastActionType.NotSupported;
            _handleType = HandleType.NotSupported;
        }


        internal FluentBlaiseApi(IBlaiseApi blaiseApi)
        {
            InitialiseSettings();

            _blaiseApi = blaiseApi;
            _serverParkNames = new List<string>();
            _caseData = new Dictionary<string, string>();
        }

        public FluentBlaiseApi()
        {
            InitialiseSettings();

            var unityContainer = new UnityContainer();
            unityContainer.RegisterSingleton<IBlaiseApi, BlaiseApi>();
            _blaiseApi = unityContainer.Resolve<IBlaiseApi>();
        }

        public IFluentBlaiseApi WithConnection(ConnectionModel connectionModel)
        {
            _sourceConnectionModel = connectionModel;

            return this;
        }

        public IFluentBlaiseHandler ToConnection(ConnectionModel connectionModel)
        {
            _destinationConnectionModel = connectionModel;

            return this;
        }

        public IFluentBlaiseApi WithServerPark(string serverParkName)
        {
            _lastActionType = LastActionType.ServerPark;
            _serverParkName = serverParkName;

            return this;
        }

        public IFluentBlaiseHandler ToServerPark(string serverParkName)
        {
            _toServerParkName = serverParkName;

            return this;
        }

        public IFluentBlaiseApi WithFile(string filePath)
        {
            _filePath = filePath;

            return this;
        }

        public IFluentBlaiseHandler ToFile(string filePath)
        {
            _toFilePath = filePath;

            return this;
        }

        public IEnumerable<string> ServerParks
        {
            get
            {
                ValidateSourceConnectionIsSet();

                var serverParkNames = _blaiseApi.GetServerParkNames(_sourceConnectionModel);

                InitialiseSettings();

                return serverParkNames;
            }
        }

        public IDataSet Cases
        {
            get
            {
                var cases = GetCases();

                InitialiseSettings();
                return cases;
            }
        }

        public IFluentBlaiseUserApi User
        {
            get
            {
                _lastActionType = LastActionType.User;

                return this;
            }
        }

        public IFluentBlaiseCaseApi Case
        {
            get
            {
                _lastActionType = LastActionType.Case;

                return this;
            }
        }

        public IFluentBlaiseApi WithInstrument(string instrumentName)
        {
            _instrumentName = instrumentName;

            return this;
        }

        public IFluentBlaiseHandler ToInstrument(string instrumentName)
        {
            _toInstrumentName = instrumentName;

            return this;
        }

        public IFluentBlaiseCaseApi WithPrimaryKey(string primaryKeyValue)
        {
            _primaryKeyValue = primaryKeyValue;

            return this;
        }

        public IFluentBlaiseCaseApi WithDataRecord(IDataRecord caseDataRecord)
        {
            _caseDataRecord = caseDataRecord;

            return this;
        }

        public IFluentBlaiseUserApi WithUserName(string userName)
        {
            _lastActionType = LastActionType.User;
            _userName = userName;

            return this;
        }

        public IFluentBlaiseUserApi WithPassword(string password)
        {
            _lastActionType = LastActionType.User;
            _password = password;

            return this;
        }

        public IFluentBlaiseUserApi WithRole(string role)
        {
            _lastActionType = LastActionType.User;
            _role = role;

            return this;
        }

        public IFluentBlaiseUserApi WithServerParks(IList<string> serverParkNames)
        {
            _lastActionType = LastActionType.User;
            _serverParkNames = serverParkNames;

            return this;
        }

        public IFluentBlaiseUserApi WithDefaultServerPark(string defaultServerPark)
        {
            _lastActionType = LastActionType.User;
            _defaultServerPark = defaultServerPark;

            return this;
        }

        public IFluentBlaiseCaseApi WithData(Dictionary<string, string> data)
        {
            _lastActionType = LastActionType.Case;

            _caseData = data;

            return this;
        }

        public bool Processed
        {
            get
            {
                ValidateCaseDataRecordIsSet();
                var processed = _blaiseApi.CaseHasBeenProcessed(_caseDataRecord);

                InitialiseSettings();
                return processed;
            }
        }

        public WebFormStatusType WebFormStatus
        {
            get
            {
                var status = GetWebFormStatusType();

                InitialiseSettings();

                return status;
            }
        }

        public void Add()
        {
            switch (_lastActionType)
            {
                case LastActionType.Case:
                    AddCase();
                    break;
                case LastActionType.User:
                    AddUser();
                    break;
                default:
                    throw new NotSupportedException("You have not declared a step previously where this action is supported");
            }
        }

        public IFluentBlaiseSurveyApi Survey
        {
            get
            {
                _lastActionType = LastActionType.Survey;
                return this;
            }
        }

        public IEnumerable<ISurvey> Surveys
        {
            get
            {
                ValidateSourceConnectionIsSet();

                var surveys = string.IsNullOrWhiteSpace(_serverParkName)
                    ? _blaiseApi.GetAllSurveys(_sourceConnectionModel)
                    : _blaiseApi.GetSurveys(_sourceConnectionModel, _serverParkName);

                InitialiseSettings();

                return surveys;
            }
        }

        public bool Exists
        {
            get
            {
                switch (_lastActionType)
                {
                    case LastActionType.Case:
                        return CaseExists();
                    case LastActionType.ServerPark:
                        return ParkExists();
                    case LastActionType.Survey:
                        return SurveyExists();
                    case LastActionType.User:
                        return UserExists();
                    default:
                        throw new NotSupportedException("You have not declared a step previously where this action is supported");
                }
            }
        }

        public bool HasField(FieldNameType fieldNameType)
        {
            return FieldExists(fieldNameType);
        }
        
        public IFluentBlaiseSurveyApi ToPath(string destinationPath)
        {
            _destinationPath = destinationPath;

            return this;
        }

        public IFluentBlaiseSurveyApi ToBucket(string bucketName)
        {
            _bucketName = bucketName;

            return this;
        }

        public void Backup()
        {
            ValidateSourceConnectionIsSet();
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            if (string.IsNullOrWhiteSpace(_bucketName))
            {
                ValidateDestinationPathIsSet();

                _blaiseApi.BackupSurveyToFile(_sourceConnectionModel, _serverParkName, _instrumentName, _destinationPath);
            }
            else
            {
                _blaiseApi.BackupSurveyToBucket(_sourceConnectionModel, _serverParkName, _instrumentName, _bucketName, _destinationPath);
            }

            InitialiseSettings();
        }

        public IFluentBlaiseCaseApi WithStatus(CaseStatusType statusType)
        {
            _lastActionType = LastActionType.Case;

            _statusType = statusType;

            return this;
        }

        public string PrimaryKey
        {
            get
            {
                ValidateCaseDataRecordIsSet();

                var primaryKey = _blaiseApi.GetPrimaryKeyValue(_caseDataRecord);

                InitialiseSettings();

                return primaryKey;
            }
        }

        public string CaseId => GetCaseId();
        public decimal HOut => GetHOut();

        public bool Completed
        {
            get
            {
                ValidateCaseDataRecordIsSet();

                var completed = _blaiseApi.CaseHasBeenCompleted(_caseDataRecord);

                InitialiseSettings();

                return completed;
            }
        }

        public void Update()
        {
            switch (_lastActionType)
            {
                case LastActionType.Case:
                    UpdateCase();
                    break;
                case LastActionType.User:
                    UpdateUser();
                    break;
                default:
                    throw new NotSupportedException("You have not declared a step previously where this action is supported");
            }
        }

        public IDataRecord Get()
        {
            switch (_lastActionType)
            {
                case LastActionType.Case:
                    return GetCase();
                default:
                    throw new NotSupportedException("You have not declared a step previously where this action is supported");
            }
        }

        public IFluentBlaiseHandler Copy
        {
            get
            {
                _handleType = HandleType.Copy;

                return this;
            }
        }

        public IFluentBlaiseHandler Move
        {
            get
            {
                _handleType = HandleType.Move;

                return this;
            }
        }

        public SurveyType Type
        {
            get
            {
                ValidateSourceConnectionIsSet();
                ValidateServerParkIsSet();
                ValidateInstrumentIsSet();

                var surveyType = _blaiseApi.GetSurveyType(_sourceConnectionModel, _instrumentName, _serverParkName);

                InitialiseSettings();

                return surveyType;
            }
        }

        public ConnectionModel DefaultConnection => _blaiseApi.GetDefaultConnectionModel();

        public void Remove()
        {
            switch (_lastActionType)
            {
                case LastActionType.Case:
                    RemoveCase();
                    break;
                case LastActionType.User:
                    RemoveUser();
                    break;
                default:
                    throw new NotSupportedException("You have not declared a step previously where this action is supported");
            }
        }

        public void Handle()
        {
            switch (_lastActionType)
            {
                case LastActionType.Case:
                    HandleCase();
                    break;
                default:
                    throw new NotSupportedException("You have not declared a step previously where this action is supported");
            }
        }

        private void AddUser()
        {
            ValidateSourceConnectionIsSet();
            ValidateUserIsSet();
            ValidatePasswordIsSet();
            ValidateRoleIsSet();
            ValidateDefaultServerParkIsSet();

            _blaiseApi.AddUser(_sourceConnectionModel, _userName, _password, _role, _serverParkNames, _defaultServerPark);
            InitialiseSettings();
        }

        private IDataRecord GetCase()
        {
            ValidateSourceConnectionIsSet();
            ValidatePrimaryKeyValueIsSet();
            ValidateInstrumentIsSet();
            ValidateServerParkIsSet();

            var dataRecord = _blaiseApi.GetDataRecord(_sourceConnectionModel, _primaryKeyValue, _instrumentName, _serverParkName);

            InitialiseSettings();

            return dataRecord;
        }

        private void UpdateCase()
        {
            if (_caseData.Any())
            {
                ValidateSourceConnectionIsSet();
                ValidateInstrumentIsSet();
                ValidateServerParkIsSet();

                if (_caseDataRecord == null)
                {
                    ValidatePrimaryKeyValueIsSet();
                    _caseDataRecord = _blaiseApi.GetDataRecord(_sourceConnectionModel, _primaryKeyValue, _instrumentName, _serverParkName);
                }

                _blaiseApi.UpdateDataRecord(_sourceConnectionModel, _caseDataRecord, _caseData, _instrumentName, _serverParkName);
            }

            if (_statusType == CaseStatusType.Completed)
            {
                SetStatusAsComplete();
            }

            if (_statusType == CaseStatusType.Processed)
            {
                SetStatusAsProcessed();
            }

            InitialiseSettings();
        }

        private void UpdateUser()
        {
            ValidateSourceConnectionIsSet();
            ValidateUserIsSet();

            if (!string.IsNullOrWhiteSpace(_password))
            {
                _blaiseApi.ChangePassword(_sourceConnectionModel, _userName, _password);
            }

            if (!string.IsNullOrWhiteSpace(_role) || _serverParkNames.Any())
            {
                ValidateRoleIsSet();

                _blaiseApi.EditUser(_sourceConnectionModel, _userName, _role, _serverParkNames);
            }

            InitialiseSettings();
        }

        private void RemoveCase()
        {
            ValidateSourceConnectionIsSet();
            ValidateInstrumentIsSet();
            ValidateServerParkIsSet();
            ValidatePrimaryKeyValueIsSet();

            _blaiseApi.RemoveCase(_sourceConnectionModel, _primaryKeyValue, _instrumentName, _serverParkName);

            InitialiseSettings();
        }

        private void RemoveUser()
        {
            ValidateSourceConnectionIsSet();
            ValidateUserIsSet();

            _blaiseApi.RemoveUser(_sourceConnectionModel, _userName);

            InitialiseSettings();
        }

        private void HandleCase()
        {
            switch (_handleType)
            {
                case HandleType.Copy:
                    HandleCopy();
                    break;
                case HandleType.Move:
                    HandleMove();
                    break;
                default:
                    throw new NotSupportedException(
                        "You have not declared a step previously where this action is supported");
            }
        }

        private void HandleCopy()
        {
            ValidateSourceConnectionIsSet();
            ValidateInstrumentIsSet();
            ValidateServerParkIsSet();
            ValidatePrimaryKeyValueIsSet();

            if (!string.IsNullOrWhiteSpace(_toFilePath))
            {
                ValidateToInstrumentIsSet();
                _blaiseApi.CopyCase(_sourceConnectionModel, _primaryKeyValue, _instrumentName, _serverParkName, _toFilePath,
                    _toInstrumentName);

                InitialiseSettings();
                return;
            }

            if (_destinationConnectionModel != null)
            {
                ValidateToInstrumentIsSet();
                ValidateToServerParkIsSet();

                _blaiseApi.CopyCase(_sourceConnectionModel, _primaryKeyValue, _instrumentName, _serverParkName, _destinationConnectionModel,
                    _toInstrumentName, _toServerParkName);

                InitialiseSettings();
                return;
            }
            InitialiseSettings();
            throw new ArgumentException("You must specify a file with the 'ToFile' step, or a server with the 'ToServer' step before calling handle");
        }

        private void HandleMove()
        {
            ValidateSourceConnectionIsSet();
            ValidateInstrumentIsSet();
            ValidateServerParkIsSet();
            ValidatePrimaryKeyValueIsSet();

            if (!string.IsNullOrWhiteSpace(_toFilePath))
            {
                ValidateToInstrumentIsSet();
                _blaiseApi.MoveCase(_sourceConnectionModel, _primaryKeyValue, _instrumentName, _serverParkName, _toFilePath,
                    _toInstrumentName);

                InitialiseSettings();
                return;
            }

            if (_destinationConnectionModel != null)
            {
                ValidateToInstrumentIsSet();
                ValidateToServerParkIsSet();

                _blaiseApi.MoveCase(_sourceConnectionModel, _primaryKeyValue, _instrumentName, _serverParkName, _destinationConnectionModel,
                    _toInstrumentName, _toServerParkName);

                InitialiseSettings();
                return;
            }

            InitialiseSettings();
            throw new ArgumentException("You must specify a file with the 'ToFile' step, or a server with the 'ToServer' step before calling handle");
        }

        private IDataSet GetCases()
        {
            var cases = string.IsNullOrWhiteSpace(_filePath)
                ? GetCasesFromDatabase()
                : GetCasesFromFile();

            return cases;
        }

        private IDataSet GetCasesFromFile()
        {
            return _blaiseApi.GetDataSet(_filePath);
        }

        private IDataSet GetCasesFromDatabase()
        {
            ValidateSourceConnectionIsSet();
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.GetDataSet(_sourceConnectionModel, _instrumentName, _serverParkName);
        }

        private bool CaseExists()
        {
            ValidateSourceConnectionIsSet();
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            bool caseExists;
            if (_caseDataRecord != null)
            {
                caseExists = _blaiseApi.CaseExists(_sourceConnectionModel, _caseDataRecord, _instrumentName, _serverParkName);
            }
            else
            {
                ValidatePrimaryKeyValueIsSet();

                caseExists = _blaiseApi.CaseExists(_sourceConnectionModel, _primaryKeyValue, _instrumentName, _serverParkName);
            }

            InitialiseSettings();

            return caseExists;
        }

        private bool ParkExists()
        {
            ValidateSourceConnectionIsSet();
            ValidateServerParkIsSet();

            var parkExists = _blaiseApi.ServerParkExists(_sourceConnectionModel, _serverParkName);
            InitialiseSettings();

            return parkExists;
        }

        private bool SurveyExists()
        {
            ValidateSourceConnectionIsSet();
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            var surveyExists = _blaiseApi.SurveyExists(_sourceConnectionModel, _instrumentName, _serverParkName);
            InitialiseSettings();

            return surveyExists;
        }

        private bool UserExists()
        {
            ValidateSourceConnectionIsSet();
            ValidateUserIsSet();

            var userExists = _blaiseApi.UserExists(_sourceConnectionModel, _userName);
            InitialiseSettings();

            return userExists;
        }

        private bool FieldExists(FieldNameType fieldNameType)
        {
            bool fieldExists;
            if (_caseDataRecord != null)
            {
                fieldExists = _blaiseApi.FieldExists(_caseDataRecord, fieldNameType);
            }
            else
            {
                ValidateSourceConnectionIsSet();
                ValidateServerParkIsSet();
                ValidateInstrumentIsSet();

                fieldExists = _blaiseApi.FieldExists(_sourceConnectionModel, _instrumentName, _serverParkName, fieldNameType);
            }

            InitialiseSettings();

            return fieldExists;
        }

        private void SetStatusAsComplete()
        {
            ValidateSourceConnectionIsSet();
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidateCaseDataRecordIsSet();

            _blaiseApi.MarkCaseAsComplete(_sourceConnectionModel, _caseDataRecord, _instrumentName, _serverParkName);
            InitialiseSettings();
        }

        private void SetStatusAsProcessed()
        {
            ValidateSourceConnectionIsSet();
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidateCaseDataRecordIsSet();

            _blaiseApi.MarkCaseAsProcessed(_sourceConnectionModel, _caseDataRecord, _instrumentName, _serverParkName);
            InitialiseSettings();
        }

        private string GetCaseId()
        {
            ValidateCaseDataRecordIsSet();
            var dataValue = _blaiseApi.GetFieldValue(_caseDataRecord, FieldNameType.CaseId);

            InitialiseSettings();

            return dataValue.ValueAsText;
        }

        private decimal GetHOut()
        {
            ValidateCaseDataRecordIsSet();
            var dataValue = _blaiseApi.GetFieldValue(_caseDataRecord, FieldNameType.HOut);

            InitialiseSettings();

            return dataValue.IntegerValue;
        }

        private WebFormStatusType GetWebFormStatusType()
        {
            IDataValue dataValue;

            if (_caseDataRecord != null)
            {
                dataValue = _blaiseApi.GetFieldValue(_caseDataRecord, FieldNameType.WebFormStatus);
            }
            else
            {
                ValidateSourceConnectionIsSet();
                ValidateInstrumentIsSet();
                ValidateServerParkIsSet();
                ValidatePrimaryKeyValueIsSet();

                dataValue = _blaiseApi.GetFieldValue(_sourceConnectionModel, _primaryKeyValue, _instrumentName, _serverParkName,
                    FieldNameType.WebFormStatus);
            }

            switch (dataValue.EnumerationValue)
            {
                case 0:
                    return WebFormStatusType.NotProcessed;
                case 1:
                    return WebFormStatusType.Complete;
                case 2:
                    return WebFormStatusType.Partial;
                default:
                    return WebFormStatusType.NotSpecified;
            }
        }

        private void AddCase()
        {
            ValidateSourceConnectionIsSet();
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidatePrimaryKeyValueIsSet();
            ValidateDataIsSet();

            _blaiseApi.CreateNewDataRecord(_sourceConnectionModel, _primaryKeyValue, _caseData, _instrumentName, _serverParkName);
            InitialiseSettings();
        }

        private void ValidateSourceConnectionIsSet()
        {
            if (_sourceConnectionModel == null)
            {
                throw new NullReferenceException("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to");
            }
        }

        private void ValidateDestinationPathIsSet()
        {
            if (_destinationPath == null)
            {
                throw new NullReferenceException("The 'ToPath' step needs to be called with a valid value, check that the step has been called with a valid value for the destination file path");
            }
        }

        private void ValidateServerParkIsSet()
        {
            if (string.IsNullOrWhiteSpace(_serverParkName))
            {
                throw new NullReferenceException("The 'WithServerPark' step needs to be called with a valid value, check that the step has been called with a valid value for the server park");
            }
        }

        private void ValidateToServerParkIsSet()
        {
            if (string.IsNullOrWhiteSpace(_toServerParkName))
            {
                throw new NullReferenceException("The 'ToServerPark' step needs to be called with a valid value, check that the step has been called with a valid value for the destination name of the server park");
            }
        }

        private void ValidateInstrumentIsSet()
        {
            if (string.IsNullOrWhiteSpace(_instrumentName))
            {
                throw new NullReferenceException("The 'WithInstrument' step needs to be called with a valid value, check that the step has been called with a valid instrument");
            }
        }

        private void ValidateToInstrumentIsSet()
        {
            if (string.IsNullOrWhiteSpace(_toInstrumentName))
            {
                throw new NullReferenceException("The 'ToInstrument' step needs to be called with a valid value, check that the step has been called with a valid value for the destination instrument");
            }
        }

        private void ValidatePrimaryKeyValueIsSet()
        {
            if (string.IsNullOrWhiteSpace(_primaryKeyValue))
            {
                throw new NullReferenceException("The 'WithPrimaryKey' step needs to be called with a valid value, check that the step has been called with a valid primary key value");
            }
        }

        private void ValidateCaseDataRecordIsSet()
        {
            if (_caseDataRecord == null)
            {
                throw new NullReferenceException("The 'WithDataRecord' step needs to be called with a valid value, check that the step has been called with a valid data record");
            }
        }

        private void ValidateUserIsSet()
        {
            if (_userName == null)
            {
                throw new NullReferenceException("The 'WithUser' step needs to be called with a valid value, check that the step has been called with a valid user name");
            }
        }

        private void ValidatePasswordIsSet()
        {
            if (_password == null)
            {
                throw new NullReferenceException("The 'WithPassword' step needs to be called with a valid value, check that the step has been called with a valid password");
            }
        }

        private void ValidateRoleIsSet()
        {
            if (_role == null)
            {
                throw new NullReferenceException("The 'WithRole' step needs to be called with a valid value, check that the step has been called with a valid role");
            }
        }

        private void ValidateDefaultServerParkIsSet()
        {
            if (_defaultServerPark == null)
            {
                throw new NullReferenceException("The 'WithDefaultServerPark' step needs to be called with a valid value, check that the step has been called with a valid server park");
            }
        }

        private void ValidateDataIsSet()
        {
            if (!_caseData.Any())
            {
                throw new NullReferenceException("The 'WithData' step needs to be called with a valid value, check that the step has been called with a valid set of data");
            }
        }
    }
}
