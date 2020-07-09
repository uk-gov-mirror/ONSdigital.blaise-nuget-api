using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IFieldService
    {
        bool CompletedFieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        bool CaseHasBeenCompleted(IDataRecord dataRecord);

        void MarkCaseAsComplete(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);

        bool ProcessedFieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        bool CaseHasBeenProcessed(IDataRecord dataRecord);

        void MarkCaseAsProcessed(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);
    }
}