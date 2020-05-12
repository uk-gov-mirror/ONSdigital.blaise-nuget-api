using Blaise.Nuget.Core.Interfaces;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Core.Services
{
    public class DataManagerService : IDataManagerService
    {
        public IDataRecord GetDataRecord(IDatamodel datamodel)
        {
            return DataRecordManager.GetDataRecord(datamodel);
        }

        public IKey GetKey(IDatamodel datamodel, string keyName)
        {
            return DataRecordManager.GetKey(datamodel, keyName);
        }
    }
}
