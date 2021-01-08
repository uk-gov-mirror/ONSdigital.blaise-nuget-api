using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Core.Factories
{
    public class DataInterfaceFactory : IDataInterfaceFactory
    {
        private readonly IPasswordService _passwordService;

        private readonly Dictionary<Guid, Tuple<IDataInterface, DateTime>> _dataInterfaces;

        public DataInterfaceFactory(IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _dataInterfaces = new Dictionary<Guid, Tuple<IDataInterface, DateTime>>();
        }

        public IDataInterface GetDataInterface(ConnectionModel connectionModel, Guid instrumentGuid)
        {
            if (!_dataInterfaces.ContainsKey(instrumentGuid))
            {
                return GetFreshServerConnection(connectionModel, instrumentGuid);
            }

            var (dataInterface, expiryDate) = _dataInterfaces[instrumentGuid];

            return expiryDate.HasExpired()
                ? GetFreshServerConnection(connectionModel, instrumentGuid)
                : dataInterface ?? GetFreshServerConnection(connectionModel, instrumentGuid);
        }

        public IDataInterface GetDataInterfaceForSql(string databaseConnectionString)
        {
            var dataInterface = DataInterfaceManager.GetDataInterface();
            dataInterface.ConnectionInfo.DataSourceType = DataSourceType.MySQL;
            dataInterface.ConnectionInfo.DataProviderType = DataProviderType.MySQLDataProvider;
            dataInterface.DataPartitionType = DataPartitionType.Stream;

            var connectionStringBuilder = DataInterfaceManager.GetBlaiseConnectionStringBuilder();
            connectionStringBuilder.DataSource = databaseConnectionString;
            dataInterface.ConnectionInfo.SetConnectionString(connectionStringBuilder.ConnectionString);

            return dataInterface;
        }

        private IDataInterface GetFreshServerConnection(ConnectionModel connectionModel, Guid instrumentGuid)
        {
            var remoteConnection = CreateDataInterfaceConnection(connectionModel, instrumentGuid);

            _dataInterfaces[instrumentGuid] =
                new Tuple<IDataInterface, DateTime>(remoteConnection, connectionModel.ConnectionExpiresInMinutes.GetExpiryDate());

            return remoteConnection;
        }

        private IDataInterface CreateDataInterfaceConnection(ConnectionModel connectionModel, Guid instrumentGuid)
        {
            var securePassword = _passwordService.CreateSecurePassword(connectionModel.Password);

            return DataInterfaceManager.GetDataInterface(
                connectionModel.Binding,
                connectionModel.ServerName,
                connectionModel.Port,
                connectionModel.UserName,
                securePassword,
                instrumentGuid);
        }
    }
}
