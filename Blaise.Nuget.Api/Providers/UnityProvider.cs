namespace Blaise.Nuget.Api.Providers
{
    using System.ComponentModel;
    using Blaise.Nuget.Api.Core.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Mappers;
    using Blaise.Nuget.Api.Core.Interfaces.Providers;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using Blaise.Nuget.Api.Core.Mappers;
    using Blaise.Nuget.Api.Core.Providers;
    using Blaise.Nuget.Api.Core.Services;
    using Unity;
    using Unity.Interception;
    using Unity.Interception.ContainerIntegration;
    using Unity.Interception.Interceptors.InstanceInterceptors.InterfaceInterception;

    public static class UnityProvider
    {
        private static readonly UnityContainer UnityContainer;

        static UnityProvider()
        {
            UnityContainer = new UnityContainer();

            // Add interception support
            UnityContainer.AddNewExtension<Interception>();

            // configuration provider
            UnityContainer.RegisterSingleton<IBlaiseConfigurationProvider, BlaiseConfigurationProvider>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());

            // password service
            UnityContainer.RegisterType<IPasswordService, PasswordService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());


            // factories
            UnityContainer.RegisterSingleton<IConnectedServerFactory, ConnectedServerFactory>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterSingleton<IRemoteDataServerFactory, RemoteDataServerFactory>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<ICatiManagementServerFactory, CatiManagementServerFactory>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterSingleton<ISecurityManagerFactory, SecurityManagerFactory>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IDataInterfaceFactory, DataInterfaceFactory>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IAuditTrailManagerFactory, AuditTrailManagerFactory>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());

            // mappers
            UnityContainer.RegisterType<IDataRecordMapper, DataRecordMapper>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IRolePermissionMapper, RolePermissionMapper>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IAuditTrailDataMapper, AuditTrailDataMapper>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());

            // data link providers
            UnityContainer.RegisterType<ILocalDataLinkProvider, LocalDataLinkProvider>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterSingleton<IRemoteDataLinkProvider, RemoteDataLinkProvider>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IDataInterfaceProvider, DataInterfaceProvider>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IRemoteCatiManagementServerProvider, RemoteCatiManagementServerProvider>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());

            // services
            UnityContainer.RegisterType<IDataModelService, DataModelService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IDataRecordService, DataRecordService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<ICaseService, CaseService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IFieldService, FieldService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IKeyService, KeyService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IServerParkService, ServerParkService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IQuestionnaireService, QuestionnaireService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IUserService, UserService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IFileService, FileService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<ICatiService, CatiService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IRoleService, RoleService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IQuestionnaireMetaService, QuestionnaireMetaService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<ISqlService, SqlService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());
            UnityContainer.RegisterType<IAuditTrailService, AuditTrailService>(
                new Interceptor<InterfaceInterceptor>(),
                new InterceptionBehavior<LoggingInterceptionBehavior>());

            UnityContainer.RegisterType<LoggingInterceptionBehavior>();
        }

        public static T Resolve<T>()
        {
            return UnityContainer.Resolve<T>();
        }
    }
}
