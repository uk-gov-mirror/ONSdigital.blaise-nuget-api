﻿namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseFileApi
    {
        void UpdateInstrumentFileWithData(string serverParkName, string instrumentName, string instrumentFile);

        void UpdateInstrumentFileWithSqlConnection(string instrumentName, string instrumentFile);
    }
}