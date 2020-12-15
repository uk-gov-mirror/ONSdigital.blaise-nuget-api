using System;
using System.Configuration;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Helpers;

namespace Blaise.Nuget.Api.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        public ConnectionModel GetConnectionModel()
        {
            var connectionModel = new ConnectionModel
            {
                ServerName = GetEnvironmentVariable("ENV_BLAISE_SERVER_HOST_NAME") ?? GetLocalVariable("BlaiseServerHostName"),
                UserName = GetEnvironmentVariable("ENV_BLAISE_ADMIN_USER") ?? GetLocalVariable("BlaiseServerUserName"),
                Password = GetEnvironmentVariable("ENV_BLAISE_ADMIN_PASSWORD") ?? GetLocalVariable("BlaiseServerPassword"),
                Binding = GetEnvironmentVariable("ENV_BLAISE_SERVER_BINDING") ?? GetLocalVariable("BlaiseServerBinding"),
                Port = ConvertToInt(GetEnvironmentVariable("ENV_BLAISE_CONNECTION_PORT") ?? GetLocalVariable("BlaiseConnectionPort")),
                RemotePort = ConvertToInt(GetEnvironmentVariable("ENV_BLAISE_REMOTE_CONNECTION_PORT") ?? GetLocalVariable("BlaiseRemoteConnectionPort")),
                ConnectionExpiresInMinutes = ConvertToInt(GetEnvironmentVariable("ENV_CONNECTION_EXPIRES_IN_MINUTES") ?? GetLocalVariable("ConnectionExpiresInMinutes"))
            };
            return connectionModel;
        }

        public string LibraryDirectory => GetEnvironmentVariable("ENV_LIBRARY_DIRECTORY") ?? GetLocalVariable("LibraryDirectory");

        public int ConnectionExpiresInMinutes =>
            ConvertToInt(
                GetEnvironmentVariable("ENV_CONNECTION_EXPIRES_IN_MINUTES") ??
                (string.IsNullOrWhiteSpace(GetLocalVariable("ConnectionExpiresInMinutes"))
                    ? "30" : GetLocalVariable("ConnectionExpiresInMinutes")));

        private static string GetEnvironmentVariable(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName, EnvironmentVariableTarget.Machine);
        }

        private static string GetLocalVariable(string variableName)
        {
            var variable = ConfigurationManager.AppSettings[variableName];
            variable.ThrowExceptionIfNullOrEmpty(variableName);
            return variable;
        }

        private static int ConvertToInt(string variableName)
        {
            if (int.TryParse(variableName, out var result))
            {
                return result;
            }

            throw new ArgumentException($"Variable '{variableName}' is not in an integer format");
        }
    }
}
