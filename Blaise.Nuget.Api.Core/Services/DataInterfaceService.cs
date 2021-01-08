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

        public void CreateDataInterface(string fileName, string dataModelFileName)
        {
            var databaseConnectionString = _configurationProvider.DatabaseConnectionString;
            var dataInterface = _dataInterfaceFactory.GetDataInterfaceForSql(databaseConnectionString);

            dataInterface.FileName = fileName;
            dataInterface.DatamodelFileName = dataModelFileName;
            dataInterface.CreateTableDefinitions();
            dataInterface.CreateDatabaseObjects(dataInterface.ConnectionInfo.GetConnectionString(), true);
            dataInterface.SaveToFile(true);
        }
    }
}
