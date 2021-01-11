using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using System;
using Blaise.Nuget.Api.Contracts.Models;
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
            _dataInterfaceFactoryMock.Setup(d => d.GetDataInterfaceForSql(It.IsAny<string>())).Returns(_dataInterfaceMock.Object);

            _configurationProviderMock = new Mock<IBlaiseConfigurationProvider>();
            _configurationProviderMock.Setup(c => c.DatabaseConnectionString).Returns(_connectionString);

            _sut = new DataInterfaceService(_dataInterfaceFactoryMock.Object, _configurationProviderMock.Object);
        }

        [Test]
        public void Given_I_Call_GetSurveyNames_Then_I_Get_A_Correct_List_Of_Survey_Names_Returned()
        {
            //arrange
            const string fileName = "OPN.bdix";
            const string dataModelFileName = "OPN.bmix";
            _dataInterfaceMock.Setup(d => d.ConnectionInfo.GetConnectionString(null)).Returns(_connectionString);

            //act
            _sut.CreateDataInterface(fileName, dataModelFileName);

            //assert
            _configurationProviderMock.Verify(v => v.DatabaseConnectionString, Times.Once);
            _dataInterfaceFactoryMock.Verify(v => v.GetDataInterfaceForSql(_connectionString), Times.Once);
            _dataInterfaceMock.Verify(v => v.CreateTableDefinitions(), Times.Once);
            _dataInterfaceMock.Verify(v => v.CreateDatabaseObjects(_connectionString, true), Times.Once);
            _dataInterfaceMock.Verify(v => v.SaveToFile(true), Times.Once);
        }

    }
}
