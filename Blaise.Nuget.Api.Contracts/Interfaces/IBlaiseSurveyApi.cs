using System;
using System.Collections.Generic;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseSurveyApi
    {
        bool SurveyExistsOnServerPark(string instrumentName, string serverParkName);

        IEnumerable<ISurvey> GetSurveysInstalled();

        IEnumerable<ISurvey> GetSurveysInstalledOnServerPark(string serverParkName);

        IEnumerable<string> GetNamesOfSurveysInstalledOnServerPark(string serverParkName);

        Guid GetIdOfSurveyInstalledOnServerPark(string instrumentName, string serverParkName);

        void InstallSurveyOnServerPark(string serverParkName, string instrumentFile);

        void UninstallSurveyFromServerPark(string serverParkName, string instrumentName);

        void CreateDayBatchForSurveyInstalledOnServerPark(string instrumentName, string serverParkName, DateTime dayBatchDate);
    }
}