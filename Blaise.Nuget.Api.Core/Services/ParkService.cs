using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Core.Interfaces;
using StatNeth.Blaise.API.ServerManager;
using System;
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

            return serverParkNames.Any(sp => sp == serverParkName);
        }

        public IEnumerable<string> GetSurveys(string serverParkName)
        {
            var serverPark = GetServerPark(serverParkName);

            if(!serverPark.Surveys.Any())
            {
                throw new DataNotFoundException($"No surveys found for server park '{serverParkName}'");
            }

            return serverPark.Surveys.Select(sp => sp.Name);
        }

        public Guid GetInstrumentId(string instrumentName, string serverParkName)
        {
            var serverPark = GetServerPark(serverParkName);
            var survey = serverPark.Surveys.FirstOrDefault(s => string.Equals(s.Name, instrumentName, StringComparison.OrdinalIgnoreCase));

            if (survey == null)
            {
                throw new DataNotFoundException($"Instrument '{instrumentName}' not found on server park '{serverParkName}'");
            }

            return survey.InstrumentID;
        }

        private IServerPark GetServerPark(string serverParkName)
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
