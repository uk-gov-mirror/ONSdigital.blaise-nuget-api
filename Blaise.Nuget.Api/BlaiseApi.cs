using Blaise.Nuget.Api.Helpers;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using System.Collections.Generic;
using Unity;
using Unity.Injection;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Core.Services;
using Blaise.Nuget.Api.Core.Factories;
using StatNeth.Blaise.API.DataLink;
using Blaise.Nuget.Api.Core.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api
{ 
    public class BlaiseApi : IBlaiseApi
    {
        private readonly IDataService _dataService;
        private readonly IParkService _parkService;
        private readonly ISurveyService _surveyService;

        internal BlaiseApi(
            IDataService dataService,
            IParkService parkService, 
            ISurveyService surveyService)
        {
            _dataService = dataService;
            _parkService = parkService;
            _surveyService = surveyService;
        }

        public BlaiseApi()
        {
            var unityContainer = new UnityContainer();
            var configurationProvider = new ConfigurationProvider();

            //factories
            unityContainer.RegisterType<IPasswordService, PasswordService>();

            var connectionModel = configurationProvider.GetConnectionModel();
            unityContainer.RegisterSingleton<IConnectedServerFactory, ConnectedServerFactory>(
                new InjectionConstructor(connectionModel, unityContainer.Resolve<IPasswordService>()));

            var remoteConnectionModel = configurationProvider.GetRemoteConnectionModel();
            unityContainer.RegisterSingleton<IRemoteDataServerFactory, RemoteDataServerFactory>(
                new InjectionConstructor(remoteConnectionModel, unityContainer.Resolve<IPasswordService>()));

            //providers
            unityContainer.RegisterType<ILocalDataLinkProvider, LocalDataLinkProvider>();
            unityContainer.RegisterType<IRemoteDataLinkProvider, RemoteDataLinkProvider>();

            //services
            unityContainer.RegisterType<IDataService, DataService>();
            unityContainer.RegisterType<IDataRecordService, DataRecordService>();
            unityContainer.RegisterType<IDataModelService, DataModelService>();
            unityContainer.RegisterType<IKeyService, KeyService>();
            unityContainer.RegisterType<IFieldService, FieldService>();
            unityContainer.RegisterType<IParkService, ParkService>();
            unityContainer.RegisterType<ISurveyService, SurveyService>();
            
            //resolve dependencies
            _dataService = unityContainer.Resolve<IDataService>();
            _parkService = unityContainer.Resolve<IParkService>();
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

        public IKey GetKey(IDatamodel dataModel, string keyName)
        {
            dataModel.ThrowExceptionIfNull("dataModel");
            keyName.ThrowExceptionIfNullOrEmpty("keyName");

            return _dataService.GetKey(dataModel, keyName);
        }

        public bool KeyExists(IKey key, string instrumentName, string serverParkName)
        {
            key.ThrowExceptionIfNull("key");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dataService.KeyExists(key, instrumentName, serverParkName);
        }

        public string GetPrimaryKey(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _dataService.GetPrimaryKey(dataRecord);
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
    }
}
