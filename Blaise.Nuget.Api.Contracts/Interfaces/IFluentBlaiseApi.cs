using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseApi : IFluentBlaiseLocalApi, IFluentBlaiseRemoteApi
    {
        IEnumerable<string> GetServerParkNames();

        bool ServerParkExists(string serverParkName);

        IEnumerable<string> GetSurveys();

        IKey GetKey(IDatamodel dataModel, string keyName);

        IDataRecord GetDataRecord(IDatamodel dataModel);
    }
}
