using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.Cati.Specification;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DayBatchService : IDayBatchService
    {
        private readonly IRemoteCatiServerProvider _catiServerProvider;

        public DayBatchService(IRemoteCatiServerProvider catiServerProvider)
        {
            _catiServerProvider = catiServerProvider;
        }

        public void CreateDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime dayBatchDate)
        {
            var catiManagement = _catiServerProvider.GetRemoteConnection(connectionModel);

            catiManagement.SelectServerPark(serverParkName);
            catiManagement
                .LoadCatiInstrumentManager(instrumentName)
                .CreateDaybatch(dayBatchDate);
        }

        public List<DateTime> GetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var surveyDays = new List<DateTime>();

            var catiManagement = _catiServerProvider.GetRemoteConnection(connectionModel);

            catiManagement.SelectServerPark(serverParkName);
            ISurveyDayCollection surveyDateCollection = catiManagement
                .LoadCatiInstrumentManager(instrumentName).Specification.SurveyDays;

            foreach (var surveyDay in surveyDateCollection)
            {
                surveyDays.Add(surveyDay.Date);
            }

            return surveyDays;
        }
    }
}
