using System.Linq;
using Blaise.Nuget.Api.Api;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Behaviour.Survey
{
    public class SurveyTests
    {
        private readonly BlaiseSurveyApi _sut;

        private readonly string serverParkName = "LocalDevelopment";
        private readonly string fullInstrumentPath = @"C:\Users\User\Downloads\OPN2101A.zip";
        private readonly string surveyName = "OPN2101A";
        public SurveyTests()
        {
            _sut = new BlaiseSurveyApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_I_Have_A_Valid_Instrument_It_Gets_Installed_On_A_Server_Park()
        {
            //arrange
            

            //act
            _sut.InstallSurvey(serverParkName, fullInstrumentPath);

            //assert
            var surveys = _sut.GetSurveys(serverParkName).ToList();
            Assert.IsNotNull(surveys.First(s => s.Name == surveyName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_It_Gets_Uninstalled_From_The_Server_Park()
        {
            //arrange
            _sut.InstallSurvey(serverParkName, fullInstrumentPath);
            var surveys = _sut.GetSurveys(serverParkName).ToList();
            Assert.IsNotNull(surveys.First(s => s.Name == surveyName));

            //act_
            _sut.UninstallSurvey(serverParkName, surveyName);

            //assert
            var surveysInstalled = _sut.GetSurveys(serverParkName).ToList();
            Assert.IsNull(surveysInstalled.FirstOrDefault(s => s.Name == surveyName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_When_I_Call_GetSurveyStatus_The_Correct_Status_Is_Returned()
        {
            //act
            var result =_sut.GetSurvey(surveyName, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ISurvey>(result);

            Assert.AreEqual(surveyName, result.Name);
        }
    }
}
