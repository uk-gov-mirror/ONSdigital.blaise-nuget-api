
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.ServerManager;
using IDatamodel = StatNeth.Blaise.API.Meta.IDatamodel;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseApi
    {
        ConnectionModel GetDefaultConnectionModel();

        IEnumerable<string> GetServerParkNames(ConnectionModel connectionModel);

        IEnumerable<string> GetSurveyNames(ConnectionModel connectionModel, string serverParkName);

        bool SurveyExists(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IEnumerable<ISurvey> GetSurveys(ConnectionModel connectionModel, string serverParkName);

        IEnumerable<ISurvey> GetAllSurveys(ConnectionModel connectionModel);

        bool ServerParkExists(ConnectionModel connectionModel, string serverParkName);

        Guid GetInstrumentId(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IDatamodel GetDataModel(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        SurveyType GetSurveyType(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IKey GetKey(IDatamodel dataModel, string keyName);

        IKey GetPrimaryKey(IDatamodel dataModel);

        bool KeyExists(ConnectionModel connectionModel, IKey key, string instrumentName, string serverParkName);

        bool CaseExists(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);

        bool CaseExists(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);

        string GetPrimaryKeyValue(IDataRecord dataRecord);

        void AssignPrimaryKeyValue(IKey key, string primaryKeyValue);

        IDataSet GetDataSet(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IDataSet GetDataSet(string filePath);

        IDataRecord GetDataRecord(IDatamodel dataModel);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, IKey key, string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IKey key, string filePath);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);

        void WriteDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);

        void WriteDataRecord(IDataRecord dataRecord, string filePath);

        void CreateNewDataRecord(ConnectionModel connectionModel, string primaryKeyValue, Dictionary<string, string> fieldData, string instrumentName,
            string serverParkName);

        void CreateNewDataRecord(string filePath, string primaryKeyValue, Dictionary<string, string> fieldData);

        void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, Dictionary<string, string> fieldData, string instrumentName,
            string serverParkName);

        bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName, FieldNameType fieldNameType);

        bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType);

        bool CaseHasBeenCompleted(IDataRecord dataRecord);

        void MarkCaseAsComplete(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);

        bool CaseHasBeenProcessed(IDataRecord dataRecord);

        IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType);

        IDataValue GetFieldValue(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName, FieldNameType fieldNameType);

        void MarkCaseAsProcessed(ConnectionModel connectionModel, IDataRecord dataRecord, string instrumentName, string serverParkName);

        void AddUser(ConnectionModel connectionModel, string userName, string password, string role, IList<string> serverParkNames, string defaultServerPark);

        void EditUser(ConnectionModel connectionModel, string userName, string role, IList<string> serverParkNames);

        void ChangePassword(ConnectionModel connectionModel, string userName, string password);

        bool UserExists(ConnectionModel connectionModel, string userName);

        void RemoveUser(ConnectionModel connectionModel, string userName);

        void CopyCase(ConnectionModel sourceConnectionModel, string primaryKeyValue, string sourceInstrumentName,
            string sourceServerParkName, string destinationFilePath, string destinationInstrumentName);

        void CopyCase(ConnectionModel sourceConnectionModel, string primaryKeyValue, string sourceInstrumentName, string sourceServerParkName,
            ConnectionModel destinationConnectionModel, string destinationInstrumentName, string destinationServerParkName);

        void MoveCase(ConnectionModel sourceConnectionModel, string primaryKeyValue, string sourceInstrumentName, string sourceServerParkName,
            string destinationFilePath, string destinationInstrumentName);

        void MoveCase(ConnectionModel sourceConnectionModel, string primaryKeyValue, string sourceInstrumentName, string sourceServerParkName,
            ConnectionModel destinationConnectionModel, string destinationInstrumentName, string destinationServerParkName);

        void RemoveCase(ConnectionModel sourceConnectionModel, string primaryKeyValue, string instrumentName, string serverParkName);
        void BackupSurveyToFile(ConnectionModel connectionModel, string serverParkName, string instrumentName, string destinationFilePath);
        void BackupSurveyToBucket(ConnectionModel sourceConnectionModel, string serverParkName, string instrumentName, string bucketName, string folderName = null);
    }
}
