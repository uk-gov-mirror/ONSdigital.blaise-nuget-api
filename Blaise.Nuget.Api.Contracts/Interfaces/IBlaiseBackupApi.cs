namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseBackupApi
    {
        string BackupSurveyToFile(string serverParkName, string instrumentName, 
            string destinationFilePath);
    }
}