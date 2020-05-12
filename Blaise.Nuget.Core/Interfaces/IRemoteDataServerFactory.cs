using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Core.Interfaces
{
    public interface IRemoteDataServerFactory
    {
        IRemoteDataServer GetConnection();
    }
}
