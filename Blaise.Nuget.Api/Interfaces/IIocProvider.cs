using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Interfaces
{
    public interface IIocProvider
    {
        void RegisterDependencies(ConnectionModel connectionModel);
        T Resolve<T>();
    }
}