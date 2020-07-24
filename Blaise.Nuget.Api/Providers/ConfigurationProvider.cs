using System.Configuration;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;

namespace Blaise.Nuget.Api.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public ConnectionModel GetConnectionModel()
        {
            var connectionModel = new ConnectionModel
            {
                ServerName = ConfigurationManager.AppSettings["BlaiseServerHostName"],
                UserName = ConfigurationManager.AppSettings["BlaiseServerUserName"],
                Password = Environment.GetEnvironmentVariable("BLAISE_SERVER_PASSWORD", EnvironmentVariableTarget.Machine) ?? ConfigurationManager.AppSettings["BlaiseServerPassword"],
                Binding = ConfigurationManager.AppSettings["BlaiseServerBinding"],
                Port = ConvertToInt(ConfigurationManager.AppSettings["BlaiseConnectionPort"]),
                RemotePort = ConvertToInt(ConfigurationManager.AppSettings["BlaiseRemoteConnectionPort"]),
                ConnectionExpiresInMinutes = ConvertToInt(ConfigurationManager.AppSettings["ConnectionExpiresInMinutes"]),
            };
            return connectionModel;
        }

        public string LibraryDirectory => ConfigurationManager.AppSettings["LibraryDirectory"];

        public int ConnectionExpiresInMinutes =>
            ConvertToInt(ConfigurationManager.AppSettings["ConnectionExpiresInMinutes"]);

        private static int ConvertToInt(string integer)
        {
            return int.Parse(integer);
        }
    }
}
