using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ISurveyService
    {

        IEnumerable<string> GetSurveyNames(ConnectionModel connectionModel, string serverParkName);

        bool SurveyExists(ConnectionModel connectionModel, string instrumentName, string serverParkName);
        IEnumerable<ISurvey> GetSurveys(ConnectionModel connectionModel, string serverParkName);

        ISurvey GetSurvey(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IEnumerable<ISurvey> GetAllSurveys(ConnectionModel connectionModel);

        Guid GetInstrumentId(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        string GetMetaFileName(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        string GetDataFileName(ConnectionModel connectionModel, string instrumentName, string serverParkName);
    }
}