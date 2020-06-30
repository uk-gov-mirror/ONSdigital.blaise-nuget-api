using System;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class RemoteDataServerFactory : IRemoteDataServerFactory
    {
        private readonly IPasswordService _passwordService;
        private readonly ConnectionModel _connectionModel;
        private DateTime _dataLinkExpiresOn;

        private IRemoteDataServer _remoteDataServer;

        public RemoteDataServerFactory(
            ConnectionModel connectionModel,
            IPasswordService passwordService)
        {
            _connectionModel = connectionModel;
            _passwordService = passwordService;

            _dataLinkExpiresOn = DateTime.Now.AddHours(1);
        }

        public IRemoteDataServer GetConnection()
        {   
            if(_remoteDataServer == null || DataLinkHasExpired())
            {
                CreateRemoteConnection(_connectionModel);
                _dataLinkExpiresOn = DateTime.Now.AddHours(1);
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

        private bool DataLinkHasExpired()
        {
            return _dataLinkExpiresOn < DateTime.Now;
        }
    }
}
