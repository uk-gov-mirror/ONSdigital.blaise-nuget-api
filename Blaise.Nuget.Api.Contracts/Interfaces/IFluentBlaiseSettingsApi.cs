
namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseSettingsApi
    {
        IFluentBlaiseSettingsApi WithSourceFolder(string folderPath);
        IFluentBlaiseApi ToBucket(string bucketName, string folderName = null);
    }
}