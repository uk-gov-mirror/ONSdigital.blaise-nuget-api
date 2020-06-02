using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseApi : IFluentBlaiseLocalApi, IFluentBlaiseRemoteApi, IFluentBlaiseCaseApi, IFluentBlaiseSurveyApi
    {
        IFluentBlaiseApi Server(string serverName);

        IFluentBlaiseApi Instrument(string instrumentName);

        IFluentBlaiseApi ServerPark(string serverParkName);

        IFluentBlaiseCaseApi Case(string primaryKeyValue);

        IFluentBlaiseCaseApi Case(IDataRecord caseDataRecord);

        IEnumerable<string> GetServerParkNames();

        bool ServerParkExists(string serverParkName);

        IKey GetKey(IDatamodel dataModel, string keyName);

        IKey GetPrimaryKey(IDatamodel dataModel);

        void AssignPrimaryKeyValue(IKey key, string primaryKeyValue);

        IDataRecord GetDataRecord(IDatamodel dataModel);

        bool CaseHasBeenCompleted(IDataRecord dataRecord);

        bool CaseHasBeenProcessed(IDataRecord dataRecord);
    }
}
