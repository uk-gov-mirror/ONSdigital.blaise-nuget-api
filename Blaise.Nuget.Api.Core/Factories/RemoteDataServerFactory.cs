using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class RemoteDataServerFactory : IRemoteDataServerFactory
    {
        private readonly IPasswordService _passwordService;

        private readonly Dictionary<string, Tuple<IRemoteDataServer, DateTime>> _remoteDataServers;

        public RemoteDataServerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _remoteDataServers = new Dictionary<string, Tuple<IRemoteDataServer, DateTime>>();
        }

        public IRemoteDataServer GetConnection(ConnectionModel connectionModel)
        {
            if (_remoteDataServers.Any(c => c.Key == connectionModel.ServerName))
            {
                var existingConnection = _remoteDataServers.First(c => c.Key == connectionModel.ServerName);

                return existingConnection.Value.Item2.HasExpired()
                    ? GetFreshServerConnection(connectionModel)
                    : existingConnection.Value.Item1;
            }

            return GetFreshServerConnection(connectionModel);
        }

        private IRemoteDataServer GetFreshServerConnection(ConnectionModel connectionModel)
        {
            if (_remoteDataServers.Any(c => c.Key == connectionModel.ServerName))
            {
                _remoteDataServers.Remove(connectionModel.ServerName);
            }

            var remoteConnection = CreateRemoteConnection(connectionModel);

            _remoteDataServers.Add(connectionModel.ServerName, 
                new Tuple<IRemoteDataServer, DateTime>(remoteConnection, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate()));

            return remoteConnection;
        }

        private IRemoteDataServer CreateRemoteConnection(ConnectionModel connectionModel)
        {
            var securePassword = _passwordService.CreateSecurePassword(connectionModel.Password);

            return DataLinkManager.GetRemoteDataServer(
                connectionModel.ServerName,
                connectionModel.RemotePort,
                connectionModel.Binding,
                connectionModel.UserName,
                securePassword);
        }
    }
}
