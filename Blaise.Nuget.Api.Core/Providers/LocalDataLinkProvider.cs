using System;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class LocalDataLinkProvider : ILocalDataLinkProvider
    {
        private string _filePath;
        private IDataLink _dataLink;
        private DateTime _connectionExpiresOn;

        public IDataLink GetDataLink(string filePath)
        {
            if (_dataLink == null | filePath != _filePath || ConnectionHasExpired())
            {
                _filePath = filePath;
                _dataLink = DataLinkManager.GetDataLink(filePath);
                _connectionExpiresOn = DateTime.Now.AddHours(1);
            }

            return _dataLink;
        }

        private bool ConnectionHasExpired()
        {
            return _connectionExpiresOn < DateTime.Now;
        }
    }
}
