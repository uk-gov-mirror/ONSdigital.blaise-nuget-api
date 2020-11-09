
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface ILocalDataLinkProvider
    {
        IDataLink4 GetDataLink(string databaseFile);
    }
}
