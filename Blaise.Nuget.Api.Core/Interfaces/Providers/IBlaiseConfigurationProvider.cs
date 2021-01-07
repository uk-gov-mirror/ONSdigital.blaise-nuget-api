using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IBlaiseConfigurationProvider
    {
        ConnectionModel GetConnectionModel();

        string LibraryDirectory { get; }

        int ConnectionExpiresInMinutes { get; }

        string DatabaseConnectionString { get; }
    }
}