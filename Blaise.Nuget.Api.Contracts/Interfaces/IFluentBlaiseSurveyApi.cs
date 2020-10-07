using Blaise.Nuget.Api.Contracts.Enums;


namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseSurveyApi
    {
        SurveyType Type { get; }

        IFluentBlaiseSurveyApi ToPath(string destinationPath);

        IFluentBlaiseApi ToBucket(string bucketName, string folderName = null);

        bool HasField(FieldNameType fieldNameType);

        bool Exists { get; }
    }
}