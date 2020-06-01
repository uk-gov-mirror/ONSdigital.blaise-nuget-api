using Blaise.Nuget.Api.Core.Providers;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Providers
{
    public class ConfigurationProviderTests
    {
        /// <summary>
        /// Please ensure the app.config in the test project has values that relate to the tests
        /// </summary>

        [Test]
        public void Given_AppConfig_Values_Are_Set_When_I_Call_ServerName_I_Get_The_Expected_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.ServerName;

            //assert
            Assert.NotNull(result);
            Assert.AreEqual("BlaiseServerHostNameTest", result);
        }

        [Test]
        public void Given_AppConfig_Values_Are_Set_When_I_Call_UserName_I_Get_The_Expected_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.UserName;

            //assert
            Assert.NotNull(result);
            Assert.AreEqual("BlaiseServerUserNameTest", result);
        }

        [Test]
        public void Given_AppConfig_Values_Are_Set_When_I_Call_Password_I_Get_The_Expected_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.Password;

            //assert
            Assert.NotNull(result);
            Assert.AreEqual("BlaiseServerPasswordTest", result);
        }

        [Test]
        public void Given_AppConfig_Values_Are_Set_When_I_Call_Binding_I_Get_The_Expected_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.Binding;

            //assert
            Assert.NotNull(result);
            Assert.AreEqual("BlaiseServerBindingTest", result);
        }

        [Test]
        public void Given_AppConfig_Values_Are_Set_When_I_Call_ConnectionPort_I_Get_The_Expected_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.ConnectionPort;

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(10, result);
        }

        [Test]
        public void Given_AppConfig_Values_Are_Set_When_I_Call_RemoteConnectionPort_I_Get_The_Expected_Value_Back()
        {
            //arrange
            var configurationProvider = new ConfigurationProvider();

            //act
            var result = configurationProvider.RemoteConnectionPort;

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(20, result);
        }
    }
}
