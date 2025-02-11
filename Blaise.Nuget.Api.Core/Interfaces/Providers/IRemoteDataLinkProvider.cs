﻿
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Core.Interfaces.Providers
{
    public interface IRemoteDataLinkProvider
    {
        void LockDataRecord(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            IKey primaryKey, string lockId);

        void UnLockDataRecord(ConnectionModel connectionModel, string instrumentName, string serverParkName,
            IKey primaryKey, string lockId);

        IDataLink4 GetDataLink(ConnectionModel connectionModel, string instrumentName, string serverParkName);
    }
}
