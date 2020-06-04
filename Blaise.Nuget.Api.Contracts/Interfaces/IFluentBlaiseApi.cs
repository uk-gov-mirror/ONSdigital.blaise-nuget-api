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

        IEnumerable<string> ServerParks();

        IFluentBlaiseCaseApi Case(string primaryKeyValue);

        IFluentBlaiseCaseApi Case(IDataRecord caseDataRecord);

        IDataSet Cases();

        IFluentBlaiseSurveyApi Survey();

        IEnumerable<ISurvey> Surveys();

        IFluentBlaiseUserApi User(string userName);

        bool Exists();
    }
}
