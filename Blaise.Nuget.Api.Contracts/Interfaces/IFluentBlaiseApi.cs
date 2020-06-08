using System.Collections.Generic;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseApi
    {
        IFluentBlaiseApi WithServer(string serverName);

        IFluentBlaiseApi WithInstrument(string instrumentName);

        IFluentBlaiseApi WithServerPark(string serverParkName);

        IEnumerable<string> ServerParks { get; }

        IDataSet Cases { get; }

        IFluentBlaiseUserApi User { get; }

        IFluentBlaiseCaseApi Case { get; }

        IFluentBlaiseSurveyApi Survey { get; }

        IEnumerable<ISurvey> Surveys { get; }

        bool Exists { get; }
    }
}
