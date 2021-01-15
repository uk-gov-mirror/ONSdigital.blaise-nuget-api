using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface IDataInterfaceFactory
    {
        IDataInterface GetDataInterfaceForFile(string databaseSourceFileName);

        IDataInterface GetDataInterfaceForSql(string databaseConnectionString);
    }
}