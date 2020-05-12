using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces
{
    public interface IDataService
    {
        IDatamodel GetDataModel(string instrumentName, string serverParkName);

        IKey GetKey(IDatamodel datamodel, string keyName);

        bool KeyExists(IKey key, string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IDatamodel datamodel);

        void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName);
    }
}
