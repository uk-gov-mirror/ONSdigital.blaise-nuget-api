using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Unit.Api
{
    public class SurveyTests
    {
        private Mock<IDataService> _dataServiceMock;
        private Mock<IParkService> _parkServiceMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IIocProvider> _unityProviderMock;
        private Mock<IConfigurationProvider> _configurationProviderMock;

        private readonly string _serverParkName;
        private readonly string _instrumentName;
        private readonly ConnectionModel _connectionModel;

        private IBlaiseApi _sut;

        public SurveyTests()
        {
            _connectionModel = new ConnectionModel();
            _serverParkName = "Park1";
            _instrumentName = "Instrument1";
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataServiceMock = new Mock<IDataService>();
            _parkServiceMock = new Mock<IParkService>();
            _surveyServiceMock = new Mock<ISurveyService>();
            _userServiceMock = new Mock<IUserService>();
            _fileServiceMock = new Mock<IFileService>();
            _unityProviderMock = new Mock<IIocProvider>();
            _configurationProviderMock = new Mock<IConfigurationProvider>();

            _sut = new BlaiseApi(
                _dataServiceMock.Object,
                _parkServiceMock.Object,
                _surveyServiceMock.Object,
                _userServiceMock.Object,
                _fileServiceMock.Object,
                _unityProviderMock.Object,
                _configurationProviderMock.Object);
        }

        [Test]
        public void When_I_Call_GetAllSurveys_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.GetAllSurveys(_connectionModel)).Returns(It.IsAny<List<ISurvey>>());

            //act
            _sut.GetAllSurveys(_connectionModel);

            //assert
            _surveyServiceMock.Verify(v => v.GetAllSurveys(_connectionModel), Times.Once);
        }

        [Test]
        public void When_I_Call_GetAllSurveys_Then_The_Expected_Surveys_Are_Returned()
        {
            //arrange
            var survey1Mock = new Mock<ISurvey>();
            var survey2Mock = new Mock<ISurvey>();
            var survey3Mock = new Mock<ISurvey>();

            var surveys = new List<ISurvey> { survey1Mock.Object, survey2Mock.Object, survey3Mock.Object };

            _surveyServiceMock.Setup(p => p.GetAllSurveys(_connectionModel)).Returns(surveys);

            //act
            var result = _sut.GetAllSurveys(_connectionModel).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(3, result.Count);
            Assert.True(result.Contains(survey1Mock.Object));
            Assert.True(result.Contains(survey2Mock.Object));
            Assert.True(result.Contains(survey3Mock.Object));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveyNames_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.GetSurveyNames(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<List<string>>());

            //act
            _sut.GetSurveyNames(_connectionModel, _serverParkName);

            //assert
            _surveyServiceMock.Verify(v => v.GetSurveyNames(_connectionModel, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveyNames_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var surveyList = new List<string>
            {
                "Instrument1",
                "Instrument2"
            };

            _surveyServiceMock.Setup(p => p.GetSurveyNames(_connectionModel, _serverParkName)).Returns(surveyList);

            //act            
            var result = _sut.GetSurveyNames(_connectionModel, _serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Contains("Instrument1"));
            Assert.True(result.Contains("Instrument2"));
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetSurveyNames_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveyNames(null, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetSurveyNames_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetSurveyNames(_connectionModel, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetSurveyNames_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveyNames(_connectionModel, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveys_Then_The_Correct_Service_Method_Is_Called()
        {
            _surveyServiceMock.Setup(p => p.GetSurveys(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<List<ISurvey>>());

            //act
            _sut.GetSurveys(_connectionModel, _serverParkName);

            //assert
            _surveyServiceMock.Verify(v => v.GetSurveys(_connectionModel, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveys_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var survey1Mock = new Mock<ISurvey>();
            var survey2Mock = new Mock<ISurvey>();
            var surveyList = new List<ISurvey>
            {
                survey1Mock.Object,
                survey2Mock.Object
            };

            _surveyServiceMock.Setup(p => p.GetSurveys(_connectionModel, _serverParkName)).Returns(surveyList);

            //act            
            var result = _sut.GetSurveys(_connectionModel, _serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Contains(survey1Mock.Object));
            Assert.True(result.Contains(survey2Mock.Object));
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetSurveys_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveys(null, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetSurveys_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetSurveys(_connectionModel, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetSurveys_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveys(_connectionModel, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetInstrumentId_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.GetInstrumentId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());

            //act
            _sut.GetInstrumentId(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _surveyServiceMock.Verify(v => v.GetInstrumentId(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetInstrumentId_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var instrumentId = Guid.NewGuid();

            _surveyServiceMock.Setup(p => p.GetInstrumentId(_connectionModel, _instrumentName, _serverParkName)).Returns(instrumentId);

            //act
            var result = _sut.GetInstrumentId(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(instrumentId, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetInstrumentId_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstrumentId(null, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetInstrumentId_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstrumentId(_connectionModel, string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetInstrumentId_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstrumentId(_connectionModel, null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetInstrumentId_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstrumentId(_connectionModel, _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetInstrumentId_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstrumentId(_connectionModel, _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Instrument_AndServerPark_When_I_Call_GetSurveyType_Then_The_Correct_Service_Method_Is_Called()
        {
            _dataServiceMock.Setup(d => d.GetSurveyType(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<SurveyType>());

            //act
            _sut.GetSurveyType(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.GetSurveyType(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveyType_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var surveyType = SurveyType.NotMapped;

            _dataServiceMock.Setup(d => d.GetSurveyType(_connectionModel, _instrumentName, _serverParkName)).Returns(surveyType);

            //act
            var result = _sut.GetSurveyType(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(surveyType, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetSurveyType_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveyType(null, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetSurveyType_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetSurveyType(_connectionModel, string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetSurveyType_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveyType(_connectionModel, null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetSurveyType_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetSurveyType(_connectionModel, _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetSurveyType_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveyType(_connectionModel, _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
    }
}
