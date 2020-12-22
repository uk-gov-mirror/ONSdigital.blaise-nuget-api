using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Api;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Cati
{
    public class CatiTests
    {
        private readonly BlaiseCatiApi _sut;

        private readonly string serverParkName = "LocalDevelopment";
        private readonly string surveyName = "OPN2101A";

        public CatiTests()
        {
            _sut = new BlaiseCatiApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_And_Has_SurveyDays_When_I_Call_GetSurveyDays_They_Are_Returned()
        {
            var result = _sut.GetSurveyDays(surveyName, serverParkName);
            Assert.NotNull(result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_And_A_SurveyDay_is_Added_The_Survey_Day_Is_Returned()
        {
            //Act
            _sut.SetSurveyDay(surveyName, serverParkName, DateTime.Today);

            //Assert
            var result = _sut.GetSurveyDays(surveyName, serverParkName);
            Assert.IsTrue(result.Contains(DateTime.Today));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_And_Multiple_SurveyDays_Are_Added_The_Survey_Days_Are_Returned()
        {
            //Arrange
            var daysToAdd = new List<DateTime>
            {
                DateTime.Today,
                DateTime.Today.AddDays(1)
            };

            //Act
            _sut.SetSurveyDays(surveyName, serverParkName, daysToAdd);

            //Assert
            var result = _sut.GetSurveyDays(surveyName, serverParkName);
            Assert.IsTrue(result.Contains(DateTime.Today));
            Assert.IsTrue(result.Contains(DateTime.Today.AddDays(1)));
        }
    }
}
