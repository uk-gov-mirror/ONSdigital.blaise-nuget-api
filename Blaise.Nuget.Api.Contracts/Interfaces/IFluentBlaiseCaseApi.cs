using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseCaseApi
    {
        void Create(Dictionary<string, string> data);

        bool Exists();
    }
}