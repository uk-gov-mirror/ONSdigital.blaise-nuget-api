using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDataLinkService
    {
        IDatamodel GetDataModel(string instrumentName, string serverParkName);

        bool KeyExists(IKey key, string instrumentName, string serverParkName);

        IDataSet ReadData(string instrumentName, string serverParkName);

        IDataRecord ReadDataRecord(IKey key, string instrumentName, string serverParkName);

        IDataRecord ReadDataRecord(IKey key, string filePath);

        void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName);

        void WriteDataRecord(IDataRecord dataRecord, string filePath);
    }
}
