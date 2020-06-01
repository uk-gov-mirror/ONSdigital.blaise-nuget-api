using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface IConnectedServerFactory
    {
        IConnectedServer GetConnection(string serverName = null);
    }
}
