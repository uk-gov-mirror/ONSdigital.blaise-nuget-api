using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IRoleService
    {
        void AddRole(ConnectionModel connectionModel, string name, string description, IEnumerable<string> permissions);

        IEnumerable<IRole> GetRoles(ConnectionModel connectionModel);
    }
}