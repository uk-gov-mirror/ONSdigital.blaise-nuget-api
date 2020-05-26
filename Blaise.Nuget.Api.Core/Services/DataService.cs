﻿using Blaise.Nuget.Api.Core.Interfaces.Services;
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

        public DataService(
            IDataModelService dataModelService, 
            IDataRecordService dataRecordService, 
            IKeyService keyService, 
            IFieldService fieldService)
        {
            _dataModelService = dataModelService;
            _dataRecordService = dataRecordService;
            _keyService = keyService;
            _fieldService = fieldService;
        }

        public IDatamodel GetDataModel(string instrumentName, string serverParkName)
        {
            return _dataModelService.GetDataModel(instrumentName, serverParkName);
        }

        public IKey GetKey(IDatamodel datamodel, string keyName)
        {
            return _keyService.GetKey(datamodel, keyName);
        }

        public bool KeyExists(IKey key, string instrumentName, string serverParkName)
        {
            return _keyService.KeyExists(key, instrumentName, serverParkName);
        }

        public string GetPrimaryKey(IDataRecord dataRecord)
        {
            return _keyService.GetPrimaryKey(dataRecord);
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

        public bool CaseHasBeenCompleted(IDataRecord dataRecord)
        {
            return _fieldService.CaseHasBeenCompleted(dataRecord);
        }

        public void MarkCaseAsComplete(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            _fieldService.MarkCaseAsComplete(dataRecord, instrumentName, serverParkName);
        }

        public bool CaseHasBeenProcessed(IDataRecord dataRecord)
        {
            return _fieldService.CaseHasBeenProcessed(dataRecord);
        }

        public void MarkCaseAsProcessed(IDataRecord dataRecord, string instrumentName, string serverParkName)
        {
            _fieldService.MarkCaseAsProcessed(dataRecord, instrumentName, serverParkName);
        }
    }
}
