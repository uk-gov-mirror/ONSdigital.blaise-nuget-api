﻿
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using System;
using System.Collections.Generic;
using IDatamodel = StatNeth.Blaise.API.Meta.IDatamodel;

namespace Blaise.Nuget.Api.Contracts.Interfaces
{
    public interface IBlaiseApi
    {
        IEnumerable<string> GetServerParkNames();

        IEnumerable<string> GetSurveyNames(string serverParkName);

        bool ServerParkExists(string serverParkName);

        Guid GetInstrumentId(string instrumentName, string serverParkName);

        IDatamodel GetDataModel(string instrumentName, string serverParkName);

        IKey GetKey(IDatamodel dataModel, string keyName);

        bool KeyExists(IKey key, string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IDatamodel dataModel);

        IDataSet GetDataSet(string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IKey key, string instrumentName, string serverParkName);

        IDataRecord GetDataRecord(IKey key, string filePath);

        void WriteDataRecord(IDataRecord dataRecord, string instrumentName, string serverParkName);

        void WriteDataRecord(IDataRecord dataRecord, string filePath);
    }
}
