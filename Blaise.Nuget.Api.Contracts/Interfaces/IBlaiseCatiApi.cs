using System;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseCatiApi
    {
        void CreateDayBatch(string instrumentName, string serverParkName, DateTime dayBatchDate);
    }
}