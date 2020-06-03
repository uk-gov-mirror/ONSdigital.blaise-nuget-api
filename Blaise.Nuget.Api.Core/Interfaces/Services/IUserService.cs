using System.Collections.Generic;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IUserService
    {
        void AddUser(string userName, string password, string role, IEnumerable<string> serverParkNames);

        void ChangePassword(string userName, string password);

        bool UserExists(string userName);
    }
}