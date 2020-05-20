using System;
using System.Collections.Generic;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IParkService
    {
        IEnumerable<string> GetServerParkNames();

        bool ServerParkExists(string serverParkName);

        IEnumerable<string> GetSurveyNames(string serverParkName);

        IEnumerable<ISurvey> GetSurveys(string serverParkName);

        IEnumerable<ISurvey> GetAllSurveys();

        Guid GetInstrumentId(string instrumentName, string serverParkName);
    }
}
