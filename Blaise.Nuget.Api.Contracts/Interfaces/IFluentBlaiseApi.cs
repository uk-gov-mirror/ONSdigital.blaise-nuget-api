using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseApi : IFluentBlaiseCaseApi, IFluentBlaiseSurveyApi, IFluentBlaiseServerParkApi
    {
        IFluentBlaiseApi Server(string serverName);

        IFluentBlaiseApi Instrument(string instrumentName);

        IFluentBlaiseApi ServerPark(string serverParkName);

        IFluentBlaiseCaseApi Case(string primaryKeyValue);

        IFluentBlaiseCaseApi Case(IDataRecord caseDataRecord);
    }
}
