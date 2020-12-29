namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseFileApi
    {
        string CreateInstrumentFile(string serverParkName, string instrumentName, 
            string destinationFilePath);
    }
}