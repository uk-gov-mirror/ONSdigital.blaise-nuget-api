using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class SurveyTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;

        private FluentBlaiseApi _sut;

        public SurveyTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "Instrument1";
            _serverParkName = "Park1";
        }

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();

            _sut = new FluentBlaiseApi(_blaiseApiMock.Object);
        }

        [Test]
        public void When_I_Call_Surveys_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetAllSurveys(_connectionModel)).Returns(It.IsAny<List<ISurvey>>());

            _sut.WithConnection(_connectionModel);

            //act
            var sutSurveys = _sut.Surveys;

            //assert
            _blaiseApiMock.Verify(v => v.GetAllSurveys(_connectionModel), Times.Once);
        }

        [Test]
        public void When_I_Call_Surveys_Then_The_Expected_Surveys_Are_Returned()
        {
            //arrange
            var survey1Mock = new Mock<ISurvey>();
            var survey2Mock = new Mock<ISurvey>();
            var survey3Mock = new Mock<ISurvey>();

            var surveys = new List<ISurvey> { survey1Mock.Object, survey2Mock.Object, survey3Mock.Object };

            _blaiseApiMock.Setup(p => p.GetAllSurveys(_connectionModel)).Returns(surveys);

            _sut.WithConnection(_connectionModel);

            //act
            var result = _sut.Surveys.ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(3, result.Count);
            Assert.True(result.Contains(survey1Mock.Object));
            Assert.True(result.Contains(survey2Mock.Object));
            Assert.True(result.Contains(survey3Mock.Object));
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_Surveys_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetAllSurveys(_connectionModel)).Returns(It.IsAny<List<ISurvey>>());

            // _sut.WithConnection(_connectionModel);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutSurveys = _sut.Surveys;
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_I_Call_Called_WithServerPark_When_I_Call_Surveys_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetSurveys(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<List<ISurvey>>());

            //act
            var sutSurveys = _sut
                .WithConnection(_connectionModel)
                .WithServerPark(_serverParkName)
                .Surveys;

            //assert
            _blaiseApiMock.Verify(v => v.GetSurveys(_connectionModel, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_GetSurveys_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetSurveys(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<List<ISurvey>>());

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutSurveys = _sut
                    .WithServerPark(_serverParkName)
                    .Surveys;
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_I_Call_Called_WithServerPark_When_I_Call_Surveys_Then_The_Expected_Surveys_Are_Returned()
        {
            //arrange
            var survey1Mock = new Mock<ISurvey>();
            var survey2Mock = new Mock<ISurvey>();
            var survey3Mock = new Mock<ISurvey>();

            var surveys = new List<ISurvey> { survey1Mock.Object, survey2Mock.Object, survey3Mock.Object };

            _blaiseApiMock.Setup(p => p.GetSurveys(_connectionModel, It.IsAny<string>())).Returns(surveys);

            //act
            var result = _sut
                .WithConnection(_connectionModel)
                .WithServerPark(_serverParkName)
                .Surveys
                .ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(3, result.Count);
            Assert.True(result.Contains(survey1Mock.Object));
            Assert.True(result.Contains(survey2Mock.Object));
            Assert.True(result.Contains(survey3Mock.Object));
        }

        [Test]
        public void Given_Valid_Instrument_And_ServerPark_When_I_Call_Type_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.GetSurveyType(_connectionModel, It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<SurveyType>());
           
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act
            var surveyType = _sut.Type;

            //assert
            _blaiseApiMock.Verify(v => v.GetSurveyType(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(SurveyType.Appointment)]
        [TestCase(SurveyType.CatiDial)]
        [TestCase(SurveyType.NotMapped)]
        public void Given_Valid_Arguments_When_I_Call_Type_Then_The_Expected_Result_Is_Returned(SurveyType surveyType)
        {
            //arrange
            _blaiseApiMock.Setup(d => d.GetSurveyType(_connectionModel, _instrumentName, _serverParkName)).Returns(surveyType);
        
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act
            var result = _sut.Type;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(surveyType, result);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_Type_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var surveyType = _sut.Type;
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_Type_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var surveyType = _sut.Type;
            });
            Assert.AreEqual("The 'WithServerPark' step needs to be called with a valid value, check that the step has been called with a valid value for the server park", exception.Message);
        }

        [Test]
        public void Given_WithInstrument_Has_Not_Been_Called_When_I_Call_Type_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var surveyType = _sut.Type;
            });
            Assert.AreEqual("The 'WithInstrument' step needs to be called with a valid value, check that the step has been called with a valid instrument", exception.Message);
        }
        [TestCase(FieldNameType.HOut)]
        public void Given_Valid_Arguments_When_I_Call_HasField_Then_The_Correct_Service_Method_Is_Called(FieldNameType fieldNameType)
        {
            //arrange

            _blaiseApiMock.Setup(p => p.FieldExists(_connectionModel, It.IsAny<string>(), 
                It.IsAny<string>(), It.IsAny<FieldNameType>())).Returns(It.IsAny<bool>());

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act
            var sutExists = _sut.Survey.HasField(fieldNameType);

            //assert
            _blaiseApiMock.Verify(v => v.FieldExists(_connectionModel, _instrumentName, _serverParkName, fieldNameType), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_HasField_Then_The_Expected_Result_Is_Returned(bool fieldExists)
        {
            //arrange

            _blaiseApiMock.Setup(p => p.FieldExists(_connectionModel, _instrumentName, _serverParkName
                ,FieldNameType.HOut)).Returns(fieldExists);

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            
            //act
            var result = _sut.Survey.HasField(FieldNameType.HOut);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_Connection_Has_Not_Been_Called_When_I_Call_HasField_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            
            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Survey.HasField(FieldNameType.HOut);
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_Instrument_Has_Not_Been_Called_When_I_Call_HasField_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Survey.HasField(FieldNameType.HOut);
            });
            Assert.AreEqual("The 'WithInstrument' step needs to be called with a valid value, check that the step has been called with a valid instrument", exception.Message);
        }

        [Test]
        public void Given_ServerPark_Has_Not_Been_Called_When_I_Call_HasField_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Survey.HasField(FieldNameType.HOut);
            });
            Assert.AreEqual("The 'WithServerPark' step needs to be called with a valid value, check that the step has been called with a valid value for the server park", exception.Message);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_Exists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(p => p.SurveyExists(_connectionModel, It.IsAny<string>(),
                It.IsAny<string>())).Returns(It.IsAny<bool>());

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act
            var sutExists = _sut.Survey.Exists;

            //assert
            _blaiseApiMock.Verify(v => v.SurveyExists(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_Exists_Then_The_Expected_Result_Is_Returned(bool exists)
        {
            //arrange

            _blaiseApiMock.Setup(p => p.SurveyExists(_connectionModel, _instrumentName, _serverParkName)).Returns(exists);

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act
            var result = _sut.Survey.Exists;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exists, result);
        }

        [Test]
        public void Given_Connection_Has_Not_Been_Called_When_I_Call_Exists_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Survey.Exists;
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_Instrument_Has_Not_Been_Called_When_I_Call_Exists_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Survey.Exists;
            });
            Assert.AreEqual("The 'WithInstrument' step needs to be called with a valid value, check that the step has been called with a valid instrument", exception.Message);
        }

        [Test]
        public void Given_ServerPark_Has_Not_Been_Called_When_I_Call_Exists_Then_A_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Survey.Exists;
            });
            Assert.AreEqual("The 'WithServerPark' step needs to be called with a valid value, check that the step has been called with a valid value for the server park", exception.Message);
        }
    }
}
