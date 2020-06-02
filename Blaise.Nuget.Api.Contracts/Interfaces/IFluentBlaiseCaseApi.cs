using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseCaseApi
    {
        IFluentBlaiseCaseApi Case(string primaryKeyValue);

        void Create(Dictionary<string, string> data);

        bool Exists();
    }
}