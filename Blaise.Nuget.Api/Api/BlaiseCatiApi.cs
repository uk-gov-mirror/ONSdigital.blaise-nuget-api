using System;
using System.Collections.Generic;
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
        private readonly IDayBatchService _dayBatchService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseCatiApi(
            IDayBatchService dayBatchService,
            ConnectionModel connectionModel)
        {
            _dayBatchService = dayBatchService;
            _connectionModel = connectionModel;
        }

        public BlaiseCatiApi(ConnectionModel connectionModel = null)
        {
            var unityProvider = new UnityProvider();
            unityProvider.RegisterDependencies();

            _dayBatchService = unityProvider.Resolve<IDayBatchService>();
            
            var configurationProvider = unityProvider.Resolve<IBlaiseConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }
        
        public void CreateDayBatch(string instrumentName, string serverParkName, DateTime dayBatchDate)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _dayBatchService.CreateDayBatch(_connectionModel, instrumentName, serverParkName, dayBatchDate);
        }

        public List<DateTime> GetSurveyDays(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _dayBatchService.GetSurveyDays(_connectionModel, instrumentName, serverParkName);
        }
    }
}
