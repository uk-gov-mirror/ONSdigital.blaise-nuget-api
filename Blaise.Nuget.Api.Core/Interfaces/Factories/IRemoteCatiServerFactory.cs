using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.Cati.Runtime;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface IRemoteCatiServerFactory
    {
        IRemoteCatiManagementServer GetConnection(ConnectionModel connectionModel);
    }
}