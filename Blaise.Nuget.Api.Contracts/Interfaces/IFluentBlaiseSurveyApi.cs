using Blaise.Nuget.Api.Contracts.Enums;


namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseSurveyApi
    {
        IFluentBlaiseSurveyApi WithField(FieldNameType fieldType);

        SurveyType Type { get; }

        bool Exists { get; }
    }
}