using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class KeyService : IKeyService
    {
        private readonly IRemoteDataLinkProvider _remoteDataLinkProvider;

        public KeyService(IRemoteDataLinkProvider remoteDataLinkProvider)
        {
            _remoteDataLinkProvider = remoteDataLinkProvider;
        }

        public bool KeyExists(IKey key, string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(instrumentName, serverParkName);

            return dataLink.KeyExists(key);
        }

        public IKey GetKey(IDatamodel dataModel, string keyName)
        {
            return DataRecordManager.GetKey(dataModel, keyName);
        }

        public string GetPrimaryKeyValue(IDataRecord dataRecord)
        {
            return dataRecord.Keys[0].KeyValue.Trim();
        }

        public void AssignPrimaryKeyValue(IKey key, string primaryKey)
        {
            key.Fields[0].DataValue.Assign(primaryKey);
        }
    }
}
