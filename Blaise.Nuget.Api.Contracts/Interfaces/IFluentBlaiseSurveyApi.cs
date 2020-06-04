using Blaise.Nuget.Api.Contracts.Enums;


namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseSurveyApi
    {
        SurveyType Type();

        bool HasField(FieldNameType fieldType);

        bool Exists();
    }
}