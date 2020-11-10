using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behavior.Backup
{
    public class BackupInstrumentTests
    {
        private readonly BlaiseApi _sut;

        public BackupInstrumentTests()
        {
            _sut = new BlaiseApi();
        }

        [Test]
        public void Given_A_Valid_Instrument_When_I_Call_BackupToFile_Then_An_Instrument_Is_Backed_Up_To_File()
        {
            //arrange
            var serverParkName = "LocalDevelopment";
            var instrumentName = "OPN2004A";
            var outputPath = @"d:\temp\backup";

            //act
            _sut.BackupSurveyToFile(_sut.GetDefaultConnectionModel(), serverParkName, instrumentName, outputPath);

            //arrange
        }
    }
}
