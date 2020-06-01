using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class RemoteDataServerFactory : IRemoteDataServerFactory
    {
        private readonly IPasswordService _passwordService;
        private readonly IConfigurationProvider _configurationProvider;

        private IRemoteDataServer _remoteDataServer;

        public RemoteDataServerFactory(
            IConfigurationProvider configurationProvider,
            IPasswordService passwordService)
        {
            _configurationProvider = configurationProvider;
            _passwordService = passwordService;
        }

        public IRemoteDataServer GetConnection(string serverName = null)
        {   
            if(_remoteDataServer == null)
            {
                CreateRemoteConnection(serverName);
            }

            return _remoteDataServer;
        }

        private void CreateRemoteConnection(string serverName = null)
        {
            _remoteDataServer = DataLinkManager.GetRemoteDataServer(
                serverName ?? _configurationProvider.ServerName,
                _configurationProvider.RemoteConnectionPort,
                _configurationProvider.Binding,
                _configurationProvider.UserName,
                _passwordService.CreateSecurePassword(_configurationProvider.Password));
        }
    }
}
