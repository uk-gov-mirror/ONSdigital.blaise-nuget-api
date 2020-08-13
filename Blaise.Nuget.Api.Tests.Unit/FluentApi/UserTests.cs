using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class UserTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _userName;
        private readonly string _password;
        private readonly string _role;
        private readonly List<string> _serverParkNameList;

        private FluentBlaiseApi _sut;

        public UserTests()
        {
            _connectionModel = new ConnectionModel();
            _userName = "User1";
            _password = "Password1";
            _role = "King";
            _serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };
        }

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();

            _sut = new FluentBlaiseApi(_blaiseApiMock.Object);
        }

        [Test]
        public void Given_All_Steps_Have_Been_Called_When_I_Call_Add_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);
            _sut.WithPassword(_password);
            _sut.WithRole(_role);
            _sut.WithServerParks(_serverParkNameList);

            //act
            _sut.Add();

            //assert
            _blaiseApiMock.Verify(v => v.AddUser(_connectionModel, _userName, _password, _role, _serverParkNameList), Times.Once);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithUserName(_userName);
            _sut.WithPassword(_password);
            _sut.WithRole(_role);
            _sut.WithServerParks(_serverParkNameList);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_WithUser_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithPassword(_password);
            _sut.WithRole(_role);
            _sut.WithServerParks(_serverParkNameList);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual("The 'WithUser' step needs to be called with a valid value, check that the step has been called with a valid user name", exception.Message);
        }

        [Test]
        public void Given_WithPassword_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);
            _sut.WithRole(_role);
            _sut.WithServerParks(_serverParkNameList);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual("The 'WithPassword' step needs to be called with a valid value, check that the step has been called with a valid password", exception.Message);
        }

        [Test]
        public void Given_WithRole_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);
            _sut.WithPassword(_password);
            _sut.WithServerParks(_serverParkNameList);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual("The 'WithRole' step needs to be called with a valid value, check that the step has been called with a valid role", exception.Message);
        }

        [Test]
        public void Given_WithServerParks_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);
            _sut.WithPassword(_password);
            _sut.WithRole(_role);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual("The 'WithServerParks' step needs to be called with at least one valid server park, check that the step has been called with a valid server park(s)", exception.Message);
        }

        [Test]
        public void Given_All_Steps_Have_Been_Called_When_I_Call_Update_Then_The_Correct_Service_Methods_Are_Called()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);
            _sut.WithPassword(_password);
            _sut.WithRole(_role);
            _sut.WithServerParks(_serverParkNameList);

            //act
            _sut.Update();

            //assert
            _blaiseApiMock.Verify(v => v.ChangePassword(It.IsAny<ConnectionModel>(), _userName, _password), Times.Once);
            _blaiseApiMock.Verify(v => v.EditUser(It.IsAny<ConnectionModel>(), _userName, _role, _serverParkNameList), Times.Once);
        }

        [Test]
        public void Given_Only_WithPassword_Step_Has_Been_Called_When_I_Call_Update_Then_The_Correct_Service_Methods_Are_Called()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);
            _sut.WithPassword(_password);

            //act
            _sut.Update();

            //assert
            _blaiseApiMock.Verify(v => v.ChangePassword(It.IsAny<ConnectionModel>(), _userName, _password), Times.Once);
            _blaiseApiMock.Verify(v => v.EditUser(It.IsAny<ConnectionModel>(), _userName, _role, _serverParkNameList), Times.Never);
        }

        [Test]
        public void Given_Only_WithRole_And_WithServerParks_Steps_Have_Been_Called_When_I_Call_Update_Then_The_Correct_Service_Methods_Are_Called()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);
            _sut.WithRole(_role);
            _sut.WithServerParks(_serverParkNameList);

            //act
            _sut.Update();

            //assert
            _blaiseApiMock.Verify(v => v.ChangePassword(It.IsAny<ConnectionModel>(), _userName, _password), Times.Never);
            _blaiseApiMock.Verify(v => v.EditUser(It.IsAny<ConnectionModel>(), _userName, _role, _serverParkNameList), Times.Once);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithUserName(_userName);
            _sut.WithRole(_role);
            _sut.WithServerParks(_serverParkNameList);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_WithUser_Has_Not_Been_Called_But_WithPassword_Has_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithPassword(_password);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithUser' step needs to be called with a valid value, check that the step has been called with a valid user name", exception.Message);
        }

        [Test]
        public void Given_WithUser_Has_Not_Been_Called_But_WithRole_Has_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithRole(_role);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithUser' step needs to be called with a valid value, check that the step has been called with a valid user name", exception.Message);
        }

        [Test]
        public void Given_WithUser_Has_Not_Been_Called_But_WithServerParks_Has_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerParks(_serverParkNameList);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithUser' step needs to be called with a valid value, check that the step has been called with a valid user name", exception.Message);
        }

        [Test]
        public void Given_WithRole_Has_Not_Been_Called_But_WithServerParks_Has_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);
            _sut.WithServerParks(_serverParkNameList);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithRole' step needs to be called with a valid value, check that the step has been called with a valid role", exception.Message);
        }

        [Test]
        public void Given_WithServerParks_Has_Not_Been_Called_But_WithRole_Has_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);
            _sut.WithRole(_role);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithServerParks' step needs to be called with at least one valid server park, check that the step has been called with a valid server park(s)", exception.Message);
        }

        [Test]
        public void Given_User_Has_Been_Called_When_I_Call_Exists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.UserExists(It.IsAny<ConnectionModel>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);

            //act
            var sutExists = _sut.Exists;

            //assert
            _blaiseApiMock.Verify(v => v.UserExists(It.IsAny<ConnectionModel>(), _userName), Times.Once);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_Exists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.UserExists(It.IsAny<ConnectionModel>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            _sut.WithUserName(_userName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Exists;
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_WithServerPark_Has_Been_Called_When_I_Call_Exists_Then_The_Expected_Result_Is_Returned(bool userExists)
        {
            //arrange
            _blaiseApiMock.Setup(p => p.UserExists(It.IsAny<ConnectionModel>(), _userName)).Returns(userExists);

            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);

            //act
            var result = _sut.Exists;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userExists, result);
        }

        [Test]
        public void Given_User_Has_Been_Called_When_I_Call_Remove_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithUserName(_userName);

            //act
            _sut.Remove();

            //assert
            _blaiseApiMock.Verify(v => v.RemoveUser(It.IsAny<ConnectionModel>(), _userName), Times.Once);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_Remove_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithUserName(_userName);

            var setup = _sut.User;

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.User.Remove());
            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_WithUser_Has_Not_Been_Called_When_I_Call_Remove_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);

            var setup = _sut.User;

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.User.Remove());
            Assert.AreEqual("The 'WithUser' step needs to be called with a valid value, check that the step has been called with a valid user name", exception.Message);
        }
    }
}
