using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
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

        private readonly Dictionary<Tuple<string, string>, Tuple<IDataLink4, DateTime>> _dataLinkConnections;


        public RemoteDataLinkProvider(
            IRemoteDataServerFactory connectionFactory,
            ISurveyService surveyService)
        {
            _connectionFactory = connectionFactory;
            _surveyService = surveyService;

            _dataLinkConnections = new Dictionary<Tuple<string, string>, Tuple<IDataLink4, DateTime>>();
        }

        public IDataLink4 GetDataLink(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            if (_dataLinkConnections.Any(c => c.Key.Item1 == instrumentName && c.Key.Item2 == serverParkName))
            {
                var existingConnection = _dataLinkConnections.First(c => c.Key.Item1 == instrumentName && c.Key.Item2 == serverParkName);

                return existingConnection.Value.Item2.HasExpired()
                    ? GetFreshConnection(connectionModel, instrumentName, serverParkName)
                    : existingConnection.Value.Item1;
            }

            return GetFreshConnection(connectionModel, instrumentName, serverParkName);
        }

        private IDataLink4 GetFreshConnection(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            if (_dataLinkConnections.Any(c => c.Key.Item1 == instrumentName && c.Key.Item2 == serverParkName))
            {
                _dataLinkConnections.Remove(new Tuple<string, string>(instrumentName, serverParkName));
            }

            var instrumentId = _surveyService.GetInstrumentId(connectionModel, instrumentName, serverParkName);
            var connection = _connectionFactory.GetConnection(connectionModel);

            var dataLink = connection.GetDataLink(instrumentId, serverParkName);

            _dataLinkConnections.Add(new Tuple<string, string>(instrumentName, serverParkName), 
                new Tuple<IDataLink4, DateTime>(dataLink, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate()));

            return dataLink;
        }
    }
}
