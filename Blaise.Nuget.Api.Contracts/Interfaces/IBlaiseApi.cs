
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using StatNeth.Blaise.API.ServerManager;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseApi
    {
        ConnectionModel GetDefaultConnectionModel();

        IServerPark GetServerPark(ConnectionModel connectionModel, string serverParkName);

        IEnumerable<IServerPark> GetServerParks(ConnectionModel connectionModel);

        IEnumerable<string> GetServerParkNames(ConnectionModel connectionModel);

        IEnumerable<string> GetSurveyNames(ConnectionModel connectionModel, string serverParkName);

        bool SurveyExists(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IEnumerable<ISurvey> GetSurveys(ConnectionModel connectionModel, string serverParkName);

        IEnumerable<ISurvey> GetAllSurveys(ConnectionModel connectionModel);

        bool ServerParkExists(ConnectionModel connectionModel, string serverParkName);

        Guid GetInstrumentId(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        void CreateDayBatch(ConnectionModel connectionModel, string instrumentName, string serverParkName, DateTime dayBatchDate);

        SurveyType GetSurveyType(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        bool CaseExists(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);

        string GetPrimaryKeyValue(IDataRecord dataRecord);

        IDataSet GetDataSet(ConnectionModel connectionModel, string instrumentName, string serverParkName);

        IDataSet GetDataSet(string filePath);

        IDataRecord GetDataRecord(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName);

        void CreateNewDataRecord(ConnectionModel connectionModel, string primaryKeyValue, Dictionary<string, string> fieldData, string instrumentName,
            string serverParkName);

        void CreateNewDataRecord(string filePath, string primaryKeyValue, Dictionary<string, string> fieldData);

        void UpdateDataRecord(ConnectionModel connectionModel, IDataRecord dataRecord, Dictionary<string, string> fieldData, string instrumentName,
            string serverParkName);

        bool FieldExists(ConnectionModel connectionModel, string instrumentName, string serverParkName, FieldNameType fieldNameType);

        bool FieldExists(IDataRecord dataRecord, FieldNameType fieldNameType);

        IDataValue GetFieldValue(IDataRecord dataRecord, FieldNameType fieldNameType);

        IDataValue GetFieldValue(ConnectionModel connectionModel, string primaryKeyValue, string instrumentName, string serverParkName, FieldNameType fieldNameType);

        void AddUser(ConnectionModel connectionModel, string userName, string password, string role, IList<string> serverParkNames, string defaultServerPark);

        void EditUser(ConnectionModel connectionModel, string userName, string role, IList<string> serverParkNames);

        void ChangePassword(ConnectionModel connectionModel, string userName, string password);

        bool UserExists(ConnectionModel connectionModel, string userName);

        void RemoveUser(ConnectionModel connectionModel, string userName);

        IUser GetUser(ConnectionModel connectionModel, string userName);

        void RemoveCase(ConnectionModel sourceConnectionModel, string primaryKeyValue, string instrumentName, string serverParkName);
        string BackupSurveyToFile(ConnectionModel connectionModel, string serverParkName, string instrumentName, string destinationFilePath);
        void BackupFilesToBucket(string filePath, string bucketName, string folderName = null);

        int GetNumberOfCases(ConnectionModel connectionModel, string instrumentName, string serverParkName);
        int GetNumberOfCases(string filePath);
    }
}
