using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Core.Services
{
    public class CatiService : ICatiService
    {
        private readonly IRemoteCatiManagementServerProvider _remoteCatiManagementServerProvider;
        private readonly ISurveyService _surveyService;

        public CatiService(
            IRemoteCatiManagementServerProvider remoteCatiManagementServerProvider,
            ISurveyService surveyService)
        {
            _remoteCatiManagementServerProvider = remoteCatiManagementServerProvider;
            _surveyService = surveyService;
        }

        public IEnumerable<ISurvey> GetInstalledSurveys(ConnectionModel connectionModel, string serverPark)
        {
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverPark);
            var instruments = catiManagement.GetInstalledSurveys().Keys;

            return instruments.Select(instrument => _surveyService.GetSurvey(connectionModel, instrument, serverPark)).ToList();
        }

        public void CreateDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime dayBatchDate)
        {
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);

            if (catiManagement.LoadCatiInstrumentManager(instrumentName)
                .Specification
                .SurveyDays
                .All(d => d.Date.Date != dayBatchDate.Date))
            {
                throw new DataNotFoundException($"A survey day does not exist for the required daybatch date '{dayBatchDate.Date}'");
            }

            catiManagement
                .LoadCatiInstrumentManager(instrumentName)
                .CreateDaybatch(dayBatchDate);
        }

        public List<DateTime> GetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var surveyDays = new List<DateTime>();
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var surveyDateCollection = catiManagement
                .LoadCatiInstrumentManager(instrumentName)?.Specification?.SurveyDays;

            if (surveyDateCollection == null || surveyDateCollection.Count == 0)
            {
                return surveyDays;
            }

            surveyDays.AddRange(surveyDateCollection.Select(surveyDay => surveyDay.Date));

            return surveyDays;
        }

        public void SetSurveyDay(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime surveyDay)
        {
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentName);

            catiManager.Specification.SurveyDays
                .AddSurveyDay(surveyDay);

            catiManager.SaveSpecification();
        }

        public void SetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName, List<DateTime> surveyDays)
        {
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentName);

            catiManager.Specification.SurveyDays
                .AddSurveyDays(surveyDays);

            catiManager.SaveSpecification();
        }

        public void RemoveSurveyDay(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            DateTime surveyDay)
        {
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentName);

            catiManager.Specification.SurveyDays
                .RemoveSurveyDay(surveyDay);

            catiManager.SaveSpecification();
        }

        public void RemoveSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            List<DateTime> surveyDays)
        {
            var catiManagement = _remoteCatiManagementServerProvider.GetCatiManagementForServerPark(connectionModel, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentName);

            catiManager.Specification.SurveyDays
                .RemoveSurveyDays(surveyDays);

            catiManager.SaveSpecification();
        }
    }
}
