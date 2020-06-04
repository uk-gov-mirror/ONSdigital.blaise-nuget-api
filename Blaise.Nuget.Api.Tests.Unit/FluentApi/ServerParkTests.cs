using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class ServerParkTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly string _serverParkName;

        private FluentBlaiseApi _sut;

        public ServerParkTests()
        {
            _serverParkName = "Park1";
        }

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();

            _sut = new FluentBlaiseApi(_blaiseApiMock.Object);
        }

        [Test]
        public void Given_I_Call_ServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetServerParkNames()).Returns(It.IsAny<List<string>>());

            //act
            _sut.ServerParks();

            //assert
            _blaiseApiMock.Verify(v => v.GetServerParkNames(), Times.Once);
        }

        [Test]
        public void Given_I_Call_ServerParks_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var serverParksNames = new List<string> { "Park1", "Park2" };
            _blaiseApiMock.Setup(p => p.GetServerParkNames()).Returns(serverParksNames);

            //act
            var result = _sut.ServerParks();

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(serverParksNames, result);
        }

        [Test]
        public void Given_WithServerPark_Has_Been_Called_When_I_Call_ParkExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.ServerParkExists(It.IsAny<string>())).Returns(It.IsAny<bool>());

            _sut.WithServerPark(_serverParkName);

            //act
            _sut.Exists();

            //assert
            _blaiseApiMock.Verify(v => v.ServerParkExists(_serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_WithServerPark_Has_Been_Called_When_I_Call_ParkExists_Then_The_Expected_Result_Is_Returned(bool serverParkExists)
        {
            //arrange

            _blaiseApiMock.Setup(p => p.ServerParkExists(_serverParkName)).Returns(serverParkExists);

            _sut.WithServerPark(_serverParkName);

            //act
            var result = _sut.Exists();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(serverParkExists, result);
        }
    }
}
