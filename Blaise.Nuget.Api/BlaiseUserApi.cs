using Blaise.Nuget.Api.Helpers;
using Blaise.Nuget.Api.Providers;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api
{
    public class BlaiseUserApi : IBlaiseUserApi
    {
        private readonly IUserService _userService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseUserApi(
            IUserService userService,
            ConnectionModel connectionModel)
        {
            _userService = userService;
            _connectionModel = connectionModel;
        }

        public BlaiseUserApi(ConnectionModel connectionModel = null)
        {
            var unityProvider = new UnityProvider();
            unityProvider.RegisterDependencies();

            //resolve dependencies
            _userService = unityProvider.Resolve<IUserService>();

            var configurationProvider = unityProvider.Resolve<IConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public void AddUser(string userName, string password, 
            string role, IList<string> serverParkNames, string defaultServerPark)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");
            password.ThrowExceptionIfNullOrEmpty("password");
            role.ThrowExceptionIfNullOrEmpty("role");
            defaultServerPark.ThrowExceptionIfNullOrEmpty("DefaultServerPark");

            _userService.AddUser(_connectionModel, userName, password, role, serverParkNames, defaultServerPark);
        }

        public void EditUser(string userName, string role, IList<string> serverParkNames)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");
            role.ThrowExceptionIfNullOrEmpty("role");

            _userService.EditUser(_connectionModel, userName, role, serverParkNames);
        }

        public void ChangePassword(string userName, string password)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");
            password.ThrowExceptionIfNullOrEmpty("password");

            _userService.ChangePassword(_connectionModel, userName, password);
        }

        public bool UserExists(string userName)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");

            return _userService.UserExists(_connectionModel, userName);
        }

        public void RemoveUser(string userName)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");

            _userService.RemoveUser(_connectionModel, userName);
        }

        public IUser GetUser(string userName)
        {
            userName.ThrowExceptionIfNullOrEmpty("userName");

            return _userService.GetUser(_connectionModel, userName);
        }
    }
}
