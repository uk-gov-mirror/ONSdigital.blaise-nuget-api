using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit
{
    public class FluentBlaiseApiTests
    {
        [Test]
        public void Given_I_Instantiate_FluentBlaiseApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            Assert.DoesNotThrow(() => new FluentBlaiseApi());
        }
    }
}
