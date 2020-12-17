using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;

namespace Blaise.Nuget.Api.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly ISecurityManagerFactory _securityManagerFactory;

        public RoleService(ISecurityManagerFactory securityManagerFactory)
        {
            _securityManagerFactory = securityManagerFactory;
        }

        public void AddRole(ConnectionModel connectionModel, string name, string description)
        {
            var connection = _securityManagerFactory.GetConnection(connectionModel);
            connection.AddRole(name, description);
        }
    }
}
