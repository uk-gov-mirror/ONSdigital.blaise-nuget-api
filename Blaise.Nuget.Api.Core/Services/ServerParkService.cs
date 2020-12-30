using System;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Services
{
    public class ServerParkService : IServerParkService
    {
        private readonly IConnectedServerFactory _connectionFactory;

        public ServerParkService(IConnectedServerFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<string> GetServerParkNames(ConnectionModel connectionModel)
        {
            var connection = _connectionFactory.GetConnection(connectionModel);
            var serverParks = connection.ServerParks;

            if (!serverParks.Any())
            {
                throw new DataNotFoundException("No server parks found");
            }

            return serverParks.Select(sp => sp.Name);
        }

        public bool ServerParkExists(ConnectionModel connectionModel, string serverParkName)
        {
            var serverParkNames = GetServerParkNames(connectionModel);

            return serverParkNames.Any(sp => sp.Equals(serverParkName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IServerPark GetServerPark(ConnectionModel connectionModel, string serverParkName)
        {
            var connection = _connectionFactory.GetConnection(connectionModel);

            if (!ServerParkExists(connectionModel, serverParkName))
            {
                throw new DataNotFoundException($"Server park '{serverParkName}' not found");
            }

            return connection.GetServerPark(serverParkName);
        }

        public IEnumerable<IServerPark> GetServerParks(ConnectionModel connectionModel)
        {
            var connection = _connectionFactory.GetConnection(connectionModel);
            var serverParks = connection.ServerParks;

            if (!serverParks.Any())
            {
                throw new DataNotFoundException("No server parks found");
            }

            return serverParks;
        }

        public void RegisterMachineOnServerPark(ConnectionModel connectionModel,
            string serverParkName, string machineName, string logicalRootName, IEnumerable<string> roles)
        {           
            var serverPark = (IServerPark3)GetServerPark(connectionModel, serverParkName);
            serverPark.AddMachine(
                machineName,
                logicalRootName,
                roles.ToArray(),
                connectionModel.Port);

            serverPark.Save();
        }
    }
}
