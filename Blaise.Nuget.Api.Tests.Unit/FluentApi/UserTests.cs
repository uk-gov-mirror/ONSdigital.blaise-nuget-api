using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class UserTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly string _userName;
        private readonly string _password;

        private FluentBlaiseApi _sut;

        public UserTests()
        {
            _userName = "User1";
            _password = "Password1";
        }

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();

            _sut = new FluentBlaiseApi(_blaiseApiMock.Object);
        }

        [Test]
        public void Given_User_Has_Been_Called_When_I_Call_Add_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            _sut.User(_userName);

            //act
            _sut.Add(_password, role, serverParkNameList);

            //assert
            _blaiseApiMock.Verify(v => v.AddUser(_userName, _password, role, serverParkNameList), Times.Once);
        }

        [Test]
        public void Given_User_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add(_password, role, serverParkNameList));
            Assert.AreEqual("The 'User' step needs to be called prior to this to specify the name of the user", exception.Message);
        }

        [Test]
        public void Given_User_Has_Been_Called_When_I_Call_Update_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            _sut.User(_userName);

            //act
            _sut.Update(role, serverParkNameList);

            //assert
            _blaiseApiMock.Verify(v => v.EditUser(_userName, role, serverParkNameList), Times.Once);
        }

        [Test]
        public void Given_User_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update(role, serverParkNameList));
            Assert.AreEqual("The 'User' step needs to be called prior to this to specify the name of the user", exception.Message);
        }

        [Test]
        public void Given_User_Has_Been_Called_When_I_Call_ChangePassword_Then_The_Correct_Service_Method_Is_Called()
        {
            _sut.User(_userName);

            //act
            _sut.ChangePassword(_password);

            //assert
            _blaiseApiMock.Verify(v => v.ChangePassword(_userName, _password), Times.Once);
        }

        [Test]
        public void Given_User_Has_Not_Been_Called_When_I_Call_AddUserChangePassword_Then_An_NullReferenceException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.ChangePassword(_password));
            Assert.AreEqual("The 'User' step needs to be called prior to this to specify the name of the user", exception.Message);
        }

        [Test]
        public void Given_User_Has_Been_Called_When_I_Call_Exists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(p => p.UserExists(It.IsAny<string>())).Returns(It.IsAny<bool>());

            _sut.User(_userName);

            //act
            _sut.Exists();

            //assert
            _blaiseApiMock.Verify(v => v.UserExists(_userName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_ServerPark_Has_Been_Called_When_I_Call_Exists_Then_The_Expected_Result_Is_Returned(bool userExists)
        {
            //arrange

            _blaiseApiMock.Setup(p => p.UserExists(_userName)).Returns(userExists);

            _sut.User(_userName);

            //act
            var result = _sut.Exists();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userExists, result);
        }

        [Test]
        public void Given_User_Has_Been_Called_When_I_Call_Remove_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _sut.User(_userName);

            //act
            _sut.Remove();

            //assert
            _blaiseApiMock.Verify(v => v.RemoveUser(_userName), Times.Once);
        }

        [Test]
        public void Given_Name_Has_Not_Been_Called_When_I_Call_Remove_Then_An_NullReferenceException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Remove());
            Assert.AreEqual("The 'User' step needs to be called prior to this to specify the name of the user", exception.Message);
        }
    }
}
