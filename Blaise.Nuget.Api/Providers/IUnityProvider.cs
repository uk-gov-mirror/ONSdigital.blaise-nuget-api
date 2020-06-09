using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Providers
{
    public interface IUnityProvider
    {
        void RegisterDependencies(ConnectionModel connectionModel);
        T Resolve<T>();
    }
}