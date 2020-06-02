using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class SurveyTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly string _instrumentName;
        private readonly string _serverParkName;

        private FluentBlaiseApi _sut;

        public SurveyTests()
        {
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
            _blaiseApiMock.Setup(p => p.GetAllSurveys()).Returns(It.IsAny<List<ISurvey>>());

            //act
            _sut.Surveys();

            //assert
            _blaiseApiMock.Verify(v => v.GetAllSurveys(), Times.Once);
        }

        [Test]
        public void When_I_Call_Surveys_Then_The_Expected_Surveys_Are_Returned()
        {
            //arrange
            var survey1Mock = new Mock<ISurvey>();
            var survey2Mock = new Mock<ISurvey>();
            var survey3Mock = new Mock<ISurvey>();

            var surveys = new List<ISurvey> { survey1Mock.Object, survey2Mock.Object, survey3Mock.Object };

            _blaiseApiMock.Setup(p => p.GetAllSurveys()).Returns(surveys);

            //act
            var result = _sut.Surveys().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(3, result.Count);
            Assert.True(result.Contains(survey1Mock.Object));
            Assert.True(result.Contains(survey2Mock.Object));
            Assert.True(result.Contains(survey3Mock.Object));
        }

        [Test]
        public void Given_Valid_Instrument_And_ServerPark_When_I_Call_SurveyType_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.GetSurveyType(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<SurveyType>());

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);

            //act
            _sut.SurveyType();

            //assert
            _blaiseApiMock.Verify(v => v.GetSurveyType(_instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(SurveyType.Appointment)]
        [TestCase(SurveyType.CatiDial)]
        [TestCase(SurveyType.NotMapped)]
        public void Given_Valid_Arguments_When_I_Call_SurveyType_Then_The_Expected_Result_Is_Returned(SurveyType surveyType)
        {
            //arrange

            _blaiseApiMock.Setup(d => d.GetSurveyType(_instrumentName, _serverParkName)).Returns(surveyType);

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);

            //act
            var result = _sut.SurveyType();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(surveyType, result);
        }

        [Test]
        public void Given_ServerPark_Has_Not_Been_Called_When_I_Call_SurveyType_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.Instrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.SurveyType());
            Assert.AreEqual("The 'ServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_Instrument_Has_Not_Been_Called_When_I_Call_GetSurveyType_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.ServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.SurveyType());
            Assert.AreEqual("The 'Instrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }
    }
}
