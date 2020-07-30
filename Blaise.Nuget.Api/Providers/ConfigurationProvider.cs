using System;
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
                ServerName = Environment.GetEnvironmentVariable("ENV_BLAISE_SERVER_HOST_NAME", EnvironmentVariableTarget.Machine) ?? ConfigurationManager.AppSettings["BlaiseServerHostName"],
                UserName = Environment.GetEnvironmentVariable("ENV_BLAISE_ADMIN_USER", EnvironmentVariableTarget.Machine) ?? ConfigurationManager.AppSettings["BlaiseServerUserName"],
                Password = Environment.GetEnvironmentVariable("ENV_BLAISE_ADMIN_PASSWORD", EnvironmentVariableTarget.Machine) ?? ConfigurationManager.AppSettings["BlaiseServerPassword"],
                Binding = Environment.GetEnvironmentVariable("ENV_BLAISE_SERVER_BINDING", EnvironmentVariableTarget.Machine) ?? ConfigurationManager.AppSettings["BlaiseServerBinding"],
                Port = ConvertToInt(Environment.GetEnvironmentVariable("ENV_BLAISE_CONNECTION_PORT", EnvironmentVariableTarget.Machine) ?? ConfigurationManager.AppSettings["BlaiseConnectionPort"]),
                RemotePort = ConvertToInt(Environment.GetEnvironmentVariable("ENV_BLAISE_REMOTE_CONNECTION_PORT", EnvironmentVariableTarget.Machine) ?? ConfigurationManager.AppSettings["BlaiseRemoteConnectionPort"]),
                ConnectionExpiresInMinutes = Environment.GetEnvironmentVariable("ENV_CONNECTION_EXPIRES_IN_MINUTES", EnvironmentVariableTarget.Machine) ?? ConvertToInt(ConfigurationManager.AppSettings["ConnectionExpiresInMinutes"]),
            };
            return connectionModel;
        }         

        public string LibraryDirectory => Environment.GetEnvironmentVariable("ENV_LIBRARY_DIRECTORY", EnvironmentVariableTarget.Machine) ?? ConfigurationManager.AppSettings["LibraryDirectory"];

        public int ConnectionExpiresInMinutes =>
            ConvertToInt(ConfigurationManager.AppSettings["ConnectionExpiresInMinutes"]);

        private static int ConvertToInt(string integer)
        {
            return int.Parse(integer);
        }
    }
}
