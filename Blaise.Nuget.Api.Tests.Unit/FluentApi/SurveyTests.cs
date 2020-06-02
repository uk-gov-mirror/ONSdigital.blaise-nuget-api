using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class SurveyTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private FluentBlaiseApi _sut;

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();

            _sut = new Api.FluentBlaiseApi(_blaiseApiMock.Object);
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
    }
}
