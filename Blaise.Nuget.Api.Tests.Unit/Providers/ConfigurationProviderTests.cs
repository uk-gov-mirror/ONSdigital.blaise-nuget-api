using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Providers;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Providers
{
    public class ConfigurationProviderTests
    {
        /// <summary>
        /// Please ensure the app.config in the test project has values that relate to the tests
        /// </summary>

        [Test]
        public void Given_AppConfig_Values_Are_Set_When_I_Call_GetConnectionModel_I_Get_A_ConnectionModel_Back()
        {
            //arrange
            var sut = new ConfigurationProvider();

            //act
            var result = sut.GetConnectionModel();

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ConnectionModel>(result);
            Assert.AreEqual("BlaiseServerHostNameTest", result.ServerName);
            Assert.AreEqual("BlaiseServerUserNameTest", result.UserName);
            Assert.AreEqual("BlaiseServerPasswordTest", result.Password);
            Assert.AreEqual("BlaiseServerBindingTest", result.Binding);
            Assert.AreEqual(10, result.Port);
            Assert.AreEqual(20, result.RemotePort);
        }

        [Test]
        public void Given_LibraryDirectory_Value_Is_Set_When_I_Call_LibraryDirectory_I_Get_The_Correct_Value_Back()
        {
            //arrange
            var sut = new ConfigurationProvider();

            //act
            var result = sut.LibraryDirectory;

            //assert
            Assert.NotNull(result);
            Assert.AreEqual("LibraryDirectoryTest", result);
        }
    }
}
