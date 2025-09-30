namespace Blaise.Nuget.Api.Core.Factories
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Blaise.Nuget.Api.Contracts.Models;
    using Blaise.Nuget.Api.Core.Extensions;
    using Blaise.Nuget.Api.Core.Interfaces.Factories;
    using Blaise.Nuget.Api.Core.Interfaces.Services;
    using StatNeth.Blaise.API.ServerManager;

    public class ConnectedServerFactory : IConnectedServerFactory
    {
        private readonly IPasswordService _passwordService;

        private readonly ConcurrentDictionary<string, ConnectedServerEntry> _connections;

        public ConnectedServerFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _connections = new ConcurrentDictionary<string, ConnectedServerEntry>(StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public IConnectedServer GetConnection(ConnectionModel connectionModel)
        {
            var entry = _connections.AddOrUpdate(
                connectionModel.ServerName,
                key =>
                {
                    var connectedServer = CreateServerConnection(connectionModel);
                    return new ConnectedServerEntry(
                        connectedServer,
                        connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());
                },
                (key, existingEntry) =>
                {
                    if (existingEntry.ConnectedServer != null && !existingEntry.ExpiryDate.HasExpired())
                    {
                        return existingEntry;
                    }
                    var newConnectedServer = CreateServerConnection(connectionModel);
                    var newExpiryDate = connectionModel.ConnectionExpiresInMinutes.GetExpiryDate();
                    return new ConnectedServerEntry(newConnectedServer, newExpiryDate);
                });

            return entry.ConnectedServer;
        }

        /// <inheritdoc/>
        public IConnectedServer GetIsolatedConnection(ConnectionModel connectionModel)
        {
            return CreateServerConnection(connectionModel);
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

    public class ConnectedServerEntry
    {
        public IConnectedServer ConnectedServer { get; }

        public DateTime ExpiryDate { get; }

        public ConnectedServerEntry(IConnectedServer connectedServer, DateTime expiryDate)
        {
            ConnectedServer = connectedServer;
            ExpiryDate = expiryDate;
        }
    }
}
