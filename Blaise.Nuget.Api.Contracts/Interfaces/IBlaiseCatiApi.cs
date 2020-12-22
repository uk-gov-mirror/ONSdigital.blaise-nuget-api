using System;
using System.Collections.Generic;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseCatiApi
    {
        void CreateDayBatch(string instrumentName, string serverParkName, DateTime dayBatchDate);
        List<DateTime> GetSurveyDays(string instrumentName, string serverParkName);
        void SetSurveyDay(string instrumentName, string serverParkName, DateTime surveyDay);
        void SetSurveyDays(string instrumentName, string serverParkName, List<DateTime> surveyDays);
    }
}