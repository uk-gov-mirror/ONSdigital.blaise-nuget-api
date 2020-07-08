using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class ConnectionExpiryServiceTests
    {
        private Mock<IConfigurationProvider> _configurationProviderMock;

        [SetUp]
        public void SetUpTests()
        {
            _configurationProviderMock = new Mock<IConfigurationProvider>();
        }

        [Test]
        public void Given_The_Connection_Has_Expired_When_I_Call_ConnectionHasExpired_Then_True_Is_Returned()
        {
            //arrange
            _configurationProviderMock.Setup(c => c.ConnectionExpiresInMinutes).Returns(-1);

            var sut = new ConnectionExpiryService(_configurationProviderMock.Object);

            //act
            var result = sut.ConnectionHasExpired();

            //assert
            Assert.NotNull(result);
            Assert.True(result);
        }

        [Test]
        public void Given_The_Connection_Has_Not_Expired_When_I_Call_ConnectionHasExpired_Then_False_Is_Returned()
        {
            //arrange
            _configurationProviderMock.Setup(c => c.ConnectionExpiresInMinutes).Returns(1);

            var sut = new ConnectionExpiryService(_configurationProviderMock.Object);

            //act
            var result = sut.ConnectionHasExpired();

            //assert
            Assert.NotNull(result);
            Assert.False(result);
        }

        [Test]
        public void Given_The_Connection_Has_Expired_And_I_Call_ResetConnectionExpiryPeriod_When_I_Call_ConnectionHasExpired_Then_False_Is_Returned()
        {
            //arrange
            _configurationProviderMock.Setup(c => c.ConnectionExpiresInMinutes).Returns(-1);

            var sut = new ConnectionExpiryService(_configurationProviderMock.Object);

            //act && assert
            var result = sut.ConnectionHasExpired();

            Assert.NotNull(result);
            Assert.True(result);

            _configurationProviderMock.Setup(c => c.ConnectionExpiresInMinutes).Returns(1);
            sut.ResetConnectionExpiryPeriod();

            result = sut.ConnectionHasExpired();

            Assert.NotNull(result);
            Assert.False(result);
        }
    }
}
