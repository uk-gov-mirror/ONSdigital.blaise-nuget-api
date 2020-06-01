using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface IRemoteDataServerFactory
    {
        IRemoteDataServer GetConnection(string serverName = null);
    }
}
