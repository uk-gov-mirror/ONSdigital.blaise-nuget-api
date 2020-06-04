using System.Collections.Generic;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseApi
    {
        IFluentBlaiseApi WithServer(string serverName);

        IFluentBlaiseApi WithInstrument(string instrumentName);

        IFluentBlaiseApi WithServerPark(string serverParkName);

        IEnumerable<string> ServerParks();

        IFluentBlaiseCaseApi WithCase(string primaryKeyValue);

        IFluentBlaiseCaseApi WithCase(IDataRecord caseDataRecord);

        IDataSet Cases();

        IFluentBlaiseUserApi WithUser(string userName);

        IFluentBlaiseSurveyApi Survey { get; }

        IEnumerable<ISurvey> Surveys();

        bool Exists();
    }
}
