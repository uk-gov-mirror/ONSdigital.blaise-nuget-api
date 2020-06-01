﻿
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.ServerManager;
using IDatamodel = StatNeth.Blaise.API.Meta.IDatamodel;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseApi
    {
        IEnumerable<string> GetServerParkNames();

        IEnumerable<string> GetSurveyNames(string serverParkName);

        IEnumerable<ISurvey> GetSurveys(string serverParkName);

        IEnumerable<ISurvey> GetAllSurveys();

        bool ServerParkExists(string serverParkName);

        Guid GetInstrumentId(string instrumentName, string serverParkName);

        IDatamodel GetDataModel(string instrumentName, string serverParkName);

        CaseRecordType GetCaseRecordType(string instrumentName, string serverParkName);

        IKey GetKey(IDatamodel dataModel, string keyName);

        IKey GetPrimaryKey(IDatamodel dataModel);

        bool KeyExists(IKey key, string instrumentName, string serverParkName);

        bool CaseExists(string primaryKeyValue, string instrumentName, string serverParkName);

        string GetPrimaryKeyValue(IDataRecord dataRecord);

        void AssignPrimaryKeyValue(IKey key, string primaryKeyValue);

        IDataSet GetDataSet(string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IDatamodel dataModel);

        IDataRecord GetDataRecord(IKey key, string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IKey key, string filePath);

        void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName);

        void WriteDataRecord(IDataRecord dataRecord, string filePath);

        void CreateNewDataRecord(string primaryKeyValue, Dictionary<string, string> fieldData, string instrumentName,
            string serverParkName);

        void UpdateDataRecord(IDataRecord dataRecord, Dictionary<string, string> fieldData, string instrumentName,
            string serverParkName);

        bool CompletedFieldExists(string instrumentName, string serverParkName);

        bool CaseHasBeenCompleted(IDataRecord dataRecord);

        void MarkCaseAsComplete(IDataRecord dataRecord, string instrumentName, string serverParkName);

        bool ProcessedFieldExists(string instrumentName, string serverParkName);
        bool CaseHasBeenProcessed(IDataRecord dataRecord);

        void MarkCaseAsProcessed(IDataRecord dataRecord, string instrumentName, string serverParkName);
    }
}
