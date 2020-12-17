using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Helpers;
using Blaise.Nuget.Api.Providers;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Api
{
    public class BlaiseCaseApi : IBlaiseCaseApi
    {
        private readonly ICaseService _caseService;
        private readonly ConnectionModel _connectionModel;

        internal BlaiseCaseApi(
            ICaseService caseService,
            ConnectionModel connectionModel)
        {
            _caseService = caseService;
            _connectionModel = connectionModel;
        }

        public BlaiseCaseApi(ConnectionModel connectionModel = null)
        {
           var unityProvider = new UnityProvider();
           unityProvider.RegisterDependencies();

           _caseService = unityProvider.Resolve<ICaseService>();

           var configurationProvider = unityProvider.Resolve<IBlaiseConfigurationProvider>();
           _connectionModel = connectionModel ?? configurationProvider.GetConnectionModel();
        }

        public bool CaseExists(string primaryKeyValue, string instrumentName, 
            string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.CaseExists(_connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public string GetPrimaryKeyValue(IDataRecord dataRecord)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetPrimaryKeyValue(dataRecord);
        }

        public IDataSet GetCases(string databaseFile)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetDataSet(databaseFile);
        }

        public IDataSet GetCases(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetDataSet(_connectionModel, instrumentName, serverParkName);
        }

        public IDataRecord GetCase(string primaryKeyValue, string instrumentName, 
            string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetDataRecord(_connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public void CreateCase(string primaryKeyValue, Dictionary<string, string> fieldData, 
            string instrumentName, string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.CreateNewDataRecord(_connectionModel, primaryKeyValue, fieldData, instrumentName, serverParkName);
        }

        public void CreateCase(string databaseFile, string primaryKeyValue, Dictionary<string, string> fieldData)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            fieldData.ThrowExceptionIfNull("fieldData");

            _caseService.CreateNewDataRecord(databaseFile, primaryKeyValue, fieldData);
        }

        public void UpdateCase(IDataRecord dataRecord, Dictionary<string, string> fieldData,
            string instrumentName, string serverParkName)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");
            fieldData.ThrowExceptionIfNull("fieldData");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.UpdateDataRecord(_connectionModel, dataRecord, fieldData, instrumentName, serverParkName);
        }

        public bool FieldExists(string instrumentName, string serverParkName, FieldNameType fieldNameType)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.FieldExists(_connectionModel, instrumentName, serverParkName, fieldNameType);
        }

        public bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.FieldExists(dataRecord, fieldNameType);
        }

        public IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            dataRecord.ThrowExceptionIfNull("dataRecord");

            return _caseService.GetFieldValue(dataRecord, fieldNameType);
        }

        public IDataValue GetFieldValue(string primaryKeyValue, string instrumentName,
            string serverParkName, FieldNameType fieldNameType)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            var dataRecord = _caseService.GetDataRecord(_connectionModel, primaryKeyValue, instrumentName, serverParkName);

            return GetFieldValue(dataRecord, fieldNameType);
        }

        public void RemoveCase(string primaryKeyValue, string instrumentName, 
            string serverParkName)
        {
            primaryKeyValue.ThrowExceptionIfNullOrEmpty("primaryKeyValue");
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            _caseService.RemoveDataRecord(_connectionModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public int GetNumberOfCases(string instrumentName, string serverParkName)
        {
            instrumentName.ThrowExceptionIfNullOrEmpty("instrumentName");
            serverParkName.ThrowExceptionIfNullOrEmpty("serverParkName");

            return _caseService.GetNumberOfCases(_connectionModel, instrumentName, serverParkName);
        }

        public int GetNumberOfCases(string databaseFile)
        {
            databaseFile.ThrowExceptionIfNullOrEmpty("databaseFile");

            return _caseService.GetNumberOfCases(databaseFile);
        }
    }
}
