
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
using IDatamodel = StatNeth.Blaise.API.Meta.IDatamodel;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseApi
    {
        IEnumerable<string> GetServerParkNames();

        bool ServerParkExists(string serverParkName);

        Guid GetInstrumentId(string instrumentName, string serverParkName);

        IDatamodel GetDataModel(string instrumentName, string serverParkName);

        IKey GetKey(IDatamodel dataModel, string keyName);

        bool KeyExists(IKey key, string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IDatamodel dataModel);

        void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName);
    }
}
