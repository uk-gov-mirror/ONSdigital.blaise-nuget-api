using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly ISecurityManagerFactory _securityManagerFactory;
        private readonly IRolePermissionMapper _mapper;

        public RoleService(
            ISecurityManagerFactory securityManagerFactory, 
            IRolePermissionMapper mapper)
        {
            _securityManagerFactory = securityManagerFactory;
            _mapper = mapper;
        }

        public void AddRole(ConnectionModel connectionModel, string name, string description, IEnumerable<string> permissions)
        {
            var connection = _securityManagerFactory.GetConnection(connectionModel);
            var roleId = connection.AddRole(name, description);

            var actionPermissions = _mapper.MapToActionPermissionModels(permissions);

            connection.UpdateRolePermissions(roleId, actionPermissions);
        }

        public IEnumerable<IRole> GetRoles(ConnectionModel connectionModel)
        {
            var connection = _securityManagerFactory.GetConnection(connectionModel);

            return connection.GetRoles();
        }
    }
}
