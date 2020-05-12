using Blaise.Nuget.Api.Core.Interfaces;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DataManagerService : IDataManagerService
    {
        public IDataRecord GetDataRecord(IDatamodel dataModel)
        {
            return DataRecordManager.GetDataRecord(dataModel);
        }

        public IKey GetKey(IDatamodel dataModel, string keyName)
        {
            return DataRecordManager.GetKey(dataModel, keyName);
        }
    }
}
