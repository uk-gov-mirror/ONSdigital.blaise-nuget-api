using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IParkService
    {
        IEnumerable<string> GetServerParkNames();

        bool ServerParkExists(string serverParkName);

        IEnumerable<string> GetSurveyNames(string serverParkName);

        Guid GetInstrumentId(string instrumentName, string serverParkName);
    }
}
