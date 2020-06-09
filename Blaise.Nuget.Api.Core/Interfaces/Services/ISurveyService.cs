using System;
using System.Collections.Generic;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ISurveyService
    {

        IEnumerable<string> GetSurveyNames(string serverParkName);

        IEnumerable<ISurvey> GetSurveys(string serverParkName);

        ISurvey GetSurvey(string instrumentName, string serverParkName);

        IEnumerable<ISurvey> GetAllSurveys();

        Guid GetInstrumentId(string instrumentName, string serverParkName);
    }
}