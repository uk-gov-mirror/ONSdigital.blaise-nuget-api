using Blaise.Nuget.Api.Contracts.Enums;


namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseSurveyApi
    {
        SurveyType Type { get; }

        IFluentBlaiseSurveyApi ToPath(string filePath);

        IFluentBlaiseSurveyApi ToBucket(string bucketName);

        bool HasField(FieldNameType fieldNameType);

        void Backup();

        bool Exists { get; }
    }
}