using Blaise.Nuget.Api.Contracts.Interfaces;
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
        private IKey _key;

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
            return _blaiseApi.ServerParkExists(serverParkName);
        }

        public IEnumerable<string> GetSurveys()
        {
            //check step
            return _blaiseApi.GetSurveys(_serverParkName);
        }

        public IKey GetKey(IDatamodel dataModel, string keyName)
        {
            return _blaiseApi.GetKey(dataModel, keyName);
        }

        public IFluentBlaiseLocalApi ForFile(string filePath)
        {
            _serverParkName = null;
            _filePath = filePath;

            return this;
        }

        public IFluentBlaiseRemoteApi ForServerPark(string serverParkName)
        {
            _filePath = null;
            _serverParkName = serverParkName;

            return this;

        }
        public IFluentBlaiseRemoteApi ForInstrument(string instrumentName)
        {
            _filePath = null;
            _instrumentName = instrumentName;

            return this;
        }

        public Guid GetInstrumentId()
        {
            ValidateServerParksSet();
            ValidateInstrumentSet();

            return _blaiseApi.GetInstrumentId(_instrumentName, _serverParkName);
        }

        public IDatamodel GetDataModel()
        {
            ValidateServerParksSet();
            ValidateInstrumentSet();

            return _blaiseApi.GetDataModel(_instrumentName, _serverParkName);
        }

        public IDataSet GetDataSet()
        {
            ValidateServerParksSet();
            ValidateInstrumentSet();

            return _blaiseApi.GetDataSet(_instrumentName, _serverParkName);
        }

        public bool KeyExists(IKey key)
        {
            ValidateServerParksSet();
            ValidateInstrumentSet();

            return _blaiseApi.KeyExists(key, _instrumentName, _serverParkName);
        }

        public IDataRecord GetDataRecord(IDatamodel dataModel)
        {
            return _blaiseApi.GetDataRecord(dataModel);
        }

        public IDataRecord GetDataRecord(IKey key)
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                return _blaiseApi.GetDataRecord(key, _filePath);
            }

            ValidateServerParksSet();
            ValidateInstrumentSet();

            return _blaiseApi.GetDataRecord(key, _instrumentName, _serverParkName);
            
        }

        public void WriteDataRecord(IDataRecord dataRecord)
        {
            if (!string.IsNullOrEmpty(_filePath))
            {
                _blaiseApi.WriteDataRecord(dataRecord, _filePath);
            }

            ValidateServerParksSet();
            ValidateInstrumentSet();

            _blaiseApi.WriteDataRecord(dataRecord, _instrumentName, _serverParkName);

            
        }

        private void ValidateFilePathIsSet()
        {
            if (string.IsNullOrWhiteSpace(_filePath))
            {
                throw new NullReferenceException("The 'ForFile' step needs to be called prior to this");
            }
        }

        private void ValidateServerParksSet()
        {
            if (string.IsNullOrWhiteSpace(_serverParkName))
            {
                throw new NullReferenceException("The 'ForServerPark' step needs to be called prior to this");
            }
        }

        private void ValidateInstrumentSet()
        {
            if (string.IsNullOrWhiteSpace(_instrumentName))
            {
                throw new NullReferenceException("The 'ForInstrument' step needs to be called prior to this");
            }
        }
    }
}
