using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;

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
            var catiManagement = _catiServerFactory.GetConnection(connectionModel);

            catiManagement.SelectServerPark(serverParkName);
            catiManagement
                .LoadCatiInstrumentManager(instrumentName)
                .CreateDaybatch(dayBatchDate);
        }

        public List<DateTime> GetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var surveyDays = new List<DateTime>();

            var catiManagement = _catiServerFactory.GetConnection(connectionModel);

            catiManagement.SelectServerPark(serverParkName);

            var surveyDateCollection = catiManagement
                .LoadCatiInstrumentManager(instrumentName).Specification.SurveyDays;

            if (surveyDateCollection == null || surveyDateCollection.Count == 0) return surveyDays;

            surveyDays.AddRange(surveyDateCollection.Select(surveyDay => surveyDay.Date));

            return surveyDays;
        }
    }
}
