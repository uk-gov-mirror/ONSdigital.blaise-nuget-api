using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseRemoteApi
    {
        IFluentBlaiseRemoteApi ForServerPark(string serverParkName);

        IFluentBlaiseRemoteApi ForInstrument(string instrumentName);

        bool KeyExists(IKey key);

        Guid GetInstrumentId();

        IDatamodel GetDataModel();

        IDataSet GetDataSet();

        IDataRecord GetDataRecord(IKey key);

        void WriteDataRecord(IDataRecord dataRecord);
    }
}
