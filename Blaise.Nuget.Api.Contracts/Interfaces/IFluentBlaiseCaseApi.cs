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

        IFluentBlaiseCaseApi WithStatus(CaseStatusType statusType);

        string PrimaryKey { get; }

        string CaseId { get; }

        decimal HOut { get; }

        bool Completed { get; }

        bool Processed { get; }

        WebFormStatusType WebFormStatus { get; }

        bool Exists { get; }

        void Add();

        void Update();

        void Remove();

        IDataRecord Get();

        IFluentBlaiseHandler Copy { get; }

        IFluentBlaiseHandler Move { get; }
    }
}