using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class LocalDataLinkProvider : ILocalDataLinkProvider
    {
        private readonly IConfigurationProvider _configurationProvider;

        private readonly Dictionary<string, Tuple<IDataLink, DateTime>> _dataLinkConnections;

        public LocalDataLinkProvider(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _dataLinkConnections = new Dictionary<string, Tuple<IDataLink, DateTime>>();
        }

        public IDataLink GetDataLink(string filePath)
        {
            if (_dataLinkConnections.Any(c => c.Key == filePath))
            {
                var existingConnection = _dataLinkConnections.First(c => c.Key == filePath);

                return existingConnection.Value.Item2.HasExpired()
                    ? GetFreshConnection(filePath)
                    : existingConnection.Value.Item1;
            }

            return GetFreshConnection(filePath);
        }

        private IDataLink GetFreshConnection(string filePath)
        {
            if (_dataLinkConnections.Any(c => c.Key == filePath))
            {
                _dataLinkConnections.Remove(filePath);
            }

            var dataLink = DataLinkManager.GetDataLink(filePath);

            _dataLinkConnections.Add(filePath,
                new Tuple<IDataLink, DateTime>(dataLink, _configurationProvider.ConnectionExpiresInMinutes.GetExpiryDate()));

            return dataLink;
        }
    }
}
