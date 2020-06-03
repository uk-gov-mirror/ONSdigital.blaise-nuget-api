using System.Collections.Generic;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseApi
    {
        IFluentBlaiseApi Server(string serverName);

        IFluentBlaiseApi Instrument(string instrumentName);

        IFluentBlaiseApi ServerPark(string serverParkName);

        IFluentBlaiseCaseApi Case(string primaryKeyValue);

        IFluentBlaiseCaseApi Case(IDataRecord caseDataRecord);

        IDataSet Cases();

        IFluentBlaiseUserApi User(string userName);

        IFluentBlaiseSurveyApi Survey();

        IEnumerable<ISurvey> Surveys();

        bool Exists();
    }
}
