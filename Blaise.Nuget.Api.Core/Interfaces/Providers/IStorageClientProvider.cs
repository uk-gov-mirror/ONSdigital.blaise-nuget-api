using Google.Cloud.Storage.V1;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IStorageClientProvider
    {
        StorageClient GetStorageClient();
    }
}
