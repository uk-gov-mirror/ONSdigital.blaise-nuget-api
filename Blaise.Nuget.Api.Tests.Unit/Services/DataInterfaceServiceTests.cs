using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;
using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Contracts.Extensions;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using StatNeth.Blaise.API.DataInterface;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class DataInterfaceServiceTests
    {
        private Mock<IDataInterfaceFactory> _dataInterfaceFactoryMock;
        private Mock<IBlaiseConfigurationProvider> _configurationProviderMock;

        private Mock<IDataInterface> _dataInterfaceMock;

        private readonly ConnectionModel _connectionModel;
        private readonly Guid _instrumentId;
        private readonly string _connectionString;

        private DataInterfaceService _sut;

        public DataInterfaceServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentId = Guid.NewGuid();
            _connectionString = "testConnection";
        }

        [SetUp]
        public void SetUpTests()
        {
            //setup surveys
            _dataInterfaceMock = new Mock<IDataInterface>();
            _dataInterfaceMock.Setup(d => d.ConnectionInfo.SetConnectionString(It.IsAny<string>(), It.IsAny<string>()));
            _dataInterfaceMock.Setup(d => d.CreateTableDefinitions());
            _dataInterfaceMock.Setup(d => d.CreateDatabaseObjects(It.IsAny<string>(), It.IsAny<bool>()));
            _dataInterfaceMock.Setup(d => d.SaveToFile(It.IsAny<bool>()));
            
            _dataInterfaceFactoryMock = new Mock<IDataInterfaceFactory>();
            _dataInterfaceFactoryMock.Setup(d => d.GetConnection(_connectionModel, _instrumentId)).Returns(_dataInterfaceMock.Object);

            _configurationProviderMock = new Mock<IBlaiseConfigurationProvider>();
            _configurationProviderMock.Setup(c => c.DatabaseConnectionString).Returns(_connectionString);

            _sut = new DataInterfaceService(_dataInterfaceFactoryMock.Object, _configurationProviderMock.Object);
        }

        [Test]
        public void Given_I_Call_GetSurveyNames_Then_I_Get_A_Correct_List_Of_Survey_Names_Returned()
        {
            //act
            _sut.UpdateDataInterfaceConnection(_connectionModel, _instrumentId);

            //assert
            _dataInterfaceFactoryMock.Verify(v => v.GetConnection(_connectionModel, _instrumentId), Times.Once);
            _configurationProviderMock.Verify(v => v.DatabaseConnectionString, Times.Once);
            _dataInterfaceMock.Verify(v => v.ConnectionInfo.SetConnectionString(_connectionString, null), Times.Once);
            _dataInterfaceMock.Verify(v => v.CreateTableDefinitions(), Times.Once);
            _dataInterfaceMock.Verify(v => v.CreateDatabaseObjects(_connectionString, true), Times.Once);
            _dataInterfaceMock.Verify(v => v.SaveToFile(true), Times.Once);
        }

    }
}
