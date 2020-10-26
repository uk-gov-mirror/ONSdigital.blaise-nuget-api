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

        bool HasField(FieldNameType fieldNameType);

        string PrimaryKey { get; }

        decimal HOut { get; }

        bool Exists { get; }

        void Add();

        void Update();

        void Remove();

        IDataRecord Get();
    }
}