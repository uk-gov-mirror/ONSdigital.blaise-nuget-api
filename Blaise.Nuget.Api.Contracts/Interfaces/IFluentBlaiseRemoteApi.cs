using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseRemoteApi
    {
        IFluentBlaiseRemoteApi WithServerPark(string serverParkName);

        IEnumerable<string> GetSurveys();

        IFluentBlaiseRemoteApi ForInstrument(string instrumentName);

        bool KeyExists(IKey key);

        Guid GetInstrumentId();

        IDatamodel GetDataModel();

        IDataSet GetDataSet();

        IDataRecord GetDataRecord(IKey key);

        void WriteDataRecord(IDataRecord dataRecord);
    }
}
