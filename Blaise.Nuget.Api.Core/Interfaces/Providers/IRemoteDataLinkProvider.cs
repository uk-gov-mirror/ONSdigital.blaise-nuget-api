
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IRemoteDataLinkProvider
    {
        IDataLink4 GetDataLink(string instrumentName, string serverParkName);
        IDataLink4 GetDataLink(string serverName, string instrumentName, string serverParkName);
    }
}
