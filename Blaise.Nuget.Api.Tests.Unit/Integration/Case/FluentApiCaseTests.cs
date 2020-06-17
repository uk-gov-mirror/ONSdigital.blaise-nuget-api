using Blaise.Nuget.Api.Contracts.Interfaces;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Integration.Case
{
    public class FluentApiCaseTests
    {
        //[Ignore("Wont run without app settings on build environment")]
        [Test]
        public void Given_A_Case_That_Exists_When_I_Call_Exists_True_Is_Returned()
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act
            var result = sut
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Case
                .WithPrimaryKey("11000011")
                .Exists;

            //assert
            Assert.True(result);
        }
    }
}
