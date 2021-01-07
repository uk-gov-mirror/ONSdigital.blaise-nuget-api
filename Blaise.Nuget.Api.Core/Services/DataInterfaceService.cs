using System;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DataInterfaceService : IDataInterfaceService
    {
        private readonly IDataInterfaceFactory _dataInterfaceFactory;
        private readonly IBlaiseConfigurationProvider _configurationProvider;

        public DataInterfaceService(
            IDataInterfaceFactory dataInterfaceFactory, 
            IBlaiseConfigurationProvider configurationProvider)
        {
            _dataInterfaceFactory = dataInterfaceFactory;
            _configurationProvider = configurationProvider;
        }

        public void UpdateDataInterfaceConnection(ConnectionModel connectionModel, Guid instrumentGuid)
        {
            var dataInterface = _dataInterfaceFactory.GetConnection(connectionModel, instrumentGuid);
            var databaseConnectionString = _configurationProvider.DatabaseConnectionString;

            dataInterface.ConnectionInfo.SetConnectionString(databaseConnectionString);
            dataInterface.CreateTableDefinitions();
            dataInterface.CreateDatabaseObjects(databaseConnectionString, true);
            dataInterface.SaveToFile(true);
        }
    }
}
