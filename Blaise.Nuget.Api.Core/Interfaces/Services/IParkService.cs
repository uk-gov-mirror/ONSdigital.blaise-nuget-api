using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IParkService
    {
        IEnumerable<string> GetServerParkNames();

        bool ServerParkExists(string serverParkName);

        IEnumerable<string> GetSurveys(string serverParkName);

        Guid GetInstrumentId(string instrumentName, string serverParkName);
    }
}
