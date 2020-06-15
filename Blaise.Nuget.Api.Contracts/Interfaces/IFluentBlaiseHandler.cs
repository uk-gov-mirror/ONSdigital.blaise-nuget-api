using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IFluentBlaiseHandler
    {
        IFluentBlaiseHandler ToConnection(ConnectionModel connectionModel);

        IFluentBlaiseHandler ToServerPark(string serverParkName);

        IFluentBlaiseHandler ToFile(string filePath);

        IFluentBlaiseHandler ToInstrument(string instrumentName);

        void Handle();
    }
}