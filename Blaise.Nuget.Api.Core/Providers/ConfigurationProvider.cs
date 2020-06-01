using System.Configuration;
using Blaise.Nuget.Api.Core.Interfaces.Providers;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {

        public string ServerName => ConfigurationManager.AppSettings["BlaiseServerHostName"];

        public string UserName => ConfigurationManager.AppSettings["BlaiseServerUserName"];

        public string Password => ConfigurationManager.AppSettings["BlaiseServerPassword"];

        public string Binding => ConfigurationManager.AppSettings["BlaiseServerBinding"];

        public int ConnectionPort => ConvertToInt(ConfigurationManager.AppSettings["BlaiseConnectionPort"]);

        public int RemoteConnectionPort => ConvertToInt(ConfigurationManager.AppSettings["BlaiseRemoteConnectionPort"]);

        private static int ConvertToInt(string integer)
        {
            return int.Parse(integer);
        }
    }
}
