using Blaise.Nuget.Api.Api;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.File
{
    public class InstrumentFileTests
    {
        private readonly BlaiseFileApi _sut;

        public InstrumentFileTests()
        {
            _sut = new BlaiseFileApi();
        }

        [Ignore("Integration")]
        [Test]
        public void Given_I_Call_UpdateInstrumentFileWithSqlConnection_Then_The_Instrument_Is_Updated()
        {
            //arrange
            const string instrumentName = "opn2101a";
            const string instrumentFile = @"D:\Opn\Temp\OPN2101A.zip";

            //act && assert
            Assert.DoesNotThrow(() =>_sut.UpdateInstrumentFileWithSqlConnection( instrumentName,
                instrumentFile));
        }
    }
}
