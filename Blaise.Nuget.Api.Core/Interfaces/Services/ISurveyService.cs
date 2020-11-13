using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface ISurveyService
    {
        void InstallInstrument(ConnectionModel connectionModel, string serverParkName, string instrumentPath);
        void UninstallInstrument(ConnectionModel connectionModel, string serverParkName, string instrumentName);
        IEnumerable<string> GetSurveyNames(ConnectionModel connectionModel, string serverParkName);

        bool SurveyExists(ConnectionModel connectionModel, string instrumentName, string serverParkName);
        IEnumerable<ISurvey> GetSurveys(ConnectionModel connectionModel, string serverParkName);

        IEnumerable<ISurvey> GetAllSurveys(ConnectionModel connectionModel);

        Guid GetInstrumentId(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        string GetMetaFileName(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        void CreateDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime dayBatchDate);
    }
}