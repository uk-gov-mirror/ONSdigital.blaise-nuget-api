using Blaise.Nuget.Api.Helpers;
using Blaise.Nuget.Api.Providers;
using Blaise.Nuget.Contracts.Interfaces;
using Blaise.Nuget.Core.Factories;
using Blaise.Nuget.Core.Interfaces;
using Blaise.Nuget.Core.Services;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using System.Collections.Generic;
using Unity;
using Unity.Injection;

namespace Blaise.Nuget.Api
{ 
    public class BlaiseApi : IBlaiseApi
    {
        private readonly IDataService _dataService;
        private readonly IParkService _parkService;

        internal BlaiseApi(
            IDataService dataService,
            IParkService parkService)
        {
            _dataService = dataService;
            _parkService = parkService;
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

            //services
            unityContainer.RegisterType<IDataService, DataService>();
            unityContainer.RegisterType<IDataManagerService, DataManagerService>();
            unityContainer.RegisterType<IDataLinkService, DataLinkService>();
            unityContainer.RegisterType<IParkService, ParkService>();

            //resolve dependencies
            _dataService = unityContainer.Resolve<IDataService>();
            _parkService = unityContainer.Resolve<IParkService>();
        }

        public IEnumerable<string> GetServerParkNames()
        {
            return _parkService.GetServerParkNames();
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

            return _parkService.GetInstrumentId(instrumentName, serverParkName);
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

        public IDataRecord GetDataRecord(IDatamodel dataModel)
        {
            dataModel.ThrowExceptionIfNull("dataModel");

            return _dataService.GetDataRecord(dataModel);
        }

        public void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dataService.WriteDataRecord(dataRecord, instrumentName, serverParkName);
        }
    }
}
