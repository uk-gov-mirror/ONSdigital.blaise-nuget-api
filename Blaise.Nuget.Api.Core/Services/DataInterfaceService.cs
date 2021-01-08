using System;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataInterface;

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
            //var dataInterface = _dataInterfaceFactory.GetConnection(connectionModel, instrumentGuid);
            var databaseConnectionString = _configurationProvider.DatabaseConnectionString;
            
            var dataInterface = DataInterfaceManager.GetDataInterface();
            dataInterface.ConnectionInfo.DataSourceType = DataSourceType.Blaise;
            dataInterface.ConnectionInfo.DataProviderType =DataProviderType.BlaiseDataProviderForDotNET;
            dataInterface.DataPartitionType = DataPartitionType.Stream;

            var connectionStringBuilder = DataInterfaceManager.GetBlaiseConnectionStringBuilder();
            connectionStringBuilder.DataSource = databaseConnectionString; //@"D:\Flight.bdbx";
            dataInterface.ConnectionInfo.SetConnectionString(connectionStringBuilder.ConnectionString);
            dataInterface.DatamodelFileName = @"D:\Flight.bmix";

            dataInterface.CreateTableDefinitions();
            dataInterface.CreateDatabaseObjects(databaseConnectionString, true);
            dataInterface.SaveToFile(true);
        }
    }
}
