using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IFieldService
    {
        bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName, FieldNameType fieldNameType);

        bool CaseHasBeenCompleted(IDataRecord dataRecord);

        void MarkCaseAsComplete(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);

        bool CaseHasBeenProcessed(IDataRecord dataRecord);

        void MarkCaseAsProcessed(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);

        IField GetField(IDataRecord dataRecord, FieldNameType fieldNameType);
    }
}