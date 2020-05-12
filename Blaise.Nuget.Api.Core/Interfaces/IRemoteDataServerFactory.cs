using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Interfaces
{
    public interface IRemoteDataServerFactory
    {
        IRemoteDataServer GetConnection();
    }
}
