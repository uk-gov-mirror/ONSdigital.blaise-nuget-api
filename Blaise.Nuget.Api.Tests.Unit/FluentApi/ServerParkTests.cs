using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class ServerParkTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _serverParkName;

        private FluentBlaiseApi _sut;

        public ServerParkTests()
        {
            _connectionModel = new ConnectionModel();
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
            _blaiseApiMock.Setup(p => p.GetServerParkNames(It.IsAny<ConnectionModel>())).Returns(It.IsAny<List<string>>());
            _sut.WithConnection(_connectionModel);

            //act
            var sutServerParks = _sut.ServerParks;

            //assert
            _blaiseApiMock.Verify(v => v.GetServerParkNames(It.IsAny<ConnectionModel>()), Times.Once);
        }

        [Test]
        public void Given_I_Call_ServerParks_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var serverParksNames = new List<string> { "Park1", "Park2" };
            _blaiseApiMock.Setup(p => p.GetServerParkNames(It.IsAny<ConnectionModel>())).Returns(serverParksNames);

            _sut.WithConnection(_connectionModel);

            //act
            var result = _sut.ServerParks;

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(serverParksNames, result);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_GetServerParkNames_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithConnection(_connectionModel);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var result = _sut.ServerParks;
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_WithServerPark_Has_Been_Called_When_I_Call_ParkExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.ServerParkExists(It.IsAny<ConnectionModel>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);

            //act
            var sutExists = _sut.Exists;

            //assert
            _blaiseApiMock.Verify(v => v.ServerParkExists(It.IsAny<ConnectionModel>(), _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_WithServerPark_Has_Been_Called_When_I_Call_ParkExists_Then_The_Expected_Result_Is_Returned(bool serverParkExists)
        {
            //arrange
            _blaiseApiMock.Setup(p => p.ServerParkExists(It.IsAny<ConnectionModel>(), _serverParkName)).Returns(serverParkExists);

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);

            //act
            var result = _sut.Exists;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(serverParkExists, result);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_ParkExists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var result = _sut.Exists;
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }
    }
}
