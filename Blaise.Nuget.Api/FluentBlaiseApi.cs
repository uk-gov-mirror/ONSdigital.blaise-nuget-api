﻿using Blaise.Nuget.Api.Contracts.Interfaces;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private string _password;
        private string _role;
        private IList<string> _serverParkNames;
        private IDataRecord _caseDataRecord;

        private LastActionType _lastActionType;

        internal FluentBlaiseApi(IBlaiseApi blaiseApi)
        {
            _blaiseApi = blaiseApi;
            _serverParkNames = new List<string>();
        }

        public FluentBlaiseApi()
        {
            var unityContainer = new UnityContainer();
            unityContainer.RegisterType<IBlaiseApi, BlaiseApi>();
            _blaiseApi = unityContainer.Resolve<IBlaiseApi>();
        }

        public IFluentBlaiseApi WithServer(string serverName)
        {
            _blaiseApi.UseServer(serverName);

            return this;
        }

        public IFluentBlaiseApi WithServerPark(string serverParkName)
        {
            _lastActionType = LastActionType.ServerPark;
            _serverParkName = serverParkName;

            return this;
        }

        public IEnumerable<string> ServerParks()
        {
            return _blaiseApi.GetServerParkNames();
        }

        public IFluentBlaiseApi WithInstrument(string instrumentName)
        {
            _instrumentName = instrumentName;

            return this;
        }

        public IFluentBlaiseCaseApi WithCase(string primaryKeyValue)
        {
            _lastActionType = LastActionType.Case;
            _primaryKeyValue = primaryKeyValue;

            return this;
        }

        public IFluentBlaiseCaseApi WithCase(IDataRecord caseDataRecord)
        {
            _caseDataRecord = caseDataRecord;

            return this;
        }

        public IFluentBlaiseUserApi WithUser(string userName)
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

        public void Add()
        {
            switch (_lastActionType)
            {
                case LastActionType.User:
                    AddUser();
                    break;
                default:
                    throw new NotSupportedException("You have not declared a step previously where this action is supported");
            }
        }

        public IFluentBlaiseSurveyApi Survey => this;

        public IFluentBlaiseCaseApi WithStatus(StatusType statusType)
        {
            _lastActionType = statusType == StatusType.Completed
                ? LastActionType.Completed
                : LastActionType.Processed;

            return this;
        }

        public void Update()
        {
            switch (_lastActionType)
            {
                case LastActionType.Completed:
                    SetStatusAsComplete();
                    break;
                case LastActionType.Processed:
                    SetStatusAsProcessed();
                    break;
                case LastActionType.User:
                    UpdateUser();
                    break;
                default:
                    throw new NotSupportedException("You have not declared a step previously where this action is supported");
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
                case LastActionType.Completed:
                    return CompletedFieldExists();
                case LastActionType.Processed:
                    return ProcessedFieldExists();
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


        IFluentBlaiseSurveyApi IFluentBlaiseSurveyApi.WithField(FieldNameType fieldType)
        {
            _lastActionType = fieldType == FieldNameType.Completed
                ? LastActionType.Completed
                : LastActionType.Processed;

            return this;
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

        public void Remove()
        {
            ValidateUserIsSet();

            _blaiseApi.RemoveUser(_userName);
        }

        private void AddUser()
        {
            ValidateUserIsSet();
            ValidatePasswordIsSet();
            ValidateRoleIsSet();
            ValidateServerParksAreSet();

            _blaiseApi.AddUser(_userName, _password, _role, _serverParkNames);
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

        private bool CompletedFieldExists()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.CompletedFieldExists(_instrumentName, _serverParkName);
        }

        private bool ProcessedFieldExists()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.ProcessedFieldExists(_instrumentName, _serverParkName);
        }

        private void SetStatusAsComplete()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidateCaseDataRecordIsSet();

            _blaiseApi.MarkCaseAsComplete(_caseDataRecord, _instrumentName, _serverParkName);
        }

        private void SetStatusAsProcessed()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidateCaseDataRecordIsSet();

            _blaiseApi.MarkCaseAsProcessed(_caseDataRecord, _instrumentName, _serverParkName);
        }

        private void ValidateServerParkIsSet()
        {
            if (string.IsNullOrWhiteSpace(_serverParkName))
            {
                throw new NullReferenceException("The 'WithServerPark' step needs to be called prior to this to specify the name of the server park");
            }
        }

        private void ValidateInstrumentIsSet()
        {
            if (string.IsNullOrWhiteSpace(_instrumentName))
            {
                throw new NullReferenceException("The 'WithInstrument' step needs to be called prior to this to specify the name of the instrument");
            }
        }

        private void ValidatePrimaryKeyValueIsSet()
        {
            if (string.IsNullOrWhiteSpace(_primaryKeyValue))
            {
                throw new NullReferenceException("The 'WithCase' step needs to be called prior to this to specify the primary key value of the case");
            }
        }

        private void ValidateCaseDataRecordIsSet()
        {
            if (_caseDataRecord == null)
            {
                throw new NullReferenceException("The 'WithCase' step needs to be called prior to this to specify the data record of the case");
            }
        }

        private void ValidateUserIsSet()
        {
            if (_userName == null)
            {
                throw new NullReferenceException("The 'WithUser' step needs to be called prior to this to specify the name of the user");
            }
        }

        private void ValidatePasswordIsSet()
        {
            if (_password == null)
            {
                throw new NullReferenceException("The 'WithPassword' step needs to be called prior to this to specify the password of the user");
            }
        }

        private void ValidateRoleIsSet()
        {
            if (_role == null)
            {
                throw new NullReferenceException("The 'WithRole' step needs to be called prior to this to specify the role of the user");
            }
        }

        private void ValidateServerParksAreSet()
        {
            if (!_serverParkNames.Any())
            {
                throw new NullReferenceException("The 'WithServerParks' step needs to be called prior to this to specify the server parks of the user");
            }
        }
    }
}
