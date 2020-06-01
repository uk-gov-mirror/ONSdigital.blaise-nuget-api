using System.Configuration;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Providers
{
    public class ConfigurationProvider
    {
        public ConnectionModel GetConnectionModel()
        {
            var connectionModel = new ConnectionModel
            {
                ServerName = ConfigurationManager.AppSettings["BlaiseServerHostName"],
                UserName = ConfigurationManager.AppSettings["BlaiseServerUserName"],
                Password = ConfigurationManager.AppSettings["BlaiseServerPassword"],
                Binding = ConfigurationManager.AppSettings["BlaiseServerBinding"],
                Port = ConvertToInt(ConfigurationManager.AppSettings["BlaiseConnectionPort"]),
                RemotePort = ConvertToInt(ConfigurationManager.AppSettings["BlaiseRemoteConnectionPort"])
            };
            return connectionModel;
        }

        private static int ConvertToInt(string integer)
        {
            return int.Parse(integer);
        }
    }
}
