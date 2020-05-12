using Blaise.Nuget.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blaise.Nuget.Core.Services
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

            return serverParks.Select(sp => sp.Name);
        }

        public bool ServerParkExists(string serverParkName)
        {
            var serverParkNames = GetServerParkNames();

            return serverParkNames.Any(sp => sp == serverParkName);
        }

        public Guid GetInstrumentId(string instrumentName, string serverParkName)
        {
            var connection = _connectionFactory.GetConnection();
            var serverPark = connection.GetServerPark(serverParkName);
            var survey = serverPark.Surveys.FirstOrDefault(s => string.Equals(s.Name, instrumentName, StringComparison.OrdinalIgnoreCase));

            if (survey == null)
            {
                throw new ArgumentOutOfRangeException(string.Format("Instrument: {0} not found on server park: {1}", instrumentName, serverParkName));
            }

            return survey.InstrumentID;
        }
    }
}
