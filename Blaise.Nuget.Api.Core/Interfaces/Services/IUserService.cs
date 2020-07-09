using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IUserService
    {
        void AddUser(ConnectionModel connectionModel, string userName, string password, string role, IEnumerable<string> serverParkNames);

        void EditUser(ConnectionModel connectionModel, string userName, string role, IEnumerable<string> serverParkNames);

        void ChangePassword(ConnectionModel connectionModel, string userName, string password);

        bool UserExists(ConnectionModel connectionModel, string userName);

        void RemoveUser(ConnectionModel connectionModel, string userName);
    }
}