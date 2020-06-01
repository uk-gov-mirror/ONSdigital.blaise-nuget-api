using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DataModelService : IDataModelService
    {
        private readonly IRemoteDataLinkProvider _remoteDataLinkProvider;

        public DataModelService(IRemoteDataLinkProvider remoteDataLinkProvider)
        {
            _remoteDataLinkProvider = remoteDataLinkProvider;
        }

        public IDatamodel GetDataModel(string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(instrumentName, serverParkName);

            return dataLink.Datamodel;
        }

        public IDatamodel GetDataModel(string serverName, string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(serverName, instrumentName, serverParkName);

            return dataLink.Datamodel;
        }

        public CaseRecordType GetCaseRecordType(string instrumentName, string serverParkName)
        {
            var dataModel = GetDataModel(instrumentName, serverParkName);

            if (dataModel.Name == "Appointment")
            {
                return CaseRecordType.Appointment;
            }

            if (dataModel.Name == "CatiDial")
            {
                return CaseRecordType.CatiDial;
            }

            return CaseRecordType.NotMapped;
        }
    }
}
