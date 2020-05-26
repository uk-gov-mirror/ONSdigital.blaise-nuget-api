
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
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

        IKey GetKey(IDatamodel dataModel, string keyName);

        bool KeyExists(IKey key, string instrumentName, string serverParkName);

        string GetPrimaryKey(IDataRecord dataRecord);

        IDataSet GetDataSet(string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IDatamodel dataModel);

        IDataRecord GetDataRecord(IKey key, string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IKey key, string filePath);

        void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName);

        void WriteDataRecord(IDataRecord dataRecord, string filePath);

        bool CompletedFieldExists(string instrumentName, string serverParkName);

        bool CaseHasBeenCompleted(IDataRecord dataRecord);

        void MarkCaseAsComplete(IDataRecord dataRecord, string instrumentName, string serverParkName);

        bool ProcessedFieldExists(string instrumentName, string serverParkName);
        bool CaseHasBeenProcessed(IDataRecord dataRecord);

        void MarkCaseAsProcessed(IDataRecord dataRecord, string instrumentName, string serverParkName);
    }
}
