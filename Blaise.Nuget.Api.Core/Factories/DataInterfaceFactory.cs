using Blaise.Nuget.Api.Core.Interfaces.Factories;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class DataInterfaceFactory : IDataInterfaceFactory
    {
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
