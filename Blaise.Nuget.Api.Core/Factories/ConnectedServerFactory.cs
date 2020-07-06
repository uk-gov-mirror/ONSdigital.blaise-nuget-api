using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class ConnectedServerFactory : IConnectedServerFactory
    {
        private readonly IPasswordService _passwordService;
        private readonly ConnectionModel _connectionModel;
        private IConnectedServer _connectedServer;
         private DateTime _connectionExpiresOn;

        public ConnectedServerFactory(
            ConnectionModel connectionModel,
            IPasswordService passwordService)
        {
            _connectionModel = connectionModel;
            _passwordService = passwordService;

             _connectionExpiresOn = DateTime.Now.AddHours(1);
        }
        
        public IConnectedServer GetConnection()
        {
            if(_connectedServer == null || ConnectionHasExpired())
            {
                CreateServerConnection(_connectionModel);
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

        private bool ConnectionHasExpired()
        {
            return _connectionExpiresOn < DateTime.Now;
        }
    }
}
