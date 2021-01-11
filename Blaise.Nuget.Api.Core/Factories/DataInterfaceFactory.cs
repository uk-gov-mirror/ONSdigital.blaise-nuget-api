using Blaise.Nuget.Api.Core.Interfaces.Factories;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class DataInterfaceFactory : IDataInterfaceFactory
    {
        public IDataInterface GetDataInterfaceForFile(string databaseSourceFileName)
        {
            var dataInterface = DataInterfaceManager.GetDataInterface();
            dataInterface.ConnectionInfo.DataSourceType = DataSourceType.Blaise;
            dataInterface.ConnectionInfo.DataProviderType = DataProviderType.BlaiseDataProviderForDotNET;
            dataInterface.DataPartitionType = DataPartitionType.Stream;

            var connectionBuilder = DataInterfaceManager.GetBlaiseConnectionStringBuilder();
            connectionBuilder.DataSource = databaseSourceFileName;
            dataInterface.ConnectionInfo.SetConnectionString(connectionBuilder.ConnectionString);

            return dataInterface;
        }

        public IDataInterface GetDataInterfaceForSql(string databaseConnectionString)
        {
            var dataInterface = DataInterfaceManager.GetDataInterface();
            dataInterface.ConnectionInfo.DataSourceType = DataSourceType.MySQL;
            dataInterface.ConnectionInfo.DataProviderType = DataProviderType.MySQLDataProvider;
            dataInterface.DataPartitionType = DataPartitionType.Stream;

            var connectionStringBuilder = DataInterfaceManager.GetBlaiseConnectionStringBuilder();
            connectionStringBuilder.DataSource = databaseConnectionString;
            dataInterface.ConnectionInfo.SetConnectionString(connectionStringBuilder.ConnectionString);

            return dataInterface;
        }
    }
}
