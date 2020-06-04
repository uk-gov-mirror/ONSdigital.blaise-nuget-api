using Blaise.Nuget.Api.Contracts.Enums;


namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseSurveyApi
    {
        SurveyType Type();

        IFluentBlaiseSurveyApi WithField(FieldNameType fieldType);

        bool Exists();
    }
}