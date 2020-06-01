﻿using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
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

        public IDatamodel GetDataModel(string instrumentName, string serverParkName)
        {
            return _dataModelService.GetDataModel(instrumentName, serverParkName);
        }

        public IDatamodel GetDataModel(string serverName, string instrumentName, string serverParkName)
        {
            return _dataModelService.GetDataModel(serverName, instrumentName, serverParkName);
        }

        public CaseRecordType GetCaseRecordType(string instrumentName, string serverParkName)
        {
            return _dataModelService.GetCaseRecordType(instrumentName, serverParkName);
        }

        public IKey GetKey(IDatamodel dataModel, string keyName)
        {
            return _keyService.GetKey(dataModel, keyName);
        }

        public IKey GetPrimaryKey(IDatamodel dataModel)
        {
            return _keyService.GetPrimaryKey(dataModel);
        }

        public bool KeyExists(IKey key, string instrumentName, string serverParkName)
        {
            return _keyService.KeyExists(key, instrumentName, serverParkName);
        }

        public string GetPrimaryKeyValue(IDataRecord dataRecord)
        {
            return _keyService.GetPrimaryKeyValue(dataRecord);
        }

        public void AssignPrimaryKeyValue(IKey key, string primaryKeyValue)
        {
            _keyService.AssignPrimaryKeyValue(key, primaryKeyValue);
        }

        public IDataSet GetDataSet(string instrumentName, string serverParkName)
        {
            return _dataRecordService.GetDataSet(instrumentName, serverParkName);
        }

        public IDataRecord GetDataRecord(IDatamodel datamodel)
        {
            return _dataRecordService.GetDataRecord(datamodel);
        }

        public IDataRecord GetDataRecord(IKey key, string instrumentName, string serverParkName)
        {
            return _dataRecordService.GetDataRecord(key, instrumentName, serverParkName);
        }

        public IDataRecord GetDataRecord(IKey key, string filePath)
        {
            return _dataRecordService.GetDataRecord(key, filePath);
        }

        public void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            _dataRecordService.WriteDataRecord(dataRecord, instrumentName, serverParkName);
        }

        public void WriteDataRecord(IDataRecord dataRecord, string filePath)
        {
            _dataRecordService.WriteDataRecord(dataRecord, filePath);
        }

        public bool CompletedFieldExists(string instrumentName, string serverParkName)
        {
            return _fieldService.CompletedFieldExists(instrumentName, serverParkName);
        }

        public bool CaseHasBeenCompleted(IDataRecord dataRecord)
        {
            return _fieldService.CaseHasBeenCompleted(dataRecord);
        }

        public void MarkCaseAsComplete(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            _fieldService.MarkCaseAsComplete(dataRecord, instrumentName, serverParkName);
        }

        public bool ProcessedFieldExists(string instrumentName, string serverParkName)
        {
            return _fieldService.ProcessedFieldExists(instrumentName, serverParkName);
        }

        public bool CaseHasBeenProcessed(IDataRecord dataRecord)
        {
            return _fieldService.CaseHasBeenProcessed(dataRecord);
        }

        public void MarkCaseAsProcessed(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            _fieldService.MarkCaseAsProcessed(dataRecord, instrumentName, serverParkName);
        }

        public bool CaseExists(string primaryKeyValue, string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(instrumentName, serverParkName);

            return CaseExists(dataModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public bool CaseExists(string primaryKeyValue, string serverName, string instrumentName, string serverParkName)
        {
            var dataModel = _dataModelService.GetDataModel(serverName, instrumentName, serverParkName);

            return CaseExists(dataModel, primaryKeyValue, instrumentName, serverParkName);
        }

        public void CreateNewDataRecord(string primaryKeyValue, Dictionary<string, string> fieldData, string instrumentName, string serverParkName)
        {
            var dataModel = GetDataModel(instrumentName, serverParkName);
            var key = GetPrimaryKey(dataModel);
            var dataRecord = GetDataRecord(dataModel);

            dataRecord = _mapperService.MapDataRecordFields(dataRecord, dataModel, key, primaryKeyValue, fieldData);

            WriteDataRecord(dataRecord, instrumentName, serverParkName);
        }

        public void UpdateDataRecord(IDataRecord dataRecord, Dictionary<string, string> fieldData, string instrumentName, string serverParkName)
        {
            dataRecord = _mapperService.MapDataRecordFields(dataRecord, fieldData);

            WriteDataRecord(dataRecord, instrumentName, serverParkName);
        }

        private bool CaseExists(IDatamodel dataModel, string primaryKeyValue, string instrumentName, string serverParkName)
        {
            var primaryKey = _keyService.GetPrimaryKey(dataModel);

            _keyService.AssignPrimaryKeyValue(primaryKey, primaryKeyValue);

            return _keyService.KeyExists(primaryKey, instrumentName, serverParkName);
        }
    }
}
