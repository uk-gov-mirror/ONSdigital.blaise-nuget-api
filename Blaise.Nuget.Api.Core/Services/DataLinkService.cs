using Blaise.Nuget.Api.Core.Interfaces;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DataLinkService : IDataLinkService
    {
        private readonly IRemoteDataServerFactory _connectionFactory;
        private readonly IParkService _parkService;

        private string _instrumentName;
        private string _serverParkName;
        IDataLink4 _dataLink;

        public DataLinkService(
            IRemoteDataServerFactory connectionFactory,
            IParkService parkService)
        {
            _connectionFactory = connectionFactory;
            _parkService = parkService;

            _instrumentName = string.Empty;
            _serverParkName = string.Empty;
        }

        public IDatamodel GetDataModel(string instrumentName, string serverParkName)
        {
            GetDataLink(instrumentName, serverParkName);

            return _dataLink.Datamodel;
        }

        public bool KeyExists(IKey key, string instrumentName, string serverParkName)
        {
            GetDataLink(instrumentName, serverParkName);

            return _dataLink.KeyExists(key);
        }

        public IDataSet ReadDataRecord(string instrumentName, string serverParkName)
        {
            GetDataLink(instrumentName, serverParkName);

            return _dataLink.Read(null);
        }
        public void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            GetDataLink(instrumentName, serverParkName);

            _dataLink.Write(dataRecord);
        }

        protected void GetDataLink(string instrumentName, string serverParkName)
        {
            if (_dataLink == null | instrumentName != _instrumentName || serverParkName != _serverParkName)
            {
                _instrumentName = instrumentName;
                _serverParkName = serverParkName;

                var instrumentId = _parkService.GetInstrumentId(instrumentName, serverParkName);
                var connection = _connectionFactory.GetConnection();

                _dataLink = connection.GetDataLink(instrumentId, serverParkName);
            }
        }
    }
}
