
using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Services
{
    public class SurveyService : ISurveyService
    {
        private readonly IParkService _parkService;

        public SurveyService(IParkService parkService)
        {
            _parkService = parkService;
        }

        public IEnumerable<string> GetSurveyNames(string serverParkName)
        {
            var surveys = GetSurveys(serverParkName);

            return surveys.Select(sp => sp.Name);
        }

        public IEnumerable<ISurvey> GetSurveys(string serverParkName)
        {
            var serverPark = _parkService.GetServerPark(serverParkName);

            if (!serverPark.Surveys.Any())
            {
                throw new DataNotFoundException($"No surveys found for server park '{serverParkName}'");
            }

            return serverPark.Surveys;
        }

        public ISurvey GetSurvey(string instrumentName, string serverParkName)
        {
            var surveys = GetSurveys(serverParkName);
            var survey = surveys.FirstOrDefault(s => s.Name == instrumentName);

            if (survey == null)
            {
                throw new DataNotFoundException($"No survey found for instrument name '{instrumentName}'");
            }

            return survey;
        }

        public IEnumerable<ISurvey> GetAllSurveys()
        {
            var surveyList = new List<ISurvey>();
            var serverParkNames = _parkService.GetServerParkNames();

            foreach (var serverParkName in serverParkNames)
            {
                var serverPark = _parkService.GetServerPark(serverParkName);
                surveyList.AddRange(serverPark.Surveys);
            }

            return surveyList;
        }

        public Guid GetInstrumentId(string instrumentName, string serverParkName)
        {
            var serverPark = _parkService.GetServerPark(serverParkName);
            var survey = serverPark.Surveys.FirstOrDefault(s => string.Equals(s.Name, instrumentName, StringComparison.OrdinalIgnoreCase));

            if (survey == null)
            {
                throw new DataNotFoundException($"Instrument '{instrumentName}' not found on server park '{serverParkName}'");
            }

            return survey.InstrumentID;
        }
    }
}
