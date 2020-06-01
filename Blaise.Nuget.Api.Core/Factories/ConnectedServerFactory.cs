using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class ConnectedServerFactory : IConnectedServerFactory
    {
        private readonly IPasswordService _passwordService;
        private readonly IConfigurationProvider _configurationProvider;

        private IConnectedServer _connectedServer;

        public ConnectedServerFactory(
            IConfigurationProvider configurationProvider,
            IPasswordService passwordService)
        {
            _configurationProvider = configurationProvider;
            _passwordService = passwordService;
        }
        
        public IConnectedServer GetConnection(string serverName = null)
        {
            if(_connectedServer == null)
            {
                CreateServerConnection(serverName);
            }

            return _connectedServer;
        }

        private void CreateServerConnection(string serverName = null)
        {
            _connectedServer = ServerManager.ConnectToServer(
                serverName ?? _configurationProvider.ServerName,
                _configurationProvider.ConnectionPort,
                _configurationProvider.UserName,
                _passwordService.CreateSecurePassword(_configurationProvider.Password),
                _configurationProvider.Binding);
        }
    }
}
