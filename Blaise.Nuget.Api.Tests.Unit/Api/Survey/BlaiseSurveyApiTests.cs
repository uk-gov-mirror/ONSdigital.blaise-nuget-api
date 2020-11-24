using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Unit.Api.Survey
{
    public class BlaiseSurveyApiTests
    {
        private Mock<ISurveyService> _surveyServiceMock;

        private readonly string _serverParkName;
        private readonly string _instrumentName;
        private readonly ConnectionModel _connectionModel;

        private IBlaiseSurveyApi _sut;

        public BlaiseSurveyApiTests()
        {
            _connectionModel = new ConnectionModel();
            _serverParkName = "Park1";
            _instrumentName = "Instrument1";
        }

        [SetUp]
        public void SetUpTests()
        {
            _surveyServiceMock = new Mock<ISurveyService>();

            _sut = new BlaiseSurveyApi(_surveyServiceMock.Object, _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseSurveyApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseSurveyApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseSurveyApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseSurveyApi(new ConnectionModel()));

        }
        [Test]
        public void Given_Valid_Arguments_When_I_Call_SurveyExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.SurveyExists(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            //act
            _sut.SurveyExists(_instrumentName, _serverParkName);

            //assert
            _surveyServiceMock.Verify(v => v.SurveyExists(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_SurveyExists_Then_The_Expected_Result_Is_Returned(bool exists)
        {
            //arrange
            _surveyServiceMock.Setup(p => p.SurveyExists(_connectionModel, _instrumentName,_serverParkName))
                .Returns(exists);

            //act            
            var result = _sut.SurveyExists(_instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exists, result);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_SurveyExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.SurveyExists(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_SurveyExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.SurveyExists( null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_SurveyExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.SurveyExists( _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_SurveyExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.SurveyExists(_instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void When_I_Call_GetSurveysAcrossServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.GetAllSurveys(_connectionModel)).Returns(It.IsAny<List<ISurvey>>());

            //act
            _sut.GetSurveysAcrossServerParks();

            //assert
            _surveyServiceMock.Verify(v => v.GetAllSurveys(_connectionModel), Times.Once);
        }

        [Test]
        public void When_I_Call_GetSurveysAcrossServerParks_Then_The_Expected_Surveys_Are_Returned()
        {
            //arrange
            var survey1Mock = new Mock<ISurvey>();
            var survey2Mock = new Mock<ISurvey>();
            var survey3Mock = new Mock<ISurvey>();

            var surveys = new List<ISurvey> { survey1Mock.Object, survey2Mock.Object, survey3Mock.Object };

            _surveyServiceMock.Setup(p => p.GetAllSurveys(_connectionModel)).Returns(surveys);

            //act
            var result = _sut.GetSurveysAcrossServerParks().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(3, result.Count);
            Assert.True(result.Contains(survey1Mock.Object));
            Assert.True(result.Contains(survey2Mock.Object));
            Assert.True(result.Contains(survey3Mock.Object));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveys_Then_The_Correct_Service_Method_Is_Called()
        {
            _surveyServiceMock.Setup(p => p.GetSurveys(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<List<ISurvey>>());

            //act
            _sut.GetSurveys(_serverParkName);

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
            var result = _sut.GetSurveys(_serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Contains(survey1Mock.Object));
            Assert.True(result.Contains(survey2Mock.Object));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetSurveys_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetSurveys(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetSurveys_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveys(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetNamesOfSurveys_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.GetSurveyNames(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<List<string>>());

            //act
            _sut.GetNamesOfSurveys(_serverParkName);

            //assert
            _surveyServiceMock.Verify(v => v.GetSurveyNames(_connectionModel, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetNamesOfSurveys_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var surveyList = new List<string>
            {
                "Instrument1",
                "Instrument2"
            };

            _surveyServiceMock.Setup(p => p.GetSurveyNames(_connectionModel, _serverParkName)).Returns(surveyList);

            //act            
            var result = _sut.GetNamesOfSurveys(_serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.True(result.Contains("Instrument1"));
            Assert.True(result.Contains("Instrument2"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetNamesOfSurveys_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNamesOfSurveys(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetNamesOfSurveys_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNamesOfSurveys(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetIdOfSurvey_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _surveyServiceMock.Setup(p => p.GetInstrumentId(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());

            //act
            _sut.GetIdOfSurvey(_instrumentName, _serverParkName);

            //assert
            _surveyServiceMock.Verify(v => v.GetInstrumentId(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetIdOfSurvey_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var instrumentId = Guid.NewGuid();

            _surveyServiceMock.Setup(p => p.GetInstrumentId(_connectionModel, _instrumentName, _serverParkName)).Returns(instrumentId);

            //act
            var result = _sut.GetIdOfSurvey(_instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(instrumentId, result);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetIdOfSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetIdOfSurvey(string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetIdOfSurvey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetIdOfSurvey(null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetIdOfSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetIdOfSurvey(_instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetIdOfSurvey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetIdOfSurvey(_instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
        
        [Test]
        public void Given_Valid_Arguments_When_I_Call_InstallSurvey_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var instrumentFile = @"d:\\opn2101a.pkg";

            //act
            _sut.InstallSurvey(_serverParkName, instrumentFile);

            //assert
            _surveyServiceMock.Verify(v => v.InstallInstrument(_connectionModel, _serverParkName, instrumentFile), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_InstallSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var instrumentFile = @"d:\\opn2101a.pkg";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallSurvey(string.Empty, instrumentFile));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_InstallSurvey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var instrumentFile = @"d:\\opn2101a.pkg";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallSurvey(null, instrumentFile));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_instrumentFile_When_I_Call_InstallSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.InstallSurvey(_serverParkName, string.Empty));
            Assert.AreEqual("A value for the argument 'instrumentFile' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_instrumentFile_When_I_Call_InstallSurvey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.InstallSurvey(_serverParkName, null));
            Assert.AreEqual("instrumentFile", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UninstallSurvey_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.UninstallSurvey(_serverParkName, _instrumentName);

            //assert
            _surveyServiceMock.Verify(v => v.UninstallInstrument(_connectionModel, _serverParkName, _instrumentName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_UninstallSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UninstallSurvey(string.Empty, _instrumentName));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_UninstallSurvey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UninstallSurvey(null, _instrumentName));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_instrumentName_When_I_Call_UninstallSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UninstallSurvey(_serverParkName, string.Empty));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_instrumentName_When_I_Call_UninstallSurvey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UninstallSurvey(_serverParkName, null));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }
    }
}
