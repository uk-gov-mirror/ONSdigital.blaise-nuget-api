using System.Collections.Generic;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IParkService
    {
        IEnumerable<string> GetServerParkNames();

        bool ServerParkExists(string serverParkName);

        IServerPark GetServerPark(string serverParkName);
    }
}
