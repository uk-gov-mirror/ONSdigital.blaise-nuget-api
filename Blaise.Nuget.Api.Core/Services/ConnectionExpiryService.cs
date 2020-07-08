using System;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;

namespace Blaise.Nuget.Api.Core.Services
{
    public class ConnectionExpiryService : IConnectionExpiryService
    {
        private readonly IConfigurationProvider _configurationProvider;

        private DateTime _connectionExpiresOn;

        public ConnectionExpiryService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;

            _connectionExpiresOn = DateTime.Now.AddMinutes(_configurationProvider.ConnectionExpiresInMinutes);
        }

        public void ResetConnectionExpiryPeriod()
        {
            _connectionExpiresOn = DateTime.Now.AddMinutes(_configurationProvider.ConnectionExpiresInMinutes);
        }

        public bool ConnectionHasExpired()
        {
            return _connectionExpiresOn < DateTime.Now;
        }
    }
}
