using System;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class DataModelService : IDataModelService
    {
        private readonly IRemoteDataLinkProvider _remoteDataLinkProvider;

        public DataModelService(IRemoteDataLinkProvider remoteDataLinkProvider)
        {
            _remoteDataLinkProvider = remoteDataLinkProvider;
        }

        public IDatamodel GetDataModel(string instrumentName, string serverParkName)
        {
            var dataLink = _remoteDataLinkProvider.GetDataLink(instrumentName, serverParkName);

            if (dataLink?.Datamodel == null)
            {
                throw new NullReferenceException($"No datamodel was found for instrument '{instrumentName}' on server park '{serverParkName}'");
            }

            return dataLink.Datamodel;
        }

        public SurveyType GetSurveyType(string instrumentName, string serverParkName)
        {
            var dataModel = GetDataModel(instrumentName, serverParkName);

            switch (dataModel.Name)
            {
                case "Appointment":
                    return SurveyType.Appointment;
                case "CatiDial":
                    return SurveyType.CatiDial;
                default:
                    return SurveyType.NotMapped;
            }
        }
    }
}
