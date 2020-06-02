using System.Collections.Generic;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseCaseApi
    {
        void Create(Dictionary<string, string> data);

        bool Exists();

        IDataSet Cases();

        string PrimaryKeyValue();

        bool Completed();

        bool Processed();

        void MarkAsComplete();
        
        void MarkAsProcessed();
    }
}