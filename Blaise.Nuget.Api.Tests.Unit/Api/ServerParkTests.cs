using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Unit.Api
{
    public class ServerParkTests
    {
        private Mock<IDataService> _dataServiceMock;
        private Mock<IParkService> _parkServiceMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IIocProvider> _unityProviderMock;
        private Mock<IConfigurationProvider> _configurationProviderMock;

        private readonly ConnectionModel _connectionModel;

        private IBlaiseApi _sut;

        public ServerParkTests()
        {
            _connectionModel = new ConnectionModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataServiceMock = new Mock<IDataService>();
            _parkServiceMock = new Mock<IParkService>();
            _surveyServiceMock = new Mock<ISurveyService>();
            _userServiceMock = new Mock<IUserService>();
            _fileServiceMock = new Mock<IFileService>();
            _unityProviderMock = new Mock<IIocProvider>();
            _configurationProviderMock = new Mock<IConfigurationProvider>();

            _sut = new BlaiseApi(
                _dataServiceMock.Object,
                _parkServiceMock.Object,
                _surveyServiceMock.Object,
                _userServiceMock.Object,
                _fileServiceMock.Object,
                _unityProviderMock.Object,
                _configurationProviderMock.Object);
        }
        [Test]
        public void When_I_Call_GetServerPark_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "serverParkName";
            var serverParkMock = new Mock<IServerPark>();

            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, serverParkName)).Returns(serverParkMock.Object);

            //act
            _sut.GetServerPark(_connectionModel, serverParkName);

            //assert
            _parkServiceMock.Verify(v => v.GetServerPark(_connectionModel, serverParkName), Times.Once);
        }

        [Test]
        public void When_I_Call_GetServerPark_Then_The_Correct_ServerPark_Is_Returned()
        {
            //arrange
            var serverParkName = "Park1";
            var serverParkMock = new Mock<IServerPark>();

            _parkServiceMock.Setup(p => p.GetServerPark(_connectionModel, serverParkName)).Returns(serverParkMock.Object);

            //act
            var result =_sut.GetServerPark(_connectionModel, serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreSame(serverParkMock.Object, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetServerPark(null, serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetServerPark(_connectionModel, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetServerPark(_connectionModel, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void When_I_Call_GetServerParks_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkMock1 = new Mock<IServerPark>();
            var serverParkMock2 = new Mock<IServerPark>();

            var serverParkItems = new List<IServerPark> { serverParkMock1.Object, serverParkMock2.Object };

            _parkServiceMock.Setup(p => p.GetServerParks(_connectionModel)).Returns(serverParkItems);

            //act
            _sut.GetServerParks(_connectionModel);

            //assert
            _parkServiceMock.Verify(v => v.GetServerParks(_connectionModel), Times.Once);
        }

        [Test]
        public void When_I_Call_GetServerParks_Then_The_Correct_ServerPark_Is_Returned()
        {
            //arrange
            var serverParkMock1 = new Mock<IServerPark>();
            var serverParkMock2 = new Mock<IServerPark>();

            var serverParkItems = new List<IServerPark> { serverParkMock1.Object, serverParkMock2.Object };

            _parkServiceMock.Setup(p => p.GetServerParks(_connectionModel)).Returns(serverParkItems);

            //act
            var result = _sut.GetServerParks(_connectionModel);

            //assert
            Assert.NotNull(result);
            Assert.AreSame(serverParkItems, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetServerParks_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetServerParks(null));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void When_I_Call_GetServerParkNames_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _parkServiceMock.Setup(p => p.GetServerParkNames(_connectionModel)).Returns(It.IsAny<List<string>>());

            //act
            _sut.GetServerParkNames(_connectionModel);

            //assert
            _parkServiceMock.Verify(v => v.GetServerParkNames(_connectionModel), Times.Once);
        }

        [Test]
        public void When_I_Call_GetServerParkNames_Then_The_Expected_Server_Park_Names_Are_Returned()
        {
            //arrange
            var serverParksNames = new List<string> { "Park1", "Park2" };

            _parkServiceMock.Setup(p => p.GetServerParkNames(_connectionModel)).Returns(serverParksNames);

            //act
            var result = _sut.GetServerParkNames(_connectionModel);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(serverParksNames, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetServerParkNames_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetServerParkNames(null));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_ServerParkExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "Park1";

            _parkServiceMock.Setup(p => p.ServerParkExists(_connectionModel, It.IsAny<string>())).Returns(It.IsAny<bool>());

            //act
            _sut.ServerParkExists(_connectionModel, serverParkName);

            //assert
            _parkServiceMock.Verify(v => v.ServerParkExists(_connectionModel, serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_ServerParkExists_Then_The_Expected_Result_Is_Returned(bool serverParkExists)
        {
            //arrange
            var serverParkName = "Park1";

            _parkServiceMock.Setup(p => p.ServerParkExists(_connectionModel, serverParkName)).Returns(serverParkExists);

            //act
            var result = _sut.ServerParkExists(_connectionModel, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(serverParkExists, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_ServerParkExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ServerParkExists(null, serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_ServerParkExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ServerParkExists(_connectionModel, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_ServerParkExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ServerParkExists(_connectionModel, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
    }
}
