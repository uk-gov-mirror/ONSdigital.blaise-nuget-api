namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IConfigurationProvider
    {
        string ServerName { get; }
        string UserName { get; }
        string Password { get; }
        string Binding { get; }
        int ConnectionPort { get; }
        int RemoteConnectionPort { get; }
    }
}