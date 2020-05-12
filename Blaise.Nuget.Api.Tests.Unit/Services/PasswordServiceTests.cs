
using Blaise.Nuget.Api.Core.Services;
using NUnit.Framework;
using System.Security;

namespace BlaiseCaseCreator_UnitTests.Blaise
{
    public class PasswordServiceTests
    {
        [Test]
        public void Given_I_Call_CreateSecurePassword_A_SecureString_Is_Returned()
        {
            //arrange
            var password = "Password123";
            var passwordService = new PasswordService();

            //act
            var result = passwordService.CreateSecurePassword(password);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<SecureString>(result);
        }
    }
}
