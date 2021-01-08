using System;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface IDataInterfaceFactory
    {
        IDataInterface GetDataInterface(ConnectionModel connectionModel, Guid instrumentGuid);

        IDataInterface GetDataInterfaceForSql(string databaseConnectionString);
    }
}