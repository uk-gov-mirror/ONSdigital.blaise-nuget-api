using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behavior.Backup
{
    public class BackupInstrumentTests
    {
        private BlaiseApi sut;

        public BackupInstrumentTests()
        {
            sut = new BlaiseApi();
        }

        [Test]
        public void Given_A_Valid_Instrument_When_I_Call_BackupToFile_Then_An_Instrument_Is_Backed_Up_To_File()
        {
            //arrange
            var serverParkName = "Tel";
            var instrumentName = "";
            var outputPath = @"d:\temp\backup";

            //act
            sut.BackupSurveyToFile(sut.GetDefaultConnectionModel(), serverParkName, instrumentName, outputPath);

            //arrange
        }
    }
}
