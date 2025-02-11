﻿using Blaise.Nuget.Api.Core.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Mappers;
using Blaise.Nuget.Api.Core.Providers;
using Blaise.Nuget.Api.Core.Services;
using Blaise.Nuget.Api.Interfaces;
using Unity;

namespace Blaise.Nuget.Api.Providers
{
    public class UnityProvider : IIocProvider
    {
        private UnityContainer _unityContainer;

        public void RegisterDependencies()
        {
            _unityContainer = new UnityContainer();

            // configuration provider
            _unityContainer.RegisterSingleton<IBlaiseConfigurationProvider, BlaiseConfigurationProvider>();

            //password service
            _unityContainer.RegisterType<IPasswordService, PasswordService>();

            //factories
            _unityContainer.RegisterSingleton<IConnectedServerFactory, ConnectedServerFactory>();
            _unityContainer.RegisterSingleton<IRemoteDataServerFactory, RemoteDataServerFactory>();
            _unityContainer.RegisterSingleton<ICatiManagementServerFactory, CatiManagementServerFactory>();
            _unityContainer.RegisterSingleton<ISecurityManagerFactory, SecurityManagerFactory>();
            _unityContainer.RegisterSingleton<IDataInterfaceFactory, DataInterfaceFactory>();

            //mappers
            _unityContainer.RegisterType<IDataMapperService, DataMapperService>();
            _unityContainer.RegisterType<IRolePermissionMapper, RolePermissionMapper>();

            //data link providers
            _unityContainer.RegisterSingleton<ILocalDataLinkProvider, LocalDataLinkProvider>();
            _unityContainer.RegisterSingleton<IRemoteDataLinkProvider, RemoteDataLinkProvider>();
            _unityContainer.RegisterType<IDataInterfaceProvider, DataInterfaceProvider>();
            _unityContainer.RegisterType<IRemoteCatiManagementServerProvider, RemoteCatiManagementServerProvider>();

            //services
            _unityContainer.RegisterType<IDataModelService, DataModelService>();
            _unityContainer.RegisterType<IDataRecordService, DataRecordService>();
            _unityContainer.RegisterType<ICaseService, CaseService>();
            _unityContainer.RegisterType<IFieldService, FieldService>();
            _unityContainer.RegisterType<IKeyService, KeyService>();
            _unityContainer.RegisterType<IServerParkService, ServerParkService>();
            _unityContainer.RegisterType<ISurveyService, SurveyService>();
            _unityContainer.RegisterType<IUserService, UserService>();
            _unityContainer.RegisterType<IFileService, FileService>();
            _unityContainer.RegisterType<ICatiService, CatiService>();
            _unityContainer.RegisterType<IRoleService, RoleService>();
        }

        public T Resolve<T>()
        {
            return _unityContainer.Resolve<T>();
        }
    }
}
