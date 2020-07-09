using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class RemoteDataServerFactory : IRemoteDataServerFactory
    {
        private readonly IPasswordService _passwordService;
        private readonly IConnectionExpiryService _connectionExpiryService;

        private IRemoteDataServer _remoteDataServer;

        public RemoteDataServerFactory(
            IPasswordService passwordService, 
            IConnectionExpiryService connectionExpiryService)
        {
            _passwordService = passwordService;
            _connectionExpiryService = connectionExpiryService;
        }

        public IRemoteDataServer GetConnection(ConnectionModel connectionModel)
        {   
            if(_remoteDataServer == null || _connectionExpiryService.ConnectionHasExpired())
            {
                CreateRemoteConnection(connectionModel);
                _connectionExpiryService.ResetConnectionExpiryPeriod();
            }

            return _remoteDataServer;
        }

        private void CreateRemoteConnection(ConnectionModel connectionModel)
        {
            var securePassword = _passwordService.CreateSecurePassword(connectionModel.Password);

            _remoteDataServer = DataLinkManager.GetRemoteDataServer(
                connectionModel.ServerName,
                connectionModel.RemotePort,
                connectionModel.Binding,
                connectionModel.UserName,
                securePassword);
        }
    }
}
