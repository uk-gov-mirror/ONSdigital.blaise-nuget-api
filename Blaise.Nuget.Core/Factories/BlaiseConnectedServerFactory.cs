
using Blaise.Nuget.Core.Interfaces;
using Blaise.Nuget.Core.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Core.Factories
{
    public class BlaiseConnectedServerFactory : IConnectedServerFactory
    {
        private readonly IPasswordService _passwordService;
        private readonly ConnectionModel _connectionModel;

        private IConnectedServer _connectedServer;

        public BlaiseConnectedServerFactory(
            ConnectionModel connectionModel,
            IPasswordService passwordService)
        {
            _connectionModel = connectionModel;
            _passwordService = passwordService;
        }
        
        public IConnectedServer GetConnection()
        {
            if(_connectedServer == null)
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
    }
}
