using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class ConnectedServerFactory : IConnectedServerFactory
    {
        private readonly IPasswordService _passwordService;

        private readonly Dictionary<string, Tuple<IConnectedServer, DateTime>> _connectedServers;

        public ConnectedServerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _connectedServers = new Dictionary<string, Tuple<IConnectedServer, DateTime>>();
        }

        public IConnectedServer GetConnection(ConnectionModel connectionModel)
        {
            if (_connectedServers.Any(c => c.Key == connectionModel.ServerName))
            {
                var existingConnection = _connectedServers.First(c => c.Key == connectionModel.ServerName);

                return existingConnection.Value.Item2.HasExpired()
                    ? GetFreshServerConnection(connectionModel)
                    : existingConnection.Value.Item1;
            }

            return GetFreshServerConnection(connectionModel);
        }

        private IConnectedServer GetFreshServerConnection(ConnectionModel connectionModel)
        {
            if (_connectedServers.Any(c => c.Key == connectionModel.ServerName))
            {
                _connectedServers.Remove(connectionModel.ServerName);
            }

            var connectedServer = CreateServerConnection(connectionModel);

            _connectedServers.Add(connectionModel.ServerName,
                new Tuple<IConnectedServer, DateTime>(connectedServer, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate()));

            return connectedServer;
        }

        private IConnectedServer CreateServerConnection(ConnectionModel connectionModel)
        {
            return ServerManager.ConnectToServer(
                connectionModel.ServerName,
                connectionModel.Port,
                connectionModel.UserName,
                _passwordService.CreateSecurePassword(connectionModel.Password),
                connectionModel.Binding);
        }
    }
}
