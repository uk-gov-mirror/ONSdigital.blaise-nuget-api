using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Core.Interfaces
{
    public interface IConnectedServerFactory
    {
        IConnectedServer GetConnection();
    }
}
