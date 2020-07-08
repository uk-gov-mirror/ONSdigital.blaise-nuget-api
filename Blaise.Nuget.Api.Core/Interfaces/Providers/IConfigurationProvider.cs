using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IConfigurationProvider
    {
        ConnectionModel GetConnectionModel();

        string LibraryDirectory { get; }

        int ConnectionExpiresInMinutes { get; }
    }
}