using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDataService
    {
        IDatamodel GetDataModel(string instrumentName, string serverParkName);

        IKey GetKey(IDatamodel datamodel, string keyName);

        bool KeyExists(IKey key, string instrumentName, string serverParkName);

        string GetPrimaryKey(IDataRecord dataRecord);

        IDataSet GetDataSet(string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IDatamodel datamodel);

        IDataRecord GetDataRecord(IKey key, string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IKey key, string filePath);

        void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName);

        void WriteDataRecord(IDataRecord dataRecord, string filePath);
    }
}
