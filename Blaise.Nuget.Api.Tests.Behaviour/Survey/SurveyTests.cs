using System.Linq;
using NUnit.Framework;

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

        [Test]
        public void Given_I_Have_A_Valid_Instrument_It_Gets_Installed_On_A_Server_Park()
        {
            //arrange
            

            //act
            _sut.InstallSurveyOnServerPark(serverParkName, fullInstrumentPath);

            //assert
            var surveys = _sut.GetSurveysInstalledOnServerPark(serverParkName).ToList();
            Assert.IsNotNull(surveys.First(s => s.Name == surveyName));
        }

        [Test]
        public void Given_An_Instrument_Is_Installed_It_Gets_Unistalled_From_The_Server_Park()
        {
            //arrange
            _sut.InstallSurveyOnServerPark(serverParkName, fullInstrumentPath);
            var surveys = _sut.GetSurveysInstalledOnServerPark(serverParkName).ToList();
            Assert.IsNotNull(surveys.First(s => s.Name == surveyName));

            //act_sut.GetDefaultConnectionModel(), 
            _sut.UninstallSurveyFromServerPark(serverParkName, surveyName);

            //assert
            var surveysInstalled = _sut.GetSurveysInstalledOnServerPark(serverParkName).ToList();
            Assert.IsNull(surveysInstalled.FirstOrDefault(s => s.Name == surveyName));
        }
    }
}
