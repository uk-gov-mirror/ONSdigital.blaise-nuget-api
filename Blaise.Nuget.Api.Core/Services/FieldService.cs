using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class FieldService : IFieldService
    {
        private readonly IDataRecordService _dataRecordService;
        private readonly IDataModelService _dataModelService;

        public FieldService(
            IDataRecordService dataRecordService, 
            IDataModelService dataModelService)
        {
            _dataRecordService = dataRecordService;
            _dataModelService = dataModelService;
        }

        public bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName, FieldNameType fieldNameType)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
            var definitionScope = (IDefinitionScope2)dataModel;

            return definitionScope.FieldExists(fieldNameType.ToString());
        }

        public bool CaseHasBeenCompleted(IDataRecord dataRecord)
        {
            var completedField = GetField(dataRecord, FieldNameType.Completed);

            return completedField.DataValue.IntegerValue == 1;
        }

        public void MarkCaseAsComplete(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            var completedField = GetField(dataRecord, FieldNameType.Completed);
            completedField.DataValue.Assign("1");

            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public bool CaseHasBeenProcessed(IDataRecord dataRecord)
        {
            var processedField = GetField(dataRecord, FieldNameType.Processed);

            return processedField.DataValue.EnumerationValue == 1;
        }

        public void MarkCaseAsProcessed(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            var processedField = GetField(dataRecord, FieldNameType.Processed);

            processedField.DataValue.Assign("1");

            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public IField GetField(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            var field = dataRecord.GetField(fieldNameType.FromDescription());

            return field;
        }
    }
}
