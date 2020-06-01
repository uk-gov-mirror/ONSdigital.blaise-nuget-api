using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDataModelService
    {
        IDatamodel GetDataModel(string instrumentName, string serverParkName);

        IDatamodel GetDataModel(string serverName, string instrumentName, string serverParkName);

        CaseRecordType GetCaseRecordType(string instrumentName, string serverParkName);
    }
}