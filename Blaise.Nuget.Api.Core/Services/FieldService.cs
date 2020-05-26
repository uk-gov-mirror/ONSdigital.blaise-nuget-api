using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class FieldService : IFieldService
    {
        private const string CompletedFieldName = "Completed";
        private const string ProcessedFieldName = "Processed";

        private readonly IDataRecordService _dataRecordService;
        private readonly IDataModelService _dataModelService;

        public FieldService(
            IDataRecordService dataRecordService, 
            IDataModelService dataModelService)
        {
            _dataRecordService = dataRecordService;
            _dataModelService = dataModelService;
        }

        public bool CompletedFieldExists(string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(instrumentName, serverParkName);
            var definitionScope = (IDefinitionScope2)dataModel;

            return definitionScope.FieldExists(CompletedFieldName);
        }

        public bool CaseHasBeenCompleted(IDataRecord dataRecord)
        {
            var completedField = GetField(dataRecord, CompletedFieldName);

            return completedField.DataValue.IntegerValue == 1;
        }

        public void MarkCaseAsComplete(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            var completedField = GetField(dataRecord, CompletedFieldName);
            completedField.DataValue.Assign("1");

            _dataRecordService.WriteDataRecord(dataRecord, instrumentName, serverParkName);
        }

        public bool ProcessedFieldExists(string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(instrumentName, serverParkName);
            var definitionScope = (IDefinitionScope2)dataModel;

            return definitionScope.FieldExists(ProcessedFieldName);
        }

        public bool CaseHasBeenProcessed(IDataRecord dataRecord)
        {
            var processedField = GetField(dataRecord, ProcessedFieldName);

            return processedField.DataValue.EnumerationValue == 1;
        }

        public void MarkCaseAsProcessed(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            var processedField = GetField(dataRecord, ProcessedFieldName);

            processedField.DataValue.Assign("1");

            _dataRecordService.WriteDataRecord(dataRecord, instrumentName, serverParkName);
        }

        private static IField GetField(IDataRecord dataRecord, string fieldName)
        {
            var field = dataRecord.GetField(fieldName);

            return field;
        }
    }
}
