using Blaise.Nuget.Api.Contracts.Interfaces;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.ServerManager;
using Unity;

namespace Blaise.Nuget.Api
{
    public class FluentBlaiseApi : IFluentBlaiseApi
    {
        private readonly IBlaiseApi _blaiseApi;

        private string _serverParkName;
        private string _instrumentName;
        private string _primaryKeyValue;
        private IDataRecord _caseDataRecord;

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

        public IFluentBlaiseCaseApi Case(string primaryKeyValue)
        {
            _primaryKeyValue = primaryKeyValue;

            return this;
        }

        public IFluentBlaiseCaseApi Case(IDataRecord caseDataRecord)
        {
            _caseDataRecord = caseDataRecord;

            return this;
        }


        public IFluentBlaiseApi ServerPark(string serverParkName)
        {
            _serverParkName = serverParkName;

            return this;
        }

        public IFluentBlaiseApi Instrument(string instrumentName)
        {
            _instrumentName = instrumentName;

            return this;
        }
        
        public SurveyType SurveyType()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.GetSurveyType(_instrumentName, _serverParkName);
        }

        public bool CompletedFieldExists()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.CompletedFieldExists(_instrumentName, _serverParkName);
        }

        public bool ProcessedFieldExists()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.ProcessedFieldExists(_instrumentName, _serverParkName);
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

        public bool Completed()
        {
            ValidateCaseDataRecordIsSet();

            return _blaiseApi.CaseHasBeenCompleted(_caseDataRecord);
        }

        public bool Processed()
        {
            ValidateCaseDataRecordIsSet();

            return _blaiseApi.CaseHasBeenProcessed(_caseDataRecord);
        }

        public void MarkAsComplete()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidateCaseDataRecordIsSet();

            _blaiseApi.MarkCaseAsComplete(_caseDataRecord, _instrumentName, _serverParkName);
        }

        public void MarkAsProcessed()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidateCaseDataRecordIsSet();

            _blaiseApi.MarkCaseAsProcessed(_caseDataRecord, _instrumentName, _serverParkName);
        }

        public void Create(Dictionary<string, string> data)
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();
            ValidatePrimaryIsSet();

            _blaiseApi.CreateNewDataRecord(_primaryKeyValue, data, _instrumentName, _serverParkName);
        }

        public bool Exists()
        {
            if (string.IsNullOrWhiteSpace(_primaryKeyValue))
            {
                ValidateInstrumentIsSet();

                return _blaiseApi.ServerParkExists(_serverParkName);
            }

            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.CaseExists(_primaryKeyValue, _instrumentName, _serverParkName);
        }

        public IEnumerable<ISurvey> Surveys()
        {
            return _blaiseApi.GetAllSurveys();
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

        private void ValidatePrimaryIsSet()
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
    }
}
