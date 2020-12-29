using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Extensions;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseServerParkApi : IBlaiseServerParkApi
    {
        private readonly IServerParkService _parkService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseServerParkApi(
            IServerParkService parkService,
            ConnectionModel connectionModel)
        {
            _parkService = parkService;
            _connectionModel = connectionModel;
        }

        public BlaiseServerParkApi(ConnectionModel connectionModel = null)
        {
            var unityProvider = new UnityProvider();
            unityProvider.RegisterDependencies();

            _parkService = unityProvider.Resolve<IServerParkService>();

            var configurationProvider = unityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public IServerPark GetServerPark(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _parkService.GetServerPark(_connectionModel, serverParkName);
        }

        public IEnumerable<IServerPark> GetServerParks()
        {
            return _parkService.GetServerParks(_connectionModel);
        }

        public IEnumerable<string> GetNamesOfServerParks()
        {
            return _parkService.GetServerParkNames(_connectionModel);
        }

        public bool ServerParkExists(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _parkService.ServerParkExists(_connectionModel, serverParkName);
        }

        public void RegisterMachineOnServerPark(string serverParkName, string machineName,
            string logicalRootName, IList<string> roles)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            machineName.ThrowExceptionIfNullOrEmpty("machineName");
            logicalRootName.ThrowExceptionIfNullOrEmpty("logicalRootName");

            roles.ThrowExceptionIfNullOrEmpty("roles");

            _parkService.RegisterMachineOnServerPark(_connectionModel,
                serverParkName, machineName, logicalRootName, roles);
        }
    }
}
