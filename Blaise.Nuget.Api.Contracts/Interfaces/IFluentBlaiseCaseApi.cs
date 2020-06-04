using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseCaseApi
    {
        IFluentBlaiseCaseApi WithData(Dictionary<string, string> data);

        void Add();

        string PrimaryKeyValue();

        bool IsComplete();

        bool HasBeenProcessed();

        IFluentBlaiseCaseApi WithStatus(StatusType statusType);

        void Update();

        bool Exists();
    }
}