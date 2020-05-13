using Blaise.Nuget.Api.Core.Interfaces;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DataService : IDataService
    {
        private readonly IDataLinkService _dataLinkService;
        private readonly IDataManagerService _dataManagerService;

        public DataService(
            IDataLinkService dataLinkService,
            IDataManagerService dataManagerService)
        {
            _dataLinkService = dataLinkService;
            _dataManagerService = dataManagerService;
        }

        public IDatamodel GetDataModel(string instrumentName, string serverParkName)
        {
            return _dataLinkService.GetDataModel(instrumentName, serverParkName);
        }

        public IKey GetKey(IDatamodel datamodel, string keyName)
        {
            return _dataManagerService.GetKey(datamodel, keyName);
        }

        public bool KeyExists(IKey key, string instrumentName, string serverParkName)
        {
            return _dataLinkService.KeyExists(key, instrumentName, serverParkName);
        }

        public IDataRecord GetDataRecord(IDatamodel datamodel)
        {
            return _dataManagerService.GetDataRecord(datamodel);
        }

        public IDataSet ReadDataRecord(string instrumentName, string serverParkName)
        {
            return _dataLinkService.ReadDataRecord(instrumentName, serverParkName);
        }

        public void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            _dataLinkService.WriteDataRecord(dataRecord, instrumentName, serverParkName);
        }
    }
}
