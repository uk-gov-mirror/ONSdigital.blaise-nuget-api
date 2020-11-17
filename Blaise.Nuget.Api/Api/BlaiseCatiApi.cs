using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Helpers;
using Blaise.Nuget.Api.Providers;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseCatiApi : IBlaiseCatiApi
    {
        private readonly ISurveyService _surveyService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseCatiApi(
            ISurveyService surveyService,
            ConnectionModel connectionModel)
        {
            _surveyService = surveyService;
            _connectionModel = connectionModel;
        }

        public BlaiseCatiApi(ConnectionModel connectionModel = null)
        {
            var unityProvider = new UnityProvider();
            unityProvider.RegisterDependencies();

            _surveyService = unityProvider.Resolve<ISurveyService>();
            
            var configurationProvider = unityProvider.Resolve<IConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }
        
        public void CreateDayBatch(string instrumentName, string serverParkName, DateTime dayBatchDate)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _surveyService.CreateDayBatch(_connectionModel, instrumentName, serverParkName, dayBatchDate);
        }
    }
}
