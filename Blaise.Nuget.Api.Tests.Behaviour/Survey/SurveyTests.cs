using System.Linq;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Behaviour.Survey
{
    public class SurveyTests
    {
        private readonly BlaiseSurveyApi _sut;

        private readonly string serverParkName = "LocalDevelopment";
        private readonly string fullInstrumentPath = @"C:\Users\User\Desktop\OPN2101A.zip";
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
            _sut.InstallSurvey(fullInstrumentPath, SurveyInterviewType.Cati, serverParkName);

            //assert
            var surveys = _sut.GetSurveys(serverParkName).ToList();
            Assert.IsNotNull(surveys.First(s => s.Name == surveyName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_It_Gets_Uninstalled_From_The_Server_Park()
        {
            //arrange
            _sut.InstallSurvey(fullInstrumentPath, SurveyInterviewType.Cati, serverParkName);
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
        public void Given_An_Instrument_Is_Installed_When_I_Call_GetSurvey_The_Correct_Survey_Is_Returned()
        {
            //act
            var result =_sut.GetSurvey(surveyName, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ISurvey>(result);

            Assert.AreEqual(surveyName, result.Name);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_When_I_Call_GetSurveyInterviewType_The_Correct_Type_Is_Returned()
        {
            //act
            var result = _sut.GetSurveyInterviewType(surveyName, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<SurveyInterviewType>(result);

            Assert.AreEqual(SurveyInterviewType.Cati, result);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_When_I_Call_Deactivate_Then_The_Survey_Is_Deactivated()
        {
            //act
            _sut.DeactivateSurvey(surveyName, serverParkName);

            //assert
            Assert.AreEqual(SurveyStatusType.Inactive, _sut.GetSurveyStatus(surveyName, serverParkName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_When_I_Call_Activate_Then_The_Survey_Is_Activated()
        {
            //act
            _sut.ActivateSurvey(surveyName, serverParkName);

            //assert
            Assert.AreEqual(SurveyStatusType.Active, _sut.GetSurveyStatus(surveyName, serverParkName));
        }
    }
}
