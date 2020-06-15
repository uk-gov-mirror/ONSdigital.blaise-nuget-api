using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit
{
    public class BlaiseApiTests
    {
        [Test]
        public void Given_I_Instantiate_BlaiseApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseApi());
        }
    }
}
