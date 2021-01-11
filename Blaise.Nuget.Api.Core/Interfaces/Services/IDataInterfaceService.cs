using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDataInterfaceService
    {
        IDataInterface CreateFileDataInterface(string databaseSourceFileName, string fileName, string dataModelFileName);

        void CreateSqlDataInterface(string databaseConnectionString, string fileName, string dataModelFileName);
    }
}