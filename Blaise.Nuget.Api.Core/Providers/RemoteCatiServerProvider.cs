using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using StatNeth.Blaise.API.Cati.Runtime;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class RemoteCatiServerProvider : IRemoteCatiServerProvider
    {
        private readonly IRemoteCatiServerFactory _connectionFactory;
        
        public RemoteCatiServerProvider(IRemoteCatiServerFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IRemoteCatiManagementServer GetRemoteConnection(ConnectionModel connectionModel)
        {
            return _connectionFactory.GetConnection(connectionModel);
        }
    }
}
