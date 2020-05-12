using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces
{
    public interface IConnectedServerFactory
    {
        IConnectedServer GetConnection();
    }
}
