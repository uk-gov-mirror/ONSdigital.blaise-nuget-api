using Blaise.Nuget.Api.Core.Interfaces.Providers;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class LocalDataLinkProvider : ILocalDataLinkProvider
    {
        private string _filePath;
        private IDataLink _dataLink;

        public IDataLink GetDataLink(string filePath)
        {
            if (_dataLink == null | filePath != _filePath)
            {
                _filePath = filePath;
                _dataLink = DataLinkManager.GetDataLink(filePath);
            }

            return _dataLink;
        }
    }
}
