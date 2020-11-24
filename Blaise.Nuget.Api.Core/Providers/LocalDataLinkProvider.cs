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

        private readonly Dictionary<string, Tuple<IDataLink4, DateTime>> _dataLinkConnections;

        public LocalDataLinkProvider(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
            _dataLinkConnections = new Dictionary<string, Tuple<IDataLink4, DateTime>>(StringComparer.OrdinalIgnoreCase);
        }

        public IDataLink4 GetDataLink(string databaseFile)
        {
            if (_dataLinkConnections.ContainsKey(databaseFile))
            {
                var (dataLink, expiryDate) = _dataLinkConnections[databaseFile];

                return expiryDate.HasExpired()
                    ? GetFreshConnection(databaseFile)
                    : dataLink ?? GetFreshConnection(databaseFile);
            }

            return GetFreshConnection(databaseFile);
        }

        private IDataLink4 GetFreshConnection(string databaseFile)
        {
            var dataLink = DataLinkManager.GetDataLink(databaseFile) as IDataLink4;

            _dataLinkConnections[databaseFile] = new Tuple<IDataLink4, DateTime>(dataLink, _configurationProvider.ConnectionExpiresInMinutes.GetExpiryDate());

            return dataLink;
        }
    }
}
