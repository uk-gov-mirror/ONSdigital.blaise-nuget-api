using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Core.Interfaces
{
    public interface IDataManagerService
    {
        IDataRecord GetDataRecord(IDatamodel dataModel);

        IKey GetKey(IDatamodel dataModel, string keyName);
    }
}
