
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Mappers;
using Blaise.Nuget.Api.Core.Providers;
using Blaise.Nuget.Api.Core.Services;
using Blaise.Nuget.Api.Interfaces;
using Unity;
using Unity.Injection;

namespace Blaise.Nuget.Api.Providers
{
    public class UnityProvider : IIocProvider
    {
        private UnityContainer _unityContainer;

        public void RegisterDependencies(ConnectionModel connectionModel)
        {
            _unityContainer = new UnityContainer();

            //password service
            _unityContainer.RegisterType<IPasswordService, PasswordService>();

            //factories
            _unityContainer.RegisterSingleton<IConnectedServerFactory, ConnectedServerFactory>(
                new InjectionConstructor(connectionModel, _unityContainer.Resolve<IPasswordService>()));

            _unityContainer.RegisterSingleton<IRemoteDataServerFactory, RemoteDataServerFactory>(
                new InjectionConstructor(connectionModel, _unityContainer.Resolve<IPasswordService>()));

            //mappers
            _unityContainer.RegisterType<IDataMapperService, DataMapperService>();

            //providers
            _unityContainer.RegisterType<ILocalDataLinkProvider, LocalDataLinkProvider>();
            _unityContainer.RegisterType<IRemoteDataLinkProvider, RemoteDataLinkProvider>();
            _unityContainer.RegisterType<IConfigurationProvider, ConfigurationProvider>();

            //services
            _unityContainer.RegisterType<IDataModelService, DataModelService>();
            _unityContainer.RegisterType<IDataRecordService, DataRecordService>();
            _unityContainer.RegisterType<IDataService, DataService>();
            _unityContainer.RegisterType<IFieldService, FieldService>();
            _unityContainer.RegisterType<IKeyService, KeyService>();
            _unityContainer.RegisterType<IParkService, ParkService>();
            _unityContainer.RegisterType<ISurveyService, SurveyService>();
            _unityContainer.RegisterType<IUserService, UserService>();
            _unityContainer.RegisterType<IFileService, FileService>();
        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }
    }
}
