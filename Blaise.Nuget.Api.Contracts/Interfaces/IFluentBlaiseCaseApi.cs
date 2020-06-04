using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseCaseApi
    {
        void Add(Dictionary<string, string> data);

        string PrimaryKeyValue();

        bool IsComplete();

        bool HasBeenProcessed();

        void SetStatusAs(StatusType statusType);

        bool Exists();
    }
}