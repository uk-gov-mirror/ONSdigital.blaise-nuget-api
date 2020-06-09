using System.Configuration;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;

namespace Blaise.Nuget.Api.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public ConnectionModel GetConnectionModel(string serverName = null)
        {
            var connectionModel = new ConnectionModel
            {
                ServerName = serverName ?? ConfigurationManager.AppSettings["BlaiseServerHostName"],
                UserName = ConfigurationManager.AppSettings["BlaiseServerUserName"],
                Password = ConfigurationManager.AppSettings["BlaiseServerPassword"],
                Binding = ConfigurationManager.AppSettings["BlaiseServerBinding"],
                Port = ConvertToInt(ConfigurationManager.AppSettings["BlaiseConnectionPort"]),
                RemotePort = ConvertToInt(ConfigurationManager.AppSettings["BlaiseRemoteConnectionPort"])
            };
            return connectionModel;
        }

        public string LibraryDirectory => ConfigurationManager.AppSettings["LibraryDirectory"];

        private static int ConvertToInt(string integer)
        {
            return int.Parse(integer);
        }
    }
}
