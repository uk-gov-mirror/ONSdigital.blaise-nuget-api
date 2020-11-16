using Blaise.Nuget.Api.Helpers;
using Blaise.Nuget.Api.Providers;
using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api
{
    public class BlaiseSurveyApi : IBlaiseSurveyApi
    {
        private readonly ISurveyService _surveyService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseSurveyApi(
            ISurveyService surveyService,
            ConnectionModel connectionModel)
        {
            _surveyService = surveyService;
            _connectionModel = connectionModel;
        }

        public BlaiseSurveyApi(ConnectionModel connectionModel = null)
        {
            var unityProvider = new UnityProvider();
            unityProvider.RegisterDependencies();

            _surveyService = unityProvider.Resolve<ISurveyService>();
            
            var configurationProvider = unityProvider.Resolve<IConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public bool SurveyExistsOnServerPark(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.SurveyExists(_connectionModel, instrumentName, serverParkName);
        }

        public IEnumerable<ISurvey> GetSurveysInstalled()
        {
            return _surveyService.GetAllSurveys(_connectionModel);
        }

        public IEnumerable<ISurvey> GetSurveysInstalledOnServerPark(string serverParkName)
        { 
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetSurveys(_connectionModel, serverParkName);
        }

        public IEnumerable<string> GetNamesOfSurveysInstalledOnServerPark(string serverParkName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetSurveyNames(_connectionModel, serverParkName);
        }

        public Guid GetIdOfSurveyInstalledOnServerPark(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _surveyService.GetInstrumentId(_connectionModel, instrumentName, serverParkName);
        }

        public void InstallSurveyOnServerPark(string serverParkName, string instrumentFile)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            instrumentFile.ThrowExceptionIfNullOrEmpty("instrumentFile");

            _surveyService.InstallInstrument(_connectionModel, serverParkName, instrumentFile);
        }

        public void UninstallSurveyFromServerPark(string serverParkName, string instrumentName)
        {
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");

            _surveyService.UninstallInstrument(_connectionModel, serverParkName, instrumentName);
        }

        public void CreateDayBatchForSurveyInstalledOnServerPark(string instrumentName, string serverParkName, DateTime dayBatchDate)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _surveyService.CreateDayBatch(_connectionModel, instrumentName, serverParkName, dayBatchDate);
        }
    }
}
