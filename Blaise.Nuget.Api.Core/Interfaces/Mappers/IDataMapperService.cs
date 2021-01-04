using System.Collections.Generic;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Interfaces.Mappers
{
    public interface IDataMapperService
    {
        IDataRecord MapDataRecordFields(IDataRecord dataRecord, IKey key,
            string primaryKeyValue, Dictionary<string, string> fieldData);

        IDataRecord MapDataRecordFields(IDataRecord dataRecord, Dictionary<string, string> fieldData);
    }
}