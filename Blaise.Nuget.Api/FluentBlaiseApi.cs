using Blaise.Nuget.Api.Providers;
using Blaise.Nuget.Contracts.Interfaces;
using Blaise.Nuget.Core.Factories;
using Blaise.Nuget.Core.Interfaces;
using Blaise.Nuget.Core.Services;
using Unity;
using Unity.Injection;

namespace Blaise.Nuget.Api
{ 
    public class FluentBlaiseApi : IFluentBlaiseApi
    {
        private readonly IDataService _dataService;
        private readonly IDataManagerService _dataManagerService;
        private readonly IDataLinkService _dataLinkService;
        private readonly IParkService _parkService;

        internal FluentBlaiseApi(
            IDataService dataService,
            IDataManagerService dataManagerService,
            IDataLinkService dataLinkService,
            IParkService parkService)
        {
            _dataService = dataService;
            _dataManagerService = dataManagerService;
            _dataLinkService = dataLinkService;
            _parkService = parkService;
        }

        public FluentBlaiseApi()
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
            _dataManagerService = unityContainer.Resolve<IDataManagerService>();
            _dataLinkService = unityContainer.Resolve<IDataLinkService>();
            _parkService = unityContainer.Resolve<IParkService>();
        }
    }
}
