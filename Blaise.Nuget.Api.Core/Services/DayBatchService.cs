using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.Cati.Runtime;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DayBatchService : IDayBatchService
    {
        private readonly ICatiManagementServerFactory _catiServerFactory;

        public DayBatchService(ICatiManagementServerFactory catiServerFactory)
        {
            _catiServerFactory = catiServerFactory;
        }

        public void CreateDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime dayBatchDate)
        {
            var catiManagement = GetCatiManagementForServerPark(connectionModel, serverParkName);

            catiManagement
                .LoadCatiInstrumentManager(instrumentName)
                .CreateDaybatch(dayBatchDate);
        }

        public List<DateTime> GetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var surveyDays = new List<DateTime>();
            var catiManagement = GetCatiManagementForServerPark(connectionModel, serverParkName);
            var surveyDateCollection = catiManagement
                .LoadCatiInstrumentManager(instrumentName).Specification.SurveyDays;

            if (surveyDateCollection == null || surveyDateCollection.Count == 0)
            {
                return surveyDays;
            }

            surveyDays.AddRange(surveyDateCollection.Select(surveyDay => surveyDay.Date));

            return surveyDays;
        }

        public void SetSurveyDay(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime surveyDay)
        {
            var catiManagement = GetCatiManagementForServerPark(connectionModel, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentName);
            
            catiManager.Specification.SurveyDays
                .AddSurveyDay(surveyDay);

            catiManager.SaveSpecification();
        }

        public void SetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName, List<DateTime> surveyDays)
        {
            var catiManagement = GetCatiManagementForServerPark(connectionModel, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentName);

            catiManager.Specification.SurveyDays
                .AddSurveyDays(surveyDays);
            
            catiManager.SaveSpecification();
        }

        public void RemoveSurveyDay(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            DateTime surveyDay)
        {
            var catiManagement = GetCatiManagementForServerPark(connectionModel, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentName);

            catiManager.Specification.SurveyDays
                .RemoveSurveyDay(surveyDay);

            catiManager.SaveSpecification();
        }

        public void RemoveSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            List<DateTime> surveyDays)
        {
            var catiManagement = GetCatiManagementForServerPark(connectionModel, serverParkName);
            var catiManager = catiManagement.LoadCatiInstrumentManager(instrumentName);

            catiManager.Specification.SurveyDays
                .RemoveSurveyDays(surveyDays);

            catiManager.SaveSpecification();
        }

        private IRemoteCatiManagementServer GetCatiManagementForServerPark(ConnectionModel connectionModel, string serverParkName)
        {
            var catiManagement = _catiServerFactory.GetConnection(connectionModel);

            catiManagement.SelectServerPark(serverParkName);

            return catiManagement;
        }
    }
}
