using System;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class DataModelServiceTests
    {
        private Mock<IRemoteDataLinkProvider> _remoteDataLinkProviderMock;
        private Mock<ILocalDataLinkProvider> _localDataLinkProviderMock;

        private Mock<IDataLink4> _dataLinkMock;
        private Mock<IDatamodel> _dataModelMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _filePath;

        private DataModelService _sut;

        public DataModelServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _filePath = "TestFilePath";
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();

            _dataLinkMock = new Mock<IDataLink4>();
            _dataLinkMock.Setup(d => d.Datamodel).Returns(_dataModelMock.Object);

            _remoteDataLinkProviderMock = new Mock<IRemoteDataLinkProvider>();
            _remoteDataLinkProviderMock.Setup(r => r.GetDataLink(_connectionModel, _instrumentName, _serverParkName)).Returns(_dataLinkMock.Object);

            _localDataLinkProviderMock = new Mock<ILocalDataLinkProvider>();
            _localDataLinkProviderMock.Setup(r => r.GetDataLink(_filePath)).Returns(_dataLinkMock.Object);

            _sut = new DataModelService(_remoteDataLinkProviderMock.Object, _localDataLinkProviderMock.Object);
        }


        [Test]
        public void Given_I_Call_GetDataModel_I_Get_A_DataModel_Back()
        {
            //act
            var result = _sut.GetDataModel(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDatamodel>(result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_I_Get_The_Correct_DataModel_Back()
        {
            //act
            var result = _sut.GetDataModel(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(_dataModelMock.Object, result);
        }

        [Test]
        public void Given_No_DataModel_Available_When_I_Call_GetDataModel_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _dataLinkMock.Setup(d => d.Datamodel).Returns(null as IDatamodel);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetDataModel(_connectionModel, _instrumentName, _serverParkName));
            Assert.AreEqual($"No datamodel was found for instrument '{_instrumentName}' on server park '{_serverParkName}'", exception.Message);
        }

        [Test]
        public void Given_I_Call_GetDataModel_Then_The_Correct_Services_Are_Called()
        {
            //act
             _sut.GetDataModel(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _instrumentName, _serverParkName), Times.Once);
            _dataLinkMock.Verify(v => v.Datamodel, Times.AtLeastOnce);
        }

        [Test]
        public void Given_I_Call_GetDataModel_For_Local_Connection_I_Get_A_DataModel_Back()
        {
            //act
            var result = _sut.GetDataModel(_filePath);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDatamodel>(result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_For_Local_Connection_I_Get_The_Correct_DataModel_Back()
        {
            //act
            var result = _sut.GetDataModel(_filePath);

            //assert
            Assert.AreEqual(_dataModelMock.Object, result);
        }

        [Test]
        public void Given_No_DataModel_Available_When_I_Call_GetDataModel_For_Local_Connection_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _dataLinkMock.Setup(d => d.Datamodel).Returns(null as IDatamodel);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetDataModel(_filePath));
            Assert.AreEqual($"No datamodel was found for file '{_filePath}'", exception.Message);
        }

        [Test]
        public void Given_I_Call_GetDataModel_For_Local_Connection_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataModel(_filePath);

            //assert
            _localDataLinkProviderMock.Verify(v => v.GetDataLink(_filePath), Times.Once);
            _dataLinkMock.Verify(v => v.Datamodel, Times.AtLeastOnce);
        }

        [TestCase("Appointment", SurveyType.Appointment)]
        [TestCase("CatiDial", SurveyType.CatiDial)]
        [TestCase("",SurveyType.NotMapped)]
        [TestCase("Other", SurveyType.NotMapped)]
        public void Given_I_Call_GetSurveyType_I_Get_The_Correct_Type_Back(string caseName, SurveyType surveyType)
        {
            //arrange
            _dataModelMock.Setup(d => d.Name).Returns(caseName);

            //act
            var result = _sut.GetSurveyType(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(surveyType, result);
        }

        [Test]
        public void Given_I_Call_GetSurveyType_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetSurveyType(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _instrumentName, _serverParkName), Times.Once);
            _dataLinkMock.Verify(v => v.Datamodel, Times.AtLeastOnce);
        }
    }
}
