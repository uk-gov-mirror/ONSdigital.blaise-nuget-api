using System;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class LocalDataLinkProvider : ILocalDataLinkProvider
    {
        private readonly IConnectionExpiryService _connectionExpiryService;

        private string _filePath;
        private IDataLink _dataLink;

        public LocalDataLinkProvider(IConnectionExpiryService connectionExpiryService)
        {
            _connectionExpiryService = connectionExpiryService;
        }

        public IDataLink GetDataLink(string filePath)
        {
            if (_dataLink == null | filePath != _filePath || _connectionExpiryService.ConnectionHasExpired())
            {
                _filePath = filePath;
                _dataLink = DataLinkManager.GetDataLink(filePath);
                _connectionExpiryService.ResetConnectionExpiryPeriod();
            }

            return _dataLink;
        }
    }
}
