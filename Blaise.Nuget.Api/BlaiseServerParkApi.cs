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
    public class BlaiseServerParkApi : IBlaiseServerParkApi
    {
        private readonly IParkService _parkService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseServerParkApi(
            IParkService parkService,
            ConnectionModel connectionModel)
        {
            _parkService = parkService;
            _connectionModel = connectionModel;
        }

        public BlaiseServerParkApi(ConnectionModel connectionModel = null)
        {
            var unityProvider = new UnityProvider();
            unityProvider.RegisterDependencies();

            _parkService = unityProvider.Resolve<IParkService>();

            var configurationProvider = unityProvider.Resolve<IConfigurationProvider>();
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
    }
}
