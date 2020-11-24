using System;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DataModelService : IDataModelService
    {
        private readonly IRemoteDataLinkProvider _remoteDataLinkProvider;
        private readonly ILocalDataLinkProvider _localDataLinkProvider;

        public DataModelService(
            IRemoteDataLinkProvider remoteDataLinkProvider,
            ILocalDataLinkProvider localDataLinkProvider)
        {
            _remoteDataLinkProvider = remoteDataLinkProvider;
            _localDataLinkProvider = localDataLinkProvider;
        }

        public IDatamodel GetDataModel(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(connectionModel, instrumentName, serverParkName);

            if (dataLink?.Datamodel == null)
            {
                throw new NullReferenceException($"No datamodel was found for instrument '{instrumentName}' on server park '{serverParkName}'");
            }

            return dataLink.Datamodel;
        }

        public IDatamodel GetDataModel(string databaseFile)
        {
            var dataLink = _localDataLinkProvider.GetDataLink(databaseFile);

            if (dataLink?.Datamodel == null)
            {
                throw new NullReferenceException($"No datamodel was found for file '{databaseFile}'");
            }

            return dataLink.Datamodel;
        }
    }
}
