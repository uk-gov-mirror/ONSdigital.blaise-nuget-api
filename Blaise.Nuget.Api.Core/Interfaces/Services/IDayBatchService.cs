using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDayBatchService
    {
        void CreateDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime dayBatchDate);
        List<DateTime> GetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        void SetSurveyDay(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            DateTime surveyDay);
        void SetSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName, List<DateTime> surveyDays);

        void RemoveSurveyDay(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            DateTime surveyDay);

        void RemoveSurveyDays(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            List<DateTime> surveyDays);
    }
}