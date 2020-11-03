using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ICaseService
    {
        SurveyType GetSurveyType(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        bool CaseExists(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);

        string GetPrimaryKeyValue(IDataRecord dataRecord);

        IDataSet GetDataSet(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IDataSet GetDataSet(string filePath);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);

        void WriteDataRecord(IDataRecord dataRecord, string filePath);

        void CreateNewDataRecord(ConnectionModel connectionModel, string primaryKeyValue, Dictionary<string, string> fieldData, string instrumentName,
            string serverParkName);

        void CreateNewDataRecord(string filePath, string primaryKeyValue, Dictionary<string, string> fieldData);

        void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, Dictionary<string, string> fieldData, string instrumentName,
            string serverParkName);

        bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName, FieldNameType fieldNameType);

        void RemoveDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);
        IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType);
        bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType);

        int GetNumberOfCases(ConnectionModel connectionModel, string instrumentName, string serverParkName);
        int GetNumberOfCases(string filePath);
    }
}
