using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Helpers;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using System.Collections.Generic;
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

        public bool ServerParkExists(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _blaiseApi.ServerParkExists(serverParkName);
        }

        public IKey GetKey(IDatamodel dataModel, string keyName)
        {
            dataModel.ThrowExceptionIfNull("dataModel");
            keyName.ThrowExceptionIfNullOrEmpty("keyName");

            return _blaiseApi.GetKey(dataModel, keyName);
        }

        public IDataRecord GetDataRecord(IDatamodel dataModel)
        {
            dataModel.ThrowExceptionIfNull("dataModel");

            return _blaiseApi.GetDataRecord(dataModel);
        }

        public IFluentBlaiseRemoteApi WithServerPark(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _filePath = null;
            _serverParkName = serverParkName;

            return this;
        }

        public IEnumerable<string> GetSurveyNames()
        {
            ValidateServerParkIsSet();

            return _blaiseApi.GetSurveyNames(_serverParkName);
        }

        public IFluentBlaiseRemoteApi ForInstrument(string instrumentName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");

            _filePath = null;
            _instrumentName = instrumentName;

            return this;
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

        public IDataSet GetDataSet()
        {
            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.GetDataSet(_instrumentName, _serverParkName);
        }

        public bool KeyExists(IKey key)
        {
            key.ThrowExceptionIfNull("key");

            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            return _blaiseApi.KeyExists(key, _instrumentName, _serverParkName);
        }

        public IFluentBlaiseLocalApi WithFile(string filePath)
        {
            filePath.ThrowExceptionIfNullOrEmpty("filePath");

            _serverParkName = null;
            _filePath = filePath;

            return this;
        }

        public IDataRecord GetDataRecord(IKey key)
        {
            key.ThrowExceptionIfNull("key");

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
            dataRecord.ThrowExceptionIfNull("dataRecord");

            if (!string.IsNullOrEmpty(_filePath))
            {
                _blaiseApi.WriteDataRecord(dataRecord, _filePath);

                return;
            }

            ValidateServerParkIsSet();
            ValidateInstrumentIsSet();

            _blaiseApi.WriteDataRecord(dataRecord, _instrumentName, _serverParkName);            
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
