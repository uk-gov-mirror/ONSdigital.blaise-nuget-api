using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

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
        public void Given_I_Call_ServerParkNames_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetServerParkNames(It.IsAny<ConnectionModel>())).Returns(It.IsAny<List<string>>());
            _sut.WithConnection(_connectionModel);

            //act
            var sutServerParks = _sut.ServerParkNames;

            //assert
            _blaiseApiMock.Verify(v => v.GetServerParkNames(It.IsAny<ConnectionModel>()), Times.Once);
        }

        [Test]
        public void Given_I_Call_ServerParkNames_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var serverParksNames = new List<string> { "Park1", "Park2" };
            _blaiseApiMock.Setup(p => p.GetServerParkNames(It.IsAny<ConnectionModel>())).Returns(serverParksNames);

            _sut.WithConnection(_connectionModel);

            //act
            var result = _sut.ServerParkNames;

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(serverParksNames, result);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_ServerParkNames_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithConnection(_connectionModel);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var result = _sut.ServerParkNames;
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_I_Call_ServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetServerParks(It.IsAny<ConnectionModel>())).Returns(It.IsAny<List<IServerPark>>());
            _sut.WithConnection(_connectionModel);

            //act
            var sutServerParks = _sut.ServerParks;

            //assert
            _blaiseApiMock.Verify(v => v.GetServerParks(It.IsAny<ConnectionModel>()), Times.Once);
        }

        [Test]
        public void Given_I_Call_ServerParks_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var serverParkMock1 = new Mock<IServerPark>();
            var serverParkMock2 = new Mock<IServerPark>();

            var serverParkItems = new List<IServerPark> { serverParkMock1.Object, serverParkMock2.Object };
            _blaiseApiMock.Setup(p => p.GetServerParks(It.IsAny<ConnectionModel>())).Returns(serverParkItems);

            _sut.WithConnection(_connectionModel);

            //act
            var result = _sut.ServerParks;

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(serverParkItems, result);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_ServerParks_Then_An_NullReferenceException_Is_Thrown()
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
        public void Given_I_Call_ServerPark_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "Park1";
            _blaiseApiMock.Setup(p => p.GetServerPark(It.IsAny<ConnectionModel>(), It.IsAny<string>())).Returns(It.IsAny<IServerPark>());
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(serverParkName);

            //act
            var sutServerPark = _sut.ServerPark;

            //assert
            _blaiseApiMock.Verify(v => v.GetServerPark(It.IsAny<ConnectionModel>(), serverParkName), Times.Once);
        }

        [Test]
        public void Given_I_Call_ServerPark_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var serverParkName = "Park1";
            var serverParkMock = new Mock<IServerPark>();
            _blaiseApiMock.Setup(p => p.GetServerPark(It.IsAny<ConnectionModel>(), It.IsAny<string>())).Returns(serverParkMock.Object);
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(serverParkName);

            //act
            var result = _sut.ServerPark;

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(serverParkMock.Object, result);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_ServerPark_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";

            //_sut.WithConnection(_connectionModel);
            _sut.WithServerPark(serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var result = _sut.ServerPark;
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_ServerPark_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            //_sut.WithServerPark(serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var result = _sut.ServerPark;
            });

            Assert.AreEqual(
                "The 'WithServerPark' step needs to be called with a valid value, check that the step has been called with a valid value for the server park",
                exception.Message);
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
