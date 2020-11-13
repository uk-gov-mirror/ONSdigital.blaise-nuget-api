using Blaise.Nuget.Api.Core.Interfaces;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaise.Nuget.Api.Tests.Behaviour.Survey
{
    public class SurveyTests
    {
        private readonly BlaiseApi _sut;

        private readonly string serverParkName = "LocalDevelopment";
        private readonly string fullInstrumentPath = @"C:\Users\User\Downloads\OPN2101A.zip";
        private readonly string surveyName = "OPN2101A";
        public SurveyTests()
        {
            _sut = new BlaiseApi();
        }

        [Test]
        public void Given_I_Have_A_Valid_Instrument_It_Gets_Installed_On_A_Server_Park()
        {
            //arrange
            

            //act
            _sut.InstallSurvey(_sut.GetDefaultConnectionModel(), serverParkName, fullInstrumentPath);

            //assert
            var surveys = _sut.GetAllSurveys(_sut.GetDefaultConnectionModel()).ToList();
            Assert.IsNotNull(surveys.Where(s => s.Name == surveyName).First());
        }

        [Test]
        public void Given_An_Instrument_Is_Installed_It_Gets_Unistalled_From_The_Server_Park()
        {
            //arrange
            _sut.InstallSurvey(_sut.GetDefaultConnectionModel(), serverParkName, fullInstrumentPath);
            var surveys = _sut.GetAllSurveys(_sut.GetDefaultConnectionModel()).ToList();
            Assert.IsNotNull(surveys.Where(s => s.Name == surveyName).First());

            //act
            _sut.UninstallSurvey(_sut.GetDefaultConnectionModel(), serverParkName, surveyName);

            //assert
            var surveysInstalled = _sut.GetAllSurveys(_sut.GetDefaultConnectionModel()).ToList();
            Assert.IsNull(surveysInstalled.Where(s => s.Name == surveyName).FirstOrDefault());
        }
    }
}
