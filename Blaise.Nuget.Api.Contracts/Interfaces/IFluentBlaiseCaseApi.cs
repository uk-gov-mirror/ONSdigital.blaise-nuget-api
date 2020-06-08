using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseCaseApi
    {
        IFluentBlaiseCaseApi WithPrimaryKey(string primaryKeyValue);

        IFluentBlaiseCaseApi WithDataRecord(IDataRecord caseDataRecord);

        IFluentBlaiseCaseApi WithData(Dictionary<string, string> data);

        IFluentBlaiseCaseApi WithStatus(StatusType statusType);

        string PrimaryKeyValue();

        bool IsComplete();

        bool HasBeenProcessed();

        void Add();

        void Update();

        bool Exists { get; }
    }
}