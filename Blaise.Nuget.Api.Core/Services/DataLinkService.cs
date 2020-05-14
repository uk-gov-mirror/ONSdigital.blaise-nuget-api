using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DataLinkService : IDataLinkService
    {
        private readonly IRemoteDataLinkProvider _remoteDataLinkProvider;
        private readonly ILocalDataLinkProvider _localDataLinkProvider;


        public DataLinkService(
            IRemoteDataLinkProvider remoteDataLinkProvider,
            ILocalDataLinkProvider localDataLinkProvider)
        {
            _remoteDataLinkProvider = remoteDataLinkProvider;
            _localDataLinkProvider = localDataLinkProvider;
        }

        public IDatamodel GetDataModel(string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(instrumentName, serverParkName);

            return dataLink.Datamodel;
        }

        public bool KeyExists(IKey key, string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(instrumentName, serverParkName);

            return dataLink.KeyExists(key);
        }

        public IDataSet GetDataSet(string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(instrumentName, serverParkName);

            return dataLink.Read(null);
        }

        public IDataRecord GetDataRecord(IKey key, string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(instrumentName, serverParkName);

            return dataLink.ReadRecord(key);
        }

        public IDataRecord GetDataRecord(IKey key, string filePath)
        {
            var dataLink = _localDataLinkProvider.GetDataLink(filePath);

            return dataLink.ReadRecord(key);
        }

        public void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(instrumentName, serverParkName);

            dataLink.Write(dataRecord);

        }

        public void WriteDataRecord(IDataRecord dataRecord, string filePath)
        {
            var dataLink = _localDataLinkProvider.GetDataLink(filePath);

            dataLink.Write(dataRecord);
        }
    }
}
