using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseCaseApi
    {
        void Create(Dictionary<string, string> data);

        string PrimaryKeyValue();

        bool Completed();

        bool Processed();

        void MarkAsComplete();
        
        void MarkAsProcessed();

        bool Exists();
    }
}