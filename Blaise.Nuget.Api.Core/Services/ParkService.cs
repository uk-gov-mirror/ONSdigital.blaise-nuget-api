using System;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;
using System.Collections.Generic;
using System.Linq;

namespace Blaise.Nuget.Api.Core.Services
{
    public class ParkService : IParkService
    {
        private readonly IConnectedServerFactory _connectionFactory;

        public ParkService(IConnectedServerFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<string> GetServerParkNames()
        {
            var connection = _connectionFactory.GetConnection();
            var serverParks = connection.ServerParks;

            if (!serverParks.Any())
            {
                throw new DataNotFoundException("No server parks found");
            }

            return serverParks.Select(sp => sp.Name);
        }

        public bool ServerParkExists(string serverParkName)
        {
            var serverParkNames = GetServerParkNames();

            return serverParkNames.Any(sp => sp.Equals(serverParkName, StringComparison.OrdinalIgnoreCase));
        }

        public IServerPark GetServerPark(string serverParkName)
        {
            var connection = _connectionFactory.GetConnection();

            if(!ServerParkExists(serverParkName))
            {
                throw new DataNotFoundException($"Server park '{serverParkName}' not found");
            }

            return connection.GetServerPark(serverParkName);
        }
    }
}
