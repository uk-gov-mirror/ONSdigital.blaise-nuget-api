using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseSurveyApi
    {
        IEnumerable<ISurvey> All();

        SurveyType Type();

        bool CompletedFieldExists();

        bool ProcessedFieldExists();

        bool Exists();
    }
}