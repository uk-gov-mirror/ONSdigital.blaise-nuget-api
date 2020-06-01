using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseRemoteApi
    {
        IFluentBlaiseRemoteApi WithServerPark(string serverParkName);

        IEnumerable<string> GetSurveyNames();

        IEnumerable<ISurvey> GetSurveys();

        IFluentBlaiseRemoteApi ForInstrument(string instrumentName);

        bool KeyExists(IKey key);

        bool CaseExists(string primaryKeyValue);

        bool CaseExists(string primaryKeyValue, string serverName);

        Guid GetInstrumentId();

        IDatamodel GetDataModel();

        IDatamodel GetDataModel(string serverName);

        CaseRecordType GetCaseRecordType();

        IDataSet GetDataSet();

        IDataRecord GetDataRecord(IKey key);

        void WriteDataRecord(IDataRecord dataRecord);

        void CreateNewDataRecord(string primaryKeyValue, Dictionary<string, string> fieldData);

        void UpdateDataRecord(IDataRecord dataRecord, Dictionary<string, string> fieldData);

        bool CompletedFieldExists();

        void MarkCaseAsComplete(IDataRecord dataRecord);

        bool ProcessedFieldExists();

        void MarkCaseAsProcessed(IDataRecord dataRecord);
    }
}
