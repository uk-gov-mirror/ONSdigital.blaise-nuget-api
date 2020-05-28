using System.Collections.Generic;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Interfaces.Mappers
{
    public interface IDataMapperService
    {
        IDataRecord MapDataRecordFields(IDataRecord dataRecord, IDatamodel dataModel, IKey key,
            string primaryKeyValue, Dictionary<string, string> fieldData);

        IDataRecord MapDataRecordFields(IDataRecord dataRecord, Dictionary<string, string> fieldData);
    }
}