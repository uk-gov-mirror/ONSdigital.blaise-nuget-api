using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDataModelService
    {
        IDatamodel GetDataModel(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IDatamodel GetDataModel(ConnectionModel connectionModel, string databaseFile);
    }
}