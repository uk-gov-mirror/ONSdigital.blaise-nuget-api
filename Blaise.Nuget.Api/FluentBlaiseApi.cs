using Blaise.Nuget.Api.Contracts.Interfaces;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
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
        private string _filePath;

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

        public IEnumerable<string> GetServerParkNames()
        {
            return _blaiseApi.GetServerParkNames();
        }

        public IEnumerable<ISurvey> GetAllSurveys()
        {
            return _blaiseApi.GetAllSurveys();
        }

        public bool ServerParkExists(string serverParkName)
        {
            return _blaiseApi.ServerParkExists(serverParkName);
        }

        public IKey GetKey(IDatamodel dataModel, string keyName)
        {
            return _blaiseApi.GetKey(dataModel, keyName);
        }

        public IKey GetPrimaryKey(IDatamodel dataModel)
        {
            return _blaiseApi.GetPrimaryKey(dataModel);
        }

        public string GetPrimaryKeyValue(IDataRecord dataRecord)
        {
            return _blaiseApi.GetPrimaryKeyValue(dataRecord);
        }

        public void AssignPrimaryKeyValue(IKey key, string primaryKeyValue)
        {
            _blaiseApi.AssignPrimaryKeyValue(key, primaryKeyValue);
        }

        public IDataRecord GetDataRecord(IDatamodel dataModel)
        {
            return _blaiseApi.GetDataRecord(dataModel);
        }

        public bool CaseHasBeenCompleted(IDataRecord dataRecord)
        {
            return _blaiseApi.CaseHasBeenCompleted(dataRecord);
        }

        public bool CaseHasBeenProcessed(IDataRecord dataRecord)
        {
            return _blaiseApi.CaseHasBeenProcessed(dataRecord);
        }

        public IFluentBlaiseRemoteApi WithServerPark(string serverParkName)
        {
            _filePath = null;
            _serverParkName = serverParkName;

            return this;
        }

        public IEnumerable<string> GetSurveyNames()
        {
            ValidateServerParkIsSet();

            return _blaiseApi.GetSurveyNames(_serverParkName);
        }

        public IEnumerable<ISurvey> GetSurveys()
        {
            ValidateServerParkIsSet();

            return _blaiseApi.GetSurveys(_serverParkName);
        }

        public IFluentBlaiseRemoteApi ForInstrument(string instrumentName)
        {
            _filePath = null;
            _instrumentName = instrumentName;

            return this;
        }

        public bool CaseExists(string primaryKeyValue)
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.CaseExists(primaryKeyValue, _instrumentName, _serverParkName);
        }

        public Guid GetInstrumentId()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.GetInstrumentId(_instrumentName, _serverParkName);
        }

        public IDatamodel GetDataModel()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.GetDataModel(_instrumentName, _serverParkName);
        }

        public CaseRecordType GetCaseRecordType()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.GetCaseRecordType(_instrumentName, _serverParkName);
        }

        public IDataSet GetDataSet()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.GetDataSet(_instrumentName, _serverParkName);
        }

        public bool KeyExists(IKey key)
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.KeyExists(key, _instrumentName, _serverParkName);
        }

        public IFluentBlaiseLocalApi WithFile(string filePath)
        {
            _serverParkName = null;
            _filePath = filePath;

            return this;
        }

        public IDataRecord GetDataRecord(IKey key)
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                return _blaiseApi.GetDataRecord(key, _filePath);
            }

            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.GetDataRecord(key, _instrumentName, _serverParkName);
            
        }

        public void WriteDataRecord(IDataRecord dataRecord)
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                _blaiseApi.WriteDataRecord(dataRecord, _filePath);

                return;
            }

            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            _blaiseApi.WriteDataRecord(dataRecord, _instrumentName, _serverParkName);            
        }

        public void CreateNewDataRecord(string primaryKeyValue, Dictionary<string, string> fieldData)
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            _blaiseApi.CreateNewDataRecord(primaryKeyValue, fieldData, _instrumentName, _serverParkName);
        }

        public bool CompletedFieldExists()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.CompletedFieldExists(_instrumentName, _serverParkName);
        }

        public void MarkCaseAsComplete(IDataRecord dataRecord)
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            _blaiseApi.MarkCaseAsComplete(dataRecord, _instrumentName, _serverParkName);
        }

        public bool ProcessedFieldExists()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.ProcessedFieldExists(_instrumentName, _serverParkName);
        }

        public void MarkCaseAsProcessed(IDataRecord dataRecord)
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            _blaiseApi.MarkCaseAsProcessed(dataRecord, _instrumentName, _serverParkName);
        }

        private void ValidateServerParkIsSet()
        {
            if (string.IsNullOrWhiteSpace(_serverParkName))
            {
                throw new NullReferenceException("The 'WithServerPark' step needs to be called prior to this");
            }
        }

        private void ValidateInstrumentIsSet()
        {
            if (string.IsNullOrWhiteSpace(_instrumentName))
            {
                throw new NullReferenceException("The 'ForInstrument' step needs to be called prior to this");
            }
        }
    }
}
