using System;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Core.Interfaces.Services
{
    public interface IDataInterfaceService
    {
        void UpdateDataInterfaceConnection(ConnectionModel connectionModel, Guid instrumentGuid);
    }
}