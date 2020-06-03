using System.Collections.Generic;
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

            foreach (var serverParkName in serverParkNames)
            {
                user.ServerParks.Add(serverParkName);
            }

            try
            {
                user.Role = role; // Try to update the user's role. If an error is thrown leave it blank.
            }
            catch {}

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
    }
}
