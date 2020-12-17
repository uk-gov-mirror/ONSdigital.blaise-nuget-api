using System.Collections.Generic;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseRoleApi
    {
        void AddRole(string name, string description, IEnumerable<string> permissions);

        IEnumerable<IRole> GetRoles();
    }
}