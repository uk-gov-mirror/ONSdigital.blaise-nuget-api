using System;
using Blaise.Nuget.Api.Contracts.Models;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Interfaces.Factories
{
    public interface IDataInterfaceFactory
    {
        IDataInterface GetConnection(ConnectionModel connectionModel, Guid instrumentGuid);
    }
}