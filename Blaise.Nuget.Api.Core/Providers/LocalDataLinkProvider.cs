using System;
using System.Collections.Generic;
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
            _dataLinkConnections = new Dictionary<string, Tuple<IDataLink, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public IDataLink GetDataLink(string filePath)
        {
            if (_dataLinkConnections.ContainsKey(filePath))
            {
                var (dataLink, expiryDate) = _dataLinkConnections[filePath];

                return expiryDate.HasExpired()
                    ? GetFreshConnection(filePath)
                    : dataLink ?? GetFreshConnection(filePath);
            }

            return GetFreshConnection(filePath);
        }

        private IDataLink GetFreshConnection(string filePath)
        {
            var dataLink = DataLinkManager.GetDataLink(filePath);

            _dataLinkConnections[filePath] = new Tuple<IDataLink, DateTime>(dataLink, _configurationProvider.ConnectionExpiresInMinutes.GetExpiryDate());

            return dataLink;
        }
    }
}
