using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Core.Services
{
    public class CaseService : ICaseService
    {
        private readonly IDataModelService _dataModelService;
        private readonly IDataRecordService _dataRecordService;
        private readonly IKeyService _keyService;
        private readonly IFieldService _fieldService;
        private readonly IDataMapperService _mapperService;

        public CaseService(
            IDataModelService dataModelService, 
            IDataRecordService dataRecordService, 
            IKeyService keyService, 
            IFieldService fieldService, 
            IDataMapperService mapperService)
        {
            _dataModelService = dataModelService;
            _dataRecordService = dataRecordService;
            _keyService = keyService;
            _fieldService = fieldService;
            _mapperService = mapperService;
        }

        public IDatamodel GetDataModel(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            return _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
        }

        public IDatamodel GetDataModel(string databaseFile)
        {
            return _dataModelService.GetDataModel(databaseFile);
        }

        public SurveyType GetSurveyType(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            return _dataModelService.GetSurveyType(connectionModel, instrumentName, serverParkName);
        }

        public IKey GetKey(IDatamodel dataModel, string keyName)
        {
            return _keyService.GetKey(dataModel, keyName);
        }

        public IKey GetPrimaryKey(IDatamodel dataModel)
        {
            return _keyService.GetPrimaryKey(dataModel);
        }

        public bool KeyExists(ConnectionModel connectionModel, IKey key, string instrumentName, string serverParkName)
        {
            return _keyService.KeyExists(connectionModel, key, instrumentName, serverParkName);
        }

        public string GetPrimaryKeyValue(IDataRecord dataRecord)
        {
            return _keyService.GetPrimaryKeyValue(dataRecord);
        }

        public void AssignPrimaryKeyValue(IKey key, string primaryKeyValue)
        {
            _keyService.AssignPrimaryKeyValue(key, primaryKeyValue);
        }

        public IDataSet GetDataSet(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            return _dataRecordService.GetDataSet(connectionModel, instrumentName, serverParkName);
        }

        public IDataSet GetDataSet(string databaseFile)
        {
            return _dataRecordService.GetDataSet(databaseFile);
        }

        public IDataRecord GetDataRecord(IDatamodel datamodel)
        {
            return _dataRecordService.GetDataRecord(datamodel);
        }

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, IKey key, string instrumentName, string serverParkName)
        {
            return _dataRecordService.GetDataRecord(connectionModel, key, instrumentName, serverParkName);
        }

        public IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);

            _keyService.AssignPrimaryKeyValue(primaryKey, primaryKeyValue);

            return GetDataRecord(connectionModel, primaryKey, instrumentName, serverParkName);
        }

        public IDataRecord GetDataRecord(IKey key, string databaseFile)
        {
            return _dataRecordService.GetDataRecord(key, databaseFile);
        }

        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public void WriteDataRecord(IDataRecord dataRecord, string databaseFile)
        {
            _dataRecordService.WriteDataRecord(dataRecord, databaseFile);
        }

        public bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName, FieldNameType fieldNameType)
        {
            return _fieldService.FieldExists(connectionModel, instrumentName, serverParkName, fieldNameType);
        }

        public void RemoveDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);

            _keyService.AssignPrimaryKeyValue(primaryKey, primaryKeyValue);

            _dataRecordService.DeleteDataRecord(connectionModel, primaryKey, instrumentName, serverParkName);
        }

        public IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            return _fieldService.GetField(dataRecord, fieldNameType).DataValue;
        }

        public bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType)
        {
            return _fieldService.FieldExists(dataRecord, fieldNameType);
        }

        public int GetNumberOfCases(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            return _dataRecordService.GetNumberOfRecords(connectionModel, instrumentName, serverParkName);
        }

        public int GetNumberOfCases(string databaseFile)
        {
            return _dataRecordService.GetNumberOfRecords(databaseFile);
        }

        public bool CaseExists(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(connectionModel, instrumentName, serverParkName);
            var primaryKey = _keyService.GetPrimaryKey(dataModel);

            _keyService.AssignPrimaryKeyValue(primaryKey, primaryKeyValue);

            return _keyService.KeyExists(connectionModel, primaryKey, instrumentName, serverParkName);
        }

        public void CreateNewDataRecord(ConnectionModel connectionModel, string primaryKeyValue, Dictionary<string, string> fieldData, string instrumentName, string serverParkName)
        {
            var dataModel = GetDataModel(connectionModel, instrumentName, serverParkName);
            var key = GetPrimaryKey(dataModel);
            var dataRecord = GetDataRecord(dataModel);

            dataRecord = _mapperService.MapDataRecordFields(dataRecord, dataModel, key, primaryKeyValue, fieldData);

            WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public void CreateNewDataRecord(string databaseFile, string primaryKeyValue, Dictionary<string, string> fieldData)
        {
            var dataModel = GetDataModel(databaseFile);
            var key = GetPrimaryKey(dataModel);
            var dataRecord = GetDataRecord(dataModel);

            dataRecord = _mapperService.MapDataRecordFields(dataRecord, dataModel, key, primaryKeyValue, fieldData);

            WriteDataRecord(dataRecord, databaseFile);
        }

        public void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, Dictionary<string, string> fieldData, string instrumentName, string serverParkName)
        {
            dataRecord = _mapperService.MapDataRecordFields(dataRecord, fieldData);

            WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }
    }
}
