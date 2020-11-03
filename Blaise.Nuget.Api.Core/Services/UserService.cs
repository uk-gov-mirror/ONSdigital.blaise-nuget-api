using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
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

        public void AddUser(ConnectionModel connectionModel, string userName, string password, string role, IEnumerable<string> serverParkNames, string defaultServerPark)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);
            var securePassword = _passwordService.CreateSecurePassword(password);
            var user = (IUser2)connection.AddUser(userName, securePassword);
            
            AddCatiPreferenceToUser(user, defaultServerPark);
            AddServerParksToUser(user, serverParkNames);
            AssignRoleToUser(user, role);
            
            user.Save();
        }

        public void EditUser(ConnectionModel connectionModel, string userName, string role, IEnumerable<string> serverParkNames)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);
            var user = (IUser2)connection.Users.GetItem(userName);
            

            user.ServerParks.Clear();

            AddServerParksToUser(user, serverParkNames);
            AssignRoleToUser(user, role);

            user.Save();
        }

        public void ChangePassword(ConnectionModel connectionModel, string userName, string password)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);
            var securePassword = _passwordService.CreateSecurePassword(password);

            var user = (IUser2)connection.Users.GetItem(userName);

            user.ChangePassword(securePassword);
            user.Save();
        }

        public bool UserExists(ConnectionModel connectionModel, string userName)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);

            return connection.Users.Any(u => u.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        public void RemoveUser(ConnectionModel connectionModel, string userName)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);

            connection.RemoveUser(userName);
        }

        public IUser GetUser(ConnectionModel connectionModel, string userName)
        {
            var connection = _connectedServerFactory.GetConnection(connectionModel);

            return connection.Users.FirstOrDefault(u => u.Name.Equals(userName, StringComparison.OrdinalIgnoreCase));
        }

        private static void AddServerParksToUser(IUser user, IEnumerable<string> serverParkNames)
        {
            foreach (var serverParkName in serverParkNames)
            {
                user.ServerParks.Add(serverParkName);
            }
        }

        private static void AssignRoleToUser(IUser2 user, string role)
        {
            user.Role = role;
        }

        private static void AddCatiPreferenceToUser(IUser2 user, string defaultServerPark)
        {
            user.Preferences.Add("CATI.Preferences");
            var catiPreference = user.Preferences.GetItem("CATI.Preferences");
            catiPreference.Value = $"<CatiDashboard><ServerPark>{defaultServerPark}</ServerPark></CatiDashboard>";
        }
    }
}
