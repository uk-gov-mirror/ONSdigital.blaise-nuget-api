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
        IEnumerable<string> GetSurveyNames();

        IEnumerable<ISurvey> GetSurveys();

        bool KeyExists(IKey key);

        Guid GetInstrumentId();

        IDatamodel GetDataModel();

        CaseRecordType GetCaseRecordType();

        IDataSet GetDataSet();

        IDataRecord GetDataRecord(IKey key);

        void WriteDataRecord(IDataRecord dataRecord);

        void UpdateDataRecord(IDataRecord dataRecord, Dictionary<string, string> fieldData);

        bool CompletedFieldExists();

        void MarkCaseAsComplete(IDataRecord dataRecord);

        bool ProcessedFieldExists();

        void MarkCaseAsProcessed(IDataRecord dataRecord);
    }
}
