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
    public class DataService : IDataService
    {
        private readonly IDataModelService _dataModelService;
        private readonly IDataRecordService _dataRecordService;
        private readonly IKeyService _keyService;
        private readonly IFieldService _fieldService;
        private readonly IDataMapperService _mapperService;

        public DataService(
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

        public IDataSet GetDataSet(string filePath)
        {
            return _dataRecordService.GetDataSet(filePath);
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

        public IDataRecord GetDataRecord(IKey key, string filePath)
        {
            return _dataRecordService.GetDataRecord(key, filePath);
        }

        public void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            _dataRecordService.WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public void WriteDataRecord(IDataRecord dataRecord, string filePath)
        {
            _dataRecordService.WriteDataRecord(dataRecord, filePath);
        }

        public bool CompletedFieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            return _fieldService.CompletedFieldExists(connectionModel, instrumentName, serverParkName);
        }

        public bool CaseHasBeenCompleted(IDataRecord dataRecord)
        {
            return _fieldService.CaseHasBeenCompleted(dataRecord);
        }

        public void MarkCaseAsComplete(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            _fieldService.MarkCaseAsComplete(connectionModel, dataRecord, instrumentName, serverParkName);
        }

        public bool ProcessedFieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName)
        {
            return _fieldService.ProcessedFieldExists(connectionModel, instrumentName, serverParkName);
        }

        public bool CaseHasBeenProcessed(IDataRecord dataRecord)
        {
            return _fieldService.CaseHasBeenProcessed(dataRecord);
        }

        public void MarkCaseAsProcessed(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            _fieldService.MarkCaseAsProcessed(connectionModel, dataRecord, instrumentName, serverParkName);
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

        public void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, Dictionary<string, string> fieldData, string instrumentName, string serverParkName)
        {
            dataRecord = _mapperService.MapDataRecordFields(dataRecord, fieldData);

            WriteDataRecord(connectionModel, dataRecord, instrumentName, serverParkName);
        }
    }
}
