using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDataService
    {
        IDatamodel GetDataModel(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        SurveyType GetSurveyType(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IKey GetKey(IDatamodel dataModel, string keyName);

        IKey GetPrimaryKey(IDatamodel dataModel);

        bool KeyExists(ConnectionModel connectionModel, IKey key, string instrumentName, string serverParkName);

        bool CaseExists(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);

        string GetPrimaryKeyValue(IDataRecord dataRecord);

        void AssignPrimaryKeyValue(IKey key, string primaryKeyValue);

        IDataSet GetDataSet(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IDataSet GetDataSet(string filePath);

        IDataRecord GetDataRecord(IDatamodel datamodel);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, IKey key, string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IKey key, string filePath);

        void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);

        void WriteDataRecord(IDataRecord dataRecord, string filePath);

        void CreateNewDataRecord(ConnectionModel connectionModel, string primaryKeyValue, Dictionary<string, string> fieldData, string instrumentName,
            string serverParkName);

        void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, Dictionary<string, string> fieldData, string instrumentName,
            string serverParkName);

        bool CompletedFieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName);
        bool CaseHasBeenCompleted(IDataRecord dataRecord);

        void MarkCaseAsComplete(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);

        bool ProcessedFieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName);
        bool CaseHasBeenProcessed(IDataRecord dataRecord);

        void MarkCaseAsProcessed(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);

        void RemoveDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);
        IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType);
    }
}
