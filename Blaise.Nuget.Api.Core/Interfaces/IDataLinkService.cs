using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces
{
    public interface IDataLinkService
    {
        IDatamodel GetDataModel(string instrumentName, string serverParkName);

        bool KeyExists(IKey key, string instrumentName, string serverParkName);

        IDataSet ReadDataRecord(string instrumentName, string serverParkName);

        void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName);
    }
}
