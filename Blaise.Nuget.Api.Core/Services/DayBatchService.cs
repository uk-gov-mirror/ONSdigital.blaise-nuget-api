using System;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DayBatchService : IDayBatchService
    {
        private readonly IRemoteCatiServerProvider _catiServerProvider;

        public DayBatchService(IRemoteCatiServerProvider catiServerProvider)
        {
            _catiServerProvider = catiServerProvider;
        }

        public void CreateDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime dayBatchDate)
        {
            var catiManagement = _catiServerProvider.GetRemoteConnection(connectionModel);

            catiManagement.SelectServerPark(serverParkName);
            catiManagement.LoadCatiInstrumentManager(instrumentName).CreateDaybatch(dayBatchDate);
        }
    }
}
