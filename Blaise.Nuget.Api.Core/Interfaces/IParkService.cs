using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Core.Interfaces
{
    public interface IParkService
    {
        IEnumerable<string> GetServerParkNames();

        bool ServerParkExists(string serverParkName);

        Guid GetInstrumentId(string instrumentName, string serverParkName);
    }
}
