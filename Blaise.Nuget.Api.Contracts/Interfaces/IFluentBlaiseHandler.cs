namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseHandler
    {
        IFluentBlaiseHandler ToServer(string serverName);

        IFluentBlaiseHandler ToServerPark(string serverParkName);

        IFluentBlaiseHandler ToFile(string filePath);

        IFluentBlaiseHandler ToInstrument(string instrumentName);

        void Handle();
    }
}