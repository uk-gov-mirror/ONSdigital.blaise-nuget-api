using System;
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
        public void Given_ServerPark_Has_Been_Called_When_I_Call_ParkExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(p => p.ServerParkExists(It.IsAny<string>())).Returns(It.IsAny<bool>());

            _sut.ServerPark(_serverParkName);

            //act
            _sut.ParkExists();

            //assert
            _blaiseApiMock.Verify(v => v.ServerParkExists(_serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_ServerPark_Has_Been_Called_When_I_Call_ParkExists_Then_The_Expected_Result_Is_Returned(bool serverParkExists)
        {
            //arrange

            _blaiseApiMock.Setup(p => p.ServerParkExists(_serverParkName)).Returns(serverParkExists);

            _sut.ServerPark(_serverParkName);

            //act
            var result = _sut.ParkExists();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(serverParkExists, result);
        }

        [Test]
        public void Given_ServerPark_Has_Not_Been_Called_When_I_Call_ParkExists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.ParkExists());
            Assert.AreEqual("The 'ServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }
    }
}
