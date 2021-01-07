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

        private const string ServerParkName = "LocalDevelopment";
        private const string FullInstrumentPath = @"C:\Users\User\Desktop\OPN2101A.zip";
        private const string InstrumentName = "OPN2101A";

        public SurveyTests()
        {
            _sut = new BlaiseSurveyApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_I_Have_A_Valid_Instrument_It_Gets_Installed_On_A_Server_Park()
        {
            //act
            _sut.InstallSurvey(InstrumentName, ServerParkName, FullInstrumentPath, SurveyInterviewType.Cati);

            //assert
            var surveys = _sut.GetSurveys(ServerParkName).ToList();
            Assert.IsNotNull(surveys.First(s => s.Name == InstrumentName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_It_Gets_Uninstalled_From_The_Server_Park()
        {
            //arrange
            _sut.InstallSurvey(InstrumentName, ServerParkName,FullInstrumentPath, SurveyInterviewType.Cati);
            var surveys = _sut.GetSurveys(ServerParkName).ToList();
            Assert.IsNotNull(surveys.First(s => s.Name == InstrumentName));

            //act_
            _sut.UninstallSurvey(ServerParkName, InstrumentName);

            //assert
            var surveysInstalled = _sut.GetSurveys(ServerParkName).ToList();
            Assert.IsNull(surveysInstalled.FirstOrDefault(s => s.Name == InstrumentName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_When_I_Call_GetSurvey_The_Correct_Survey_Is_Returned()
        {
            //act
            var result =_sut.GetSurvey(InstrumentName, ServerParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<ISurvey>(result);

            Assert.AreEqual(InstrumentName, result.Name);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_When_I_Call_GetSurveyInterviewType_The_Correct_Type_Is_Returned()
        {
            //act
            var result = _sut.GetSurveyInterviewType(InstrumentName, ServerParkName);

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
            _sut.DeactivateSurvey(InstrumentName, ServerParkName);

            //assert
            Assert.AreEqual(SurveyStatusType.Inactive, _sut.GetSurveyStatus(InstrumentName, ServerParkName));
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Instrument_Is_Installed_When_I_Call_Activate_Then_The_Survey_Is_Activated()
        {
            //act
            _sut.ActivateSurvey(InstrumentName, ServerParkName);

            //assert
            Assert.AreEqual(SurveyStatusType.Active, _sut.GetSurveyStatus(InstrumentName, ServerParkName));
        }
    }
}
