using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Interfaces
{
    public interface IIocProvider
    {
        void RegisterDependencies();
        T Resolve<T>();
    }
}