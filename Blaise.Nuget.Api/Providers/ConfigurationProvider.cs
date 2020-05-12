
using Blaise.Nuget.Api.Core.Models;
using System;
using System.Configuration;

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
            };
            return connectionModel;
        }

        public ConnectionModel GetRemoteConnectionModel()
        {
            var connectionModel = GetConnectionModel();
            connectionModel.Port = ConvertToInt(ConfigurationManager.AppSettings["BlaiseRemoteConnectionPort"]);
            return connectionModel;
        }

        private int ConvertToInt(string integer)
        {
            return Int32.Parse(integer);
        }
    }
}
