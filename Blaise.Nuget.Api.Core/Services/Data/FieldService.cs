using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Services.Data
{
    public class FieldService : IFieldService
    {
        private const string CompletedFieldName = "Completed";
        private const string ProcessedFieldName = "Processed";

        private readonly IDataRecordService _dataRecordService;

        public FieldService(IDataRecordService dataRecordService)
        {
            _dataRecordService = dataRecordService;
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
