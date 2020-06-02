using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System.Collections.Generic;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseApi : IFluentBlaiseLocalApi, IFluentBlaiseRemoteApi, IFluentBlaiseCaseApi
    {
        IFluentBlaiseApi Server(string serverName);

        IEnumerable<string> GetServerParkNames();

        IEnumerable<ISurvey> GetAllSurveys();

        bool ServerParkExists(string serverParkName);

        IKey GetKey(IDatamodel dataModel, string keyName);

        IKey GetPrimaryKey(IDatamodel dataModel);

        string GetPrimaryKeyValue(IDataRecord dataRecord);

        void AssignPrimaryKeyValue(IKey key, string primaryKeyValue);

        IDataRecord GetDataRecord(IDatamodel dataModel);

        bool CaseHasBeenCompleted(IDataRecord dataRecord);

        bool CaseHasBeenProcessed(IDataRecord dataRecord);
    }
}
