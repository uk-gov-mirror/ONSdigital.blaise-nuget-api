using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IFileService
    {
        void UpdateInstrumentFileWithData(ConnectionModel connectionModel, string instrumentFile,
            string instrumentName, string serverParkName);

        void UpdateInstrumentPackageWithSqlConnection(string instrumentName,
            string instrumentFile);
    }
}