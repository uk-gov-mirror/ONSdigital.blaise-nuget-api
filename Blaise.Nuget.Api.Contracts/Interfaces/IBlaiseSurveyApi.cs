using System;
using System.Collections.Generic;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseSurveyApi
    {
        bool SurveyExists(string instrumentName, string serverParkName);

        IEnumerable<ISurvey> GetSurveysAcrossServerParks();

        IEnumerable<ISurvey> GetSurveys(string serverParkName);

        IEnumerable<string> GetNamesOfSurveys(string serverParkName);

        Guid GetIdOfSurvey(string instrumentName, string serverParkName);

        void InstallSurvey(string serverParkName, string instrumentFile);

        void UninstallSurvey(string serverParkName, string instrumentName);

        void CreateDayBatch(string instrumentName, string serverParkName, DateTime dayBatchDate);
    }
}