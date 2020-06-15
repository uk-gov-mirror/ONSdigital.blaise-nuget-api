using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseApi
    {
        IFluentBlaiseApi WithConnection(ConnectionModel connectionModel);

        IFluentBlaiseApi WithInstrument(string instrumentName);

        IFluentBlaiseApi WithServerPark(string serverParkName);

        IFluentBlaiseApi WithFile(string filePath);

        IEnumerable<string> ServerParks { get; }

        IDataSet Cases { get; }

        IFluentBlaiseUserApi User { get; }

        IFluentBlaiseCaseApi Case { get; }

        IFluentBlaiseSurveyApi Survey { get; }

        IEnumerable<ISurvey> Surveys { get; }

        bool Exists { get; }

        ConnectionModel DefaultConnection { get; }
    }
}
