using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Factories
{
    public class ConnectedServerFactoryTests
    {
        /// <summary>
        /// Please ensure the app.config in the test project has values that relate to local environment
        /// </summary>

        [Ignore("Wont run without app settings on build environment")]
        [Test]
        public void Given_I_Call_GetServerParkNames_I_Get_The_Expected_Values_Back()
        {
            //arrange
            var blaiseApi = new BlaiseApi();

            //act
            var result = blaiseApi.GetServerParkNames().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(1, result.Count());
            Assert.True(result.Contains("LocalDevelopment"));
        }
    }
}
