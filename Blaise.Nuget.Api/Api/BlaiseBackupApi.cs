using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Helpers;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseBackupApi : IBlaiseBackupApi
    {
        private readonly ICaseService _caseService;
        private readonly ISurveyService _surveyService;
        private readonly IFileService _fileService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseBackupApi(
            ICaseService caseService,
            ISurveyService surveyService,
            IFileService fileService,
            ConnectionModel connectionModel)
        {
            _caseService = caseService;
            _surveyService = surveyService;
            _fileService = fileService;
            _connectionModel = connectionModel;
        }

        public BlaiseBackupApi(ConnectionModel connectionModel = null)
        {
            var unityProvider = new UnityProvider();
            unityProvider.RegisterDependencies();

            _caseService = unityProvider.Resolve<ICaseService>();
            _surveyService = unityProvider.Resolve<ISurveyService>();
            _fileService = unityProvider.Resolve<IFileService>();

            var configurationProvider = unityProvider.Resolve<IConfigurationProvider>();
            _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public string BackupSurveyToFile(string serverParkName, string instrumentName, string destinationFilePath)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");
            destinationFilePath.ThrowExceptionIfNullOrEmpty("destinationFilePath");

            if (_fileService.DatabaseFileExists(destinationFilePath, instrumentName))
            {
                _fileService.DeleteDatabaseFile(destinationFilePath, instrumentName);
            }

            var metaFileName = _surveyService.GetMetaFileName(_connectionModel, instrumentName, serverParkName);
            var databaseFile =_fileService.CreateDatabaseFile(metaFileName, destinationFilePath, instrumentName);

            var cases = _caseService.GetDataSet(_connectionModel, instrumentName, serverParkName);

            while (!cases.EndOfSet)
            {
                _caseService.WriteDataRecord((IDataRecord2)cases.ActiveRecord, databaseFile);

                cases.MoveNext();
            }

            return databaseFile;
        }
    }
}
