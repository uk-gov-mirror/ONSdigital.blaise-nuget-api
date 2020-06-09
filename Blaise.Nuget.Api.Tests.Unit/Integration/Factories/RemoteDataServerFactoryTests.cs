using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Tests.Unit.Integration.Factories
{
    public class RemoteDataServerFactoryTests
    {
        /// <summary>
        /// Please ensure the app.config in the test project has values that relate to local environment
        /// </summary>

        [Ignore("Wont run without app settings on build environment")]
        [Test]
        public void Given_I_Call_GetServerParkNames_I_Get_The_Expected_Values_Back()
        {
            //arrange
            var instrumentName = "OPN2004A";
            var serverPark = "LocalDevelopment";

            var blaiseApi = new BlaiseApi();

            //act
            var result = blaiseApi.GetDataSet(instrumentName, serverPark);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IDataSet>(result);
        }
    }
}
