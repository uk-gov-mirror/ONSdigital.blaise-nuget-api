using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Google.Cloud.Storage.V1;

namespace Blaise.Nuget.Api.Core.Providers
{
    public class CloudStorageClientProvider : IStorageClientProvider
    {
        public StorageClient GetStorageClient()
        {
            return StorageClient.Create();
        }
    }
}
