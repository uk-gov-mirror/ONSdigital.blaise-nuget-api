
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class RemoteDataLinkProvider : IRemoteDataLinkProvider
    {
        private readonly IRemoteDataServerFactory _connectionFactory;
        private readonly ISurveyService _surveyService;

        private string _instrumentName;
        private string _serverParkName;
        IDataLink4 _dataLink;

        public RemoteDataLinkProvider(
            IRemoteDataServerFactory connectionFactory,
            ISurveyService surveyService)
        {
            _connectionFactory = connectionFactory;
            _surveyService = surveyService;

            _instrumentName = string.Empty;
            _serverParkName = string.Empty;
        }

        public IDataLink4 GetDataLink(string instrumentName, string serverParkName)
        {
            return GetDataLink(null, instrumentName, serverParkName);
        }

        public IDataLink4 GetDataLink(string serverName, string instrumentName, string serverParkName)
        {
            if (_dataLink == null | instrumentName != _instrumentName || serverParkName != _serverParkName)
            {
                _instrumentName = instrumentName;
                _serverParkName = serverParkName;

                var instrumentId = _surveyService.GetInstrumentId(instrumentName, serverParkName);
                var connection = _connectionFactory.GetConnection(serverName);

                _dataLink = connection.GetDataLink(instrumentId, serverParkName);
            }

            return _dataLink;
        }
    }
}
