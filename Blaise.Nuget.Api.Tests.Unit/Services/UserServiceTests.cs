
using System;
using System.Collections.Generic;
using System.Security;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class UserServiceTests
    {
        private Mock<IConnectedServerFactory> _connectionFactoryMock;
        private Mock<IPasswordService> _passwordServiceMock;

        private Mock<IServerPark> _serverParkMock;
        private Mock<IConnectedServer> _connectedServerMock;
        private Mock<IServerParkCollection> _serverParkCollectionMock;

        private Mock<IUser2> _userMock;
        private Mock<IUserServerParkCollection> _userServerParkCollectionMock;
        private Mock<IUserCollection> _userCollectionMock;

        private readonly string _serverParkName;
        private readonly string _userName;
        private readonly string _password;
        private readonly SecureString _securePassword;

        private UserService _sut;

        public UserServiceTests()
        {
            _serverParkName = "TestServerParkName";
            _userName = "User1";
            _password = "Password1";
            _securePassword = new SecureString();
        }

        [SetUp]
        public void SetUpTests()
        {
            //setup server parks
            _serverParkMock = new Mock<IServerPark>();
            _serverParkMock.Setup(s => s.Name).Returns("TestServerParkName");

            var serverParkItems = new List<IServerPark> { _serverParkMock.Object };

            _serverParkCollectionMock = new Mock<IServerParkCollection>();
            _serverParkCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => serverParkItems.GetEnumerator());

            //setup user mocks
            _userServerParkCollectionMock = new Mock<IUserServerParkCollection>();

            _userMock = new Mock<IUser2>();
            _userMock.Setup(u => u.ServerParks).Returns(_userServerParkCollectionMock.Object);

            var userItems = new List<IUser> { _userMock.Object };

            _userCollectionMock = new Mock<IUserCollection>();
            _userCollectionMock.Setup(u => u.GetEnumerator()).Returns(() => userItems.GetEnumerator());
            _userCollectionMock.Setup(u => u.GetItem(It.IsAny<string>())).Returns(_userMock.Object);

            //setup connection
            _connectedServerMock = new Mock<IConnectedServer>();
            _connectedServerMock.Setup(c => c.ServerParks).Returns(_serverParkCollectionMock.Object);
            _connectedServerMock.Setup(c => c.GetServerPark(_serverParkName)).Returns(_serverParkMock.Object);
            _connectedServerMock.Setup(c => c.Users).Returns(_userCollectionMock.Object);

            _connectionFactoryMock = new Mock<IConnectedServerFactory>();
            _connectionFactoryMock.Setup(c => c.GetConnection()).Returns(_connectedServerMock.Object);
            _connectedServerMock.Setup(c => c.AddUser(It.IsAny<string>(), It.IsAny<SecureString>()))
                .Returns(_userMock.Object);

            //setup password service
            _passwordServiceMock = new Mock<IPasswordService>();
            _passwordServiceMock.Setup(p => p.CreateSecurePassword(It.IsAny<string>())).Returns(_securePassword);

            //setup service under test
            _sut = new UserService(_connectionFactoryMock.Object, _passwordServiceMock.Object);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_AddUser_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act
            _sut.AddUser(_userName, _password, role, serverParkNameList);

            //assert
            _passwordServiceMock.Verify(v => v.CreateSecurePassword(_password), Times.Once);
            _connectedServerMock.Verify(v => v.AddUser(_userName, _securePassword), Times.Once);

            foreach (var serverParkName in serverParkNameList)
            {
                _userServerParkCollectionMock.Verify(v => v.Add(serverParkName), Times.Once);
            }

            _userMock.VerifySet(u => u.Role = role, Times.Once);
            _userMock.Verify(v => v.Save(), Times.Once);
        }

        [Test]
        public void Given_An_Error_Occurs_In_Setting_The_User_Role_When_I_Call_AddUser_Then_The_User_Is_Still_Added()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            _userMock.Setup(u => u.Role).Throws(new Exception());

            //act
            _sut.AddUser(_userName, _password, role, serverParkNameList);

            //assert
            _passwordServiceMock.Verify(v => v.CreateSecurePassword(_password), Times.Once);
            _connectedServerMock.Verify(v => v.AddUser(_userName, _securePassword), Times.Once);

            foreach (var serverParkName in serverParkNameList)
            {
                _userServerParkCollectionMock.Verify(v => v.Add(serverParkName), Times.Once);
            }

            _userMock.Verify(v => v.Save(), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_EditUser_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act
            _sut.EditUser(_userName, role, serverParkNameList);

            //assert
            _connectedServerMock.Verify(v => v.Users.GetItem(_userName), Times.Once);

            _userMock.Verify(u => u.ServerParks.Clear(), Times.Once);

            foreach (var serverParkName in serverParkNameList)
            {
                _userServerParkCollectionMock.Verify(v => v.Add(serverParkName), Times.Once);
            }

            _userMock.VerifySet(u => u.Role = role, Times.Once);
            _userMock.Verify(v => v.Save(), Times.Once);
        }

        [Test]
        public void Given_An_Error_Occurs_In_Setting_The_User_Role_When_I_Call_EditUser_Then_The_User_Is_Still_Added()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            _userMock.Setup(u => u.Role).Throws(new Exception());

            //act
            _sut.EditUser(_userName, role, serverParkNameList);

            //assert
            _connectedServerMock.Verify(v => v.Users.GetItem(_userName), Times.Once);

            _userMock.Verify(u => u.ServerParks.Clear(), Times.Once);

            foreach (var serverParkName in serverParkNameList)
            {
                _userServerParkCollectionMock.Verify(v => v.Add(serverParkName), Times.Once);
            }

            _userMock.Verify(v => v.Save(), Times.Once);
        }

        [Test]
        public void Give_Valid_Parameters_When_I_Call_ChangePassword_Then_The_Correct_Services_Are_Called()
        {
            //arrange

            //act
            _sut.ChangePassword(_userName, _password);

            //assert
            _passwordServiceMock.Verify(v => v.CreateSecurePassword(_password), Times.Once);
            _connectedServerMock.Verify(v => v.Users.GetItem(_userName), Times.Once);
            _userMock.Verify(v => v.ChangePassword(_securePassword), Times.Once);
            _userMock.Verify(v => v.Save(), Times.Once);
        }

        [TestCase("User1")]
        [TestCase("USER1")]
        [TestCase("user1")]
        [TestCase("uSeR1")]
        public void Given_A_User_Exists_When_I_Call_UserExists_Then_True_Is_Returned(string userName)
        {
            //arrange
            _userMock.Setup(u => u.Name).Returns(_userName);

            //act
            var result = _sut.UserExists(userName);

            //assert
           Assert.IsTrue(result);
        }

        [Test]
        public void Given_A_User_Does_Not_Exist_When_I_Call_UserExists_Then_False_Is_Returned()
        {
            //arrange 
            _userMock.Setup(u => u.Name).Returns("NotFound");
            
            //act
            var result = _sut.UserExists(_userName);

            //assert
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_RemoveUser_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.RemoveUser(_userName);

            //assert
            _connectedServerMock.Verify(v => v.RemoveUser(_userName), Times.Once);
        }
    }
}
