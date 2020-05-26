using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDataModelService
    {
        IDatamodel GetDataModel(string instrumentName, string serverParkName);
    }
}