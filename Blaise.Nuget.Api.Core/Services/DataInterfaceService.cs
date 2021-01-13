using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DataInterfaceService : IDataInterfaceService
    {
        private readonly IDataInterfaceFactory _dataInterfaceFactory;
        public DataInterfaceService(IDataInterfaceFactory dataInterfaceFactory)
        {
            _dataInterfaceFactory = dataInterfaceFactory;
        }

        public IDataInterface CreateFileDataInterface(string databaseSourceFileName, string fileName,
            string dataModelFileName)
        {
            var dataInterface = _dataInterfaceFactory.GetDataInterfaceForFile(databaseSourceFileName);
            
            dataInterface.FileName = fileName;
            dataInterface.DatamodelFileName = dataModelFileName;
            dataInterface.CreateTableDefinitions();
            dataInterface.CreateDatabaseObjects(null, true);
            dataInterface.SaveToFile(true);

            return dataInterface;
        }

        public IDataInterface CreateSqlDataInterface(string databaseConnectionString, string fileName, string dataModelFileName)
        {
            var dataInterface = _dataInterfaceFactory.GetDataInterfaceForSql(databaseConnectionString);

            dataInterface.FileName = fileName;
            dataInterface.DatamodelFileName = dataModelFileName;
            dataInterface.CreateTableDefinitions();
            dataInterface.CreateDatabaseObjects(dataInterface.ConnectionInfo.GetConnectionString(), true);
            dataInterface.SaveToFile(true);

            return dataInterface;
        }
    }
}
