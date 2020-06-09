using Blaise.Nuget.Api.Contracts.Interfaces;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Integration.Handler
{
    public class FluentApiHandlerTests
    {
        [Ignore("Wont run without app settings on build environment")]
        [Test]
        public void Given_Valid_Arguments_For_File_When_I_Call_Copy_Then_A_Case_Is_Copied_To_A_File()
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act
            sut
                .WithServer("localhost")
                .WithServerPark("LocalDevelopment")
                .WithInstrument("OPN2004A")
                .Case
                .WithPrimaryKey("1100021")
                .Copy
                .ToFile(@"d:\temp\OPN\Handler")
                .ToInstrument("OPN2004A")
                .Handle();

            //assert
        }
    }
}
