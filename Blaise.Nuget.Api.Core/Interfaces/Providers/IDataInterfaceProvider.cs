using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IDataInterfaceProvider
    {
        IDataInterface CreateFileDataInterface(string dataSourceFileName, string dataInterfaceFileName, string dataModelFileName);

        IDataInterface CreateSqlDataInterface(string dataSourceFileName, string dataInterfaceFileName, string dataModelFileName);
    }
}