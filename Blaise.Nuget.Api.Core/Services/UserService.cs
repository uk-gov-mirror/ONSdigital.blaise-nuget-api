using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IConnectedServerFactory _connectedServerFactory;
        private readonly IPasswordService _passwordService;

        public UserService(
            IConnectedServerFactory connectedServerFactory, 
            IPasswordService passwordService)
        {
            _connectedServerFactory = connectedServerFactory;
            _passwordService = passwordService;
        }

        public void AddUser(string userName, string password, string role, IEnumerable<string> serverParkNames)
        {
            var connection = _connectedServerFactory.GetConnection();
            var securePassword = _passwordService.CreateSecurePassword(password);
            var user = (IUser2)connection.AddUser(userName, securePassword);

            AddServerParksToUser(user, serverParkNames);
            AssignRoleToUser(user, role);

            user.Save();
        }

        public void EditUser(string userName, string role, IEnumerable<string> serverParkNames)
        {
            var connection = _connectedServerFactory.GetConnection();
            var user = (IUser2)connection.Users.GetItem(userName);

            user.ServerParks.Clear();

            AddServerParksToUser(user, serverParkNames);
            AssignRoleToUser(user, role);

            user.Save();
        }

        public void ChangePassword(string userName, string password)
        {
            var connection = _connectedServerFactory.GetConnection();
            var securePassword = _passwordService.CreateSecurePassword(password);

            var user = (IUser2)connection.Users.GetItem(userName);

            user.ChangePassword(securePassword);
            user.Save();
        }

        public bool UserExists(string userName)
        {
            var connection = _connectedServerFactory.GetConnection();

            return connection.Users.Any(u => u.Name == userName);
        }

        public void RemoveUser(string userName)
        {
            var connection = _connectedServerFactory.GetConnection();

            connection.RemoveUser(userName);
        }

        private static void AddServerParksToUser(IUser user, IEnumerable<string> serverParkNames)
        {
            foreach (var serverParkName in serverParkNames)
            {
                user.ServerParks.Add(serverParkName);
            }
        }

        private void AssignRoleToUser(IUser2 user, string role)
        {
            try
            {
                user.Role = role; // Try to update the user's role. If an error is thrown leave it blank.
            }
            catch { }
        }
    }
}
