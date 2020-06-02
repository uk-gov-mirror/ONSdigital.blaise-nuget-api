using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseSurveyApi
    {
        IEnumerable<ISurvey> Surveys();

        SurveyType SurveyType();

        bool CompletedFieldExists();

        bool ProcessedFieldExists();
    }
}