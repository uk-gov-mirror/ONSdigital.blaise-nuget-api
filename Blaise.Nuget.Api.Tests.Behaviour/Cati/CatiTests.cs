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
    }
}
