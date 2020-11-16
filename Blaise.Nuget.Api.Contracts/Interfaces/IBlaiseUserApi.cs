using System.Collections.Generic;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseUserApi
    {
        void AddUser(string userName, string password, 
            string role, IList<string> serverParkNames, string defaultServerPark);

        void EditUser(string userName, string role, 
            IList<string> serverParkNames);

        void ChangePassword(string userName, string password);
        bool UserExists(string userName);
        void RemoveUser(string userName);
        IUser GetUser(string userName);
    }
}