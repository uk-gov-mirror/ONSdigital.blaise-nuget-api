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
        private string _toServerParkName;
        private string _instrumentName;
        private string _toInstrumentName;
        private string _primaryKeyValue;
        private ConnectionModel _toConnectionModel;
        private string _filePath;
        private string _toFilePath;
        private string _userName;
        private string _password;
        private string _role;

        private Dictionary<string, string> _caseData;
        private IList<string> _serverParkNames;
        private IDataRecord _caseDataRecord;

        private StatusType _statusType;
        private FieldNameType _fieldNameType;
        private LastActionType _lastActionType;
        private HandleType _handleType;


        private void InitialiseSettings()
        {
            _serverParkName = null;
            _toServerParkName = null;
            _toServerParkName = null;
            _instrumentName = null;
            _toInstrumentName = null;
            _primaryKeyValue = null;
            _filePath = null;
            _toFilePath = null;
            _userName = null;
            _password = null;
            _role = null;
            _serverParkNames = new List<string>();
            _caseData = new Dictionary<string, string>();
            _toConnectionModel = null;
            _caseDataRecord = null;
            _statusType = StatusType.NotSpecified;
            _fieldNameType = FieldNameType.NotSpecified;
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
            unityContainer.RegisterType<IBlaiseApi, BlaiseApi>();
            _blaiseApi = unityContainer.Resolve<IBlaiseApi>();
        }

        public IFluentBlaiseApi WithConnection(ConnectionModel connectionModel)
        {
            _blaiseApi.UseConnection(connectionModel);

            return this;
        }

        public IFluentBlaiseHandler ToConnection(ConnectionModel connectionModel)
        {
            _toConnectionModel = connectionModel;

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

        public IEnumerable<string> ServerParks => _blaiseApi.GetServerParkNames();

        public IDataSet Cases
        {
            get
            {
                ValidateServerParkIsSet();
                ValidateInstrumentIsSet();

                return _blaiseApi.GetDataSet(_instrumentName, _serverParkName);
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

                return _blaiseApi.CaseHasBeenProcessed(_caseDataRecord);
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

        public IFluentBlaiseSurveyApi Survey => this;

        public IEnumerable<ISurvey> Surveys => _blaiseApi.GetAllSurveys();

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
                    case LastActionType.User:
                        return UserExists();
                    case LastActionType.Field:
                        return FieldExists();
                    default:
                        throw new NotSupportedException("You have not declared a step previously where this action is supported");
                }
            }
        }


        public IFluentBlaiseCaseApi WithStatus(StatusType statusType)
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

                return _blaiseApi.GetPrimaryKeyValue(_caseDataRecord);
            }
        }

        public bool Completed
        {
            get
            {
                ValidateCaseDataRecordIsSet();

                return _blaiseApi.CaseHasBeenCompleted(_caseDataRecord);
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

        IFluentBlaiseSurveyApi IFluentBlaiseSurveyApi.WithField(FieldNameType fieldType)
        {
            _lastActionType = LastActionType.Field;

            _fieldNameType = fieldType;

            return this;
        }

        public SurveyType Type
        {
            get
            {
                ValidateServerParkIsSet();
                ValidateInstrumentIsSet();

                return _blaiseApi.GetSurveyType(_instrumentName, _serverParkName);
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
            ValidateUserIsSet();
            ValidatePasswordIsSet();
            ValidateRoleIsSet();
            ValidateServerParksAreSet();

            _blaiseApi.AddUser(_userName, _password, _role, _serverParkNames);
            InitialiseSettings();
        }

        private void UpdateCase()
        {
            if (_caseData.Any())
            {
                ValidateInstrumentIsSet();
                ValidateServerParkIsSet();
                ValidateCaseDataRecordIsSet();

                _blaiseApi.UpdateDataRecord(_caseDataRecord, _caseData, _instrumentName, _serverParkName);
            }

            if (_statusType == StatusType.Completed)
            {
                SetStatusAsComplete();
            }

            if (_statusType == StatusType.Processed)
            {
                SetStatusAsProcessed();
            }

            InitialiseSettings();
        }

        private void UpdateUser()
        {
            ValidateUserIsSet();

            if (!string.IsNullOrWhiteSpace(_password))
            {
                _blaiseApi.ChangePassword(_userName, _password);
            }

            if (!string.IsNullOrWhiteSpace(_role) || _serverParkNames.Any())
            {
                ValidateRoleIsSet();
                ValidateServerParksAreSet();
                _blaiseApi.EditUser(_userName, _role, _serverParkNames);
            }

            InitialiseSettings();
        }

        private void RemoveCase()
        {
            ValidateInstrumentIsSet();
            ValidateServerParkIsSet();
            ValidatePrimaryKeyValueIsSet();

            _blaiseApi.RemoveCase(_primaryKeyValue, _instrumentName, _serverParkName);

            InitialiseSettings();
        }

        private void RemoveUser()
        {
            ValidateUserIsSet();

            _blaiseApi.RemoveUser(_userName);

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
            ValidateInstrumentIsSet();
            ValidateServerParkIsSet();
            ValidatePrimaryKeyValueIsSet();

            if (!string.IsNullOrWhiteSpace(_toFilePath))
            {
                ValidateToInstrumentIsSet();
                _blaiseApi.CopyCase(_primaryKeyValue, _instrumentName, _serverParkName, _toFilePath,
                    _toInstrumentName);

                InitialiseSettings();
                return;
            }

            if (_toConnectionModel != null)
            {
                ValidateToInstrumentIsSet();
                ValidateToServerParkIsSet();

                _blaiseApi.CopyCase(_primaryKeyValue, _instrumentName, _serverParkName, _toConnectionModel,
                    _toInstrumentName, _toServerParkName);

                InitialiseSettings();
                return;
            }
            InitialiseSettings();
            throw new ArgumentException("You must specify a file with the 'ToFile' step, or a server with the 'ToServer' step before calling handle");
        }

        private void HandleMove()
        {
            ValidateInstrumentIsSet();
            ValidateServerParkIsSet();
            ValidatePrimaryKeyValueIsSet();

            if (!string.IsNullOrWhiteSpace(_toFilePath))
            {
                ValidateToInstrumentIsSet();
                _blaiseApi.MoveCase(_primaryKeyValue, _instrumentName, _serverParkName, _toFilePath,
                    _toInstrumentName);

                InitialiseSettings();
                return;
            }

            if (_toConnectionModel != null)
            {
                ValidateToInstrumentIsSet();
                ValidateToServerParkIsSet();

                _blaiseApi.MoveCase(_primaryKeyValue, _instrumentName, _serverParkName, _toConnectionModel,
                    _toInstrumentName, _toServerParkName);

                InitialiseSettings();
                return;
            }

            InitialiseSettings();
            throw new ArgumentException("You must specify a file with the 'ToFile' step, or a server with the 'ToServer' step before calling handle");
        }

        private bool CaseExists()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidatePrimaryKeyValueIsSet();

            var caseExists = _blaiseApi.CaseExists(_primaryKeyValue, _instrumentName, _serverParkName);
            InitialiseSettings();

            return caseExists;
        }

        private bool ParkExists()
        {
            ValidateServerParkIsSet();

            var parkExists = _blaiseApi.ServerParkExists(_serverParkName);
            InitialiseSettings();

            return parkExists;
        }


        private bool UserExists()
        {
            ValidateUserIsSet();

            var userExists = _blaiseApi.UserExists(_userName);
            InitialiseSettings();

            return userExists;
        }

        private bool FieldExists()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            switch (_fieldNameType)
            {
                case FieldNameType.Completed:
                    var completedFieldExists = _blaiseApi.CompletedFieldExists(_instrumentName, _serverParkName);
                    InitialiseSettings();
                    return completedFieldExists;
                case FieldNameType.Processed:
                    var processedFieldExists = _blaiseApi.ProcessedFieldExists(_instrumentName, _serverParkName);
                    InitialiseSettings();
                    return processedFieldExists;
                default:
                    InitialiseSettings();
                    throw new NotSupportedException("You have not declared a field previously where this action is supported");
            }
        }

        private void SetStatusAsComplete()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidateCaseDataRecordIsSet();

            _blaiseApi.MarkCaseAsComplete(_caseDataRecord, _instrumentName, _serverParkName);
            InitialiseSettings();
        }

        private void SetStatusAsProcessed()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidateCaseDataRecordIsSet();

            _blaiseApi.MarkCaseAsProcessed(_caseDataRecord, _instrumentName, _serverParkName);
            InitialiseSettings();
        }

        private void AddCase()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidatePrimaryKeyValueIsSet();
            ValidateDataIsSet();

            _blaiseApi.CreateNewDataRecord(_primaryKeyValue, _caseData, _instrumentName, _serverParkName);
            InitialiseSettings();
        }

        private void ValidateServerParkIsSet()
        {
            if (string.IsNullOrWhiteSpace(_serverParkName))
            {
                throw new NullReferenceException("The 'WithServerPark' step needs to be called with a valid value prior to this to specify the name of the server park");
            }
        }

        private void ValidateToServerParkIsSet()
        {
            if (string.IsNullOrWhiteSpace(_toServerParkName))
            {
                throw new NullReferenceException("The 'ToServerPark' step needs to be called with a valid value prior to this to specify the destination name of the server park");
            }
        }

        private void ValidateInstrumentIsSet()
        {
            if (string.IsNullOrWhiteSpace(_instrumentName))
            {
                throw new NullReferenceException("The 'WithInstrument' step needs to be called with a valid value prior to this to specify the name of the instrument");
            }
        }

        private void ValidateToInstrumentIsSet()
        {
            if (string.IsNullOrWhiteSpace(_toInstrumentName))
            {
                throw new NullReferenceException("The 'ToInstrument' step needs to be called with a valid value prior to this to specify the destination name of the instrument");
            }
        }

        private void ValidatePrimaryKeyValueIsSet()
        {
            if (string.IsNullOrWhiteSpace(_primaryKeyValue))
            {
                throw new NullReferenceException("The 'WithPrimaryKey' step needs to be called with a valid value prior to this to specify the primary key value of the case");
            }
        }

        private void ValidateCaseDataRecordIsSet()
        {
            if (_caseDataRecord == null)
            {
                throw new NullReferenceException("The 'WithDataRecord' step needs to be called with a valid value prior to this to specify the data record of the case");
            }
        }

        private void ValidateUserIsSet()
        {
            if (_userName == null)
            {
                throw new NullReferenceException("The 'WithUser' step needs to be called with a valid value prior to this to specify the name of the user");
            }
        }

        private void ValidatePasswordIsSet()
        {
            if (_password == null)
            {
                throw new NullReferenceException("The 'WithPassword' step needs to be called with a valid value prior to this to specify the password of the user");
            }
        }

        private void ValidateRoleIsSet()
        {
            if (_role == null)
            {
                throw new NullReferenceException("The 'WithRole' step needs to be called with a valid value prior to this to specify the role of the user");
            }
        }

        private void ValidateServerParksAreSet()
        {
            if (!_serverParkNames.Any())
            {
                throw new NullReferenceException("The 'WithServerParks' step needs to be called with a valid value prior to this to specify the server parks of the user");
            }
        }

        private void ValidateDataIsSet()
        {
            if (!_caseData.Any())
            {
                throw new NullReferenceException("The 'WithData' step needs to be called with a valid value prior to this to specify the data fields of the case");
            }
        }
    }
}
