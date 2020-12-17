using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IRoleService
    {
        void AddRole(ConnectionModel connectionModel, string name, string description);
    }
}