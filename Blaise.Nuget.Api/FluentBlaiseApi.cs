using Blaise.Nuget.Api.Contracts.Interfaces;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Enums;
using StatNeth.Blaise.API.ServerManager;
using Unity;

namespace Blaise.Nuget.Api
{
    public class FluentBlaiseApi : IFluentBlaiseApi, IFluentBlaiseCaseApi, IFluentBlaiseSurveyApi, IFluentBlaiseUserApi
    {
        private readonly IBlaiseApi _blaiseApi;

        private string _serverParkName;
        private string _instrumentName;
        private string _primaryKeyValue;
        private string _userName;
        private IDataRecord _caseDataRecord;

        private LastActionType _lastActionType;

        internal FluentBlaiseApi(IBlaiseApi blaiseApi)
        {
            _blaiseApi = blaiseApi;
        }

        public FluentBlaiseApi()
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IBlaiseApi, BlaiseApi>();
            _blaiseApi = unityContainer.Resolve<IBlaiseApi>();
        }

        public IFluentBlaiseApi Server(string serverName)
        {
            _blaiseApi.UseServer(serverName);

            return this;
        }

        public IFluentBlaiseApi ServerPark(string serverParkName)
        {
            _lastActionType = LastActionType.ServerPark;
            _serverParkName = serverParkName;

            return this;
        }

        public IEnumerable<string> ServerParks()
        {
            return _blaiseApi.GetServerParkNames();
        }

        public IFluentBlaiseApi Instrument(string instrumentName)
        {
            _instrumentName = instrumentName;

            return this;
        }

        public IFluentBlaiseCaseApi Case(string primaryKeyValue)
        {
            _lastActionType = LastActionType.Case;
            _primaryKeyValue = primaryKeyValue;

            return this;
        }

        public IFluentBlaiseCaseApi Case(IDataRecord caseDataRecord)
        {
            _caseDataRecord = caseDataRecord;

            return this;
        }

        public IFluentBlaiseCaseApi Case()
        {
            return this;
        }

        public IFluentBlaiseUserApi User(string userName)
        {
            _lastActionType = LastActionType.User;
            _userName = userName;

            return this;
        }

        public IFluentBlaiseSurveyApi Survey()
        {
            return this;
        }

        public void SetStatusAs(StatusType statusType)
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidateCaseDataRecordIsSet();

            switch (statusType)
            {
                case StatusType.Completed:
                    _blaiseApi.MarkCaseAsComplete(_caseDataRecord, _instrumentName, _serverParkName);
                    break;
                case StatusType.Processed:
                    _blaiseApi.MarkCaseAsProcessed(_caseDataRecord, _instrumentName, _serverParkName);
                    break;
            }
        }

        public bool Exists()
        {
            switch (_lastActionType)
            {
                case LastActionType.Case:
                    return CaseExists();
                case LastActionType.ServerPark:
                    return ParkExists();
                case LastActionType.User:
                    return UserExists();
                default:
                    throw new NotSupportedException("You have not declared a step previously where this action is supported");
            }
        }

        public IEnumerable<ISurvey> Surveys()
        {
            return _blaiseApi.GetAllSurveys();
        }

        public SurveyType Type()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.GetSurveyType(_instrumentName, _serverParkName);
        }

        public bool HasField(FieldNameType fieldNameType)
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return fieldNameType == FieldNameType.Completed 
                ? _blaiseApi.CompletedFieldExists(_instrumentName, _serverParkName) 
                : _blaiseApi.ProcessedFieldExists(_instrumentName, _serverParkName);
        }

        public IDataSet Cases()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.GetDataSet(_instrumentName, _serverParkName);
        }

        public string PrimaryKeyValue()
        {
            ValidateCaseDataRecordIsSet();

            return _blaiseApi.GetPrimaryKeyValue(_caseDataRecord);
        }

        public bool IsComplete()
        {
            ValidateCaseDataRecordIsSet();

            return _blaiseApi.CaseHasBeenCompleted(_caseDataRecord);
        }

        public bool HasBeenProcessed()
        {
            ValidateCaseDataRecordIsSet();

            return _blaiseApi.CaseHasBeenProcessed(_caseDataRecord);
        }


        public void Add(Dictionary<string, string> data)
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidatePrimaryKeyValueIsSet();

            _blaiseApi.CreateNewDataRecord(_primaryKeyValue, data, _instrumentName, _serverParkName);
        }

        public void Add(string password, string role, IList<string> serverParkNames)
        {
            ValidateUserIsSet();

            _blaiseApi.AddUser(_userName, password, role, serverParkNames);
        }

        public void Update(string role, IList<string> serverParkNames)
        {
            ValidateUserIsSet();

            _blaiseApi.EditUser(_userName, role, serverParkNames);
        }

        public void ChangePassword(string password)
        {
            ValidateUserIsSet();

            _blaiseApi.ChangePassword(_userName, password);
        }

        public void Remove()
        {
            ValidateUserIsSet();

            _blaiseApi.RemoveUser(_userName);
        }

        private bool CaseExists()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidatePrimaryKeyValueIsSet();

            return _blaiseApi.CaseExists(_primaryKeyValue, _instrumentName, _serverParkName);
        }

        private bool ParkExists()
        {
            ValidateServerParkIsSet();

            return _blaiseApi.ServerParkExists(_serverParkName);
        }


        private bool UserExists()
        {
            ValidateUserIsSet();

            return _blaiseApi.UserExists(_userName);
        }

        private void ValidateServerParkIsSet()
        {
            if (string.IsNullOrWhiteSpace(_serverParkName))
            {
                throw new NullReferenceException("The 'ServerPark' step needs to be called prior to this to specify the name of the server park");
            }
        }

        private void ValidateInstrumentIsSet()
        {
            if (string.IsNullOrWhiteSpace(_instrumentName))
            {
                throw new NullReferenceException("The 'Instrument' step needs to be called prior to this to specify the name of the instrument");
            }
        }

        private void ValidatePrimaryKeyValueIsSet()
        {
            if (string.IsNullOrWhiteSpace(_primaryKeyValue))
            {
                throw new NullReferenceException("The 'Case' step needs to be called prior to this to specify the primary key value of the case");
            }
        }

        private void ValidateCaseDataRecordIsSet()
        {
            if (_caseDataRecord == null)
            {
                throw new NullReferenceException("The 'Case' step needs to be called prior to this to specify the data record of the case");
            }
        }

        private void ValidateUserIsSet()
        {
            if (_userName == null)
            {
                throw new NullReferenceException("The 'User' step needs to be called prior to this to specify the name of the user");
            }
        }
    }
}
