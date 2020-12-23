using System;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.Administer.Deploy.ServiceContract;
using StatNeth.Blaise.Administer.Deploy.IServiceContract;
using StatNeth.Blaise.Security.ClientProxy;

namespace Blaise.Nuget.Api.Core.Services
{
    public class ParkService : IParkService
    {
        private readonly IConnectedServerFactory _connectionFactory;

        public ParkService(IConnectedServerFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public IEnumerable<string> GetServerParkNames(ConnectionModel connectionModel)
        {
            var connection = _connectionFactory.GetConnection(connectionModel);
            var serverParks = connection.ServerParks;

            if (!serverParks.Any())
            {
                throw new DataNotFoundException("No server parks found");
            }

            return serverParks.Select(sp => sp.Name);
        }

        public bool ServerParkExists(ConnectionModel connectionModel, string serverParkName)
        {
            var serverParkNames = GetServerParkNames(connectionModel);

            return serverParkNames.Any(sp => sp.Equals(serverParkName, StringComparison.InvariantCultureIgnoreCase));
        }

        public IServerPark GetServerPark(ConnectionModel connectionModel, string serverParkName)
        {
            var connection = _connectionFactory.GetConnection(connectionModel);

            if (!ServerParkExists(connectionModel, serverParkName))
            {
                throw new DataNotFoundException($"Server park '{serverParkName}' not found");
            }

            return connection.GetServerPark(serverParkName);
        }

        public IEnumerable<IServerPark> GetServerParks(ConnectionModel connectionModel)
        {
            var connection = _connectionFactory.GetConnection(connectionModel);
            var serverParks = connection.ServerParks;

            if (!serverParks.Any())
            {
                throw new DataNotFoundException("No server parks found");
            }

            return serverParks;
        }

        public void RegisterMachineOnServerPark(ConnectionModel connectionModel,
            string serverParkName, string machineName)
        {
            Console.WriteLine("Attempt to get blaise tokens");

            var securePassword = new PasswordService().CreateSecurePassword(connectionModel.Password);
            var blaiseTokens = TokenProvider.GetBlaiseTokens(
                connectionModel.Binding,
                connectionModel.ServerName,
                connectionModel.Port,
                connectionModel.UserName,
                securePassword);

            var clientProxy = new StatNeth.Blaise.Administer.Deploy.ClientProxy.DeployServiceClientProxy(
                connectionModel.Binding,
                connectionModel.ServerName,
                connectionModel.Port,
                blaiseTokens);

            var logicalRoots = clientProxy.GetRemoteLogicalRoots(machineName, 8031);

            Console.WriteLine("Obtained logical roots");

            var logicalRoot = logicalRoots.FirstOrDefault(lr => 
                lr.Name.Equals("default", StringComparison.CurrentCultureIgnoreCase));
            if (logicalRoot == null)
            {
                throw new Exception("No logical root");
            }
            Console.WriteLine($"Obtained default logical root '{logicalRoot.Name}', '{logicalRoot.Location}'");

            var roles = clientProxy
                .GetRemoteDefinedRoles2(machineName, 8031, "http");

            Console.WriteLine("Obtained roles");

            foreach (var role in roles)
            {
                Console.WriteLine($"Obtained role '{role.Name}'");
            }
            
            var serverPark = GetServerPark(connectionModel, serverParkName);
            serverPark.AddMachine(
                machineName,
                logicalRoot.Name,
                roles.Select(r => r.Name).ToArray());
        }
    }
}
