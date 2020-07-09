using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class ConnectedServerFactory : IConnectedServerFactory
    {
        private readonly IPasswordService _passwordService;
        private readonly IConnectionExpiryService _connectionExpiryService;

        private IConnectedServer _connectedServer;

        public ConnectedServerFactory(
            IPasswordService passwordService,
            IConnectionExpiryService connectionExpiryService)
        {
            _passwordService = passwordService;
            _connectionExpiryService = connectionExpiryService;
        }

        public IConnectedServer GetConnection(ConnectionModel connectionModel)
        {
            if (_connectedServer == null || _connectionExpiryService.ConnectionHasExpired())
            {
                CreateServerConnection(connectionModel);
                _connectionExpiryService.ResetConnectionExpiryPeriod();
            }

            return _connectedServer;
        }

        private void CreateServerConnection(ConnectionModel connectionModel)
        {
            _connectedServer = ServerManager.ConnectToServer(
                connectionModel.ServerName,
                connectionModel.Port,
                connectionModel.UserName,
                _passwordService.CreateSecurePassword(connectionModel.Password),
                connectionModel.Binding);
        }
    }
}
