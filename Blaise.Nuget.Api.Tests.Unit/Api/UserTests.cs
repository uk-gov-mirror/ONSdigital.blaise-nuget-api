using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Api
{
    public class UserTests
    {
        private Mock<IDataService> _dataServiceMock;
        private Mock<IParkService> _parkServiceMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IUserService> _userServiceMock;


        private readonly string _userName;
        private readonly string _password;

        private BlaiseApi _sut;

        public UserTests()
        {
            _userName = "User1";
            _password = "Password1";
        }
        [SetUp]
        public void SetUpTests()
        {
            _dataServiceMock = new Mock<IDataService>();
            _parkServiceMock = new Mock<IParkService>();
            _surveyServiceMock = new Mock<ISurveyService>();
            _userServiceMock = new Mock<IUserService>();

            _sut = new BlaiseApi(
                _dataServiceMock.Object,
                _parkServiceMock.Object,
                _surveyServiceMock.Object,
                _userServiceMock.Object);
        }

        [Test]
        public void When_I_Call_AddUser_Then_The_Correct_Service_Method_Is_Called()
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
            _userServiceMock.Verify(v => v.AddUser(_userName, _password, role, serverParkNameList), Times.Once);
        }


        [Test]
        public void Given_An_Empty_UserName_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(string.Empty, _password, role, serverParkNameList));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(null, _password, role, serverParkNameList));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Password_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_userName, string.Empty, role, serverParkNameList));
            Assert.AreEqual("A value for the argument 'password' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_Password_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_userName, null, role, serverParkNameList));
            Assert.AreEqual("password", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Role_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_userName, _password, string.Empty, serverParkNameList));
            Assert.AreEqual("A value for the argument 'role' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_Role_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_userName, _password, null, serverParkNameList));
            Assert.AreEqual("role", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkList_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_userName, _password, role, serverParkNameList));
            Assert.AreEqual("A value for the argument 'serverParkNames' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_ServerParkList_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_userName, _password, role, null));
            Assert.AreEqual("serverParkNames", exception.ParamName);
        }

        [Test]
        public void When_I_Call_ChangePassword_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.ChangePassword(_userName, _password);

            //assert
            _userServiceMock.Verify(v => v.ChangePassword(_userName, _password), Times.Once);
        }


        [Test]
        public void Given_An_Empty_UserName_When_I_Call_ChangePassword_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ChangePassword(string.Empty, _password));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_ChangePassword_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ChangePassword(null, _password));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Password_When_I_Call_ChangePassword_Then_An_ArgumentException_Is_Thrown()
        {
             //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ChangePassword(_userName, string.Empty));
            Assert.AreEqual("A value for the argument 'password' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_Password_When_I_Call_ChangePassword_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ChangePassword(_userName, null));
            Assert.AreEqual("password", exception.ParamName);
        }

        [Test]
        public void When_I_Call_UserExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.UserExists(_userName);

            //assert
            _userServiceMock.Verify(v => v.UserExists(_userName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void When_I_Call_UserExists_Then_The_Expected_Result_Is_Returned(bool userExists)
        {
            //arrange
            _userServiceMock.Setup(u => u.UserExists(_userName)).Returns(userExists);

            //act
            var result = _sut.UserExists(_userName);

            //assert
           Assert.AreEqual(userExists, result);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_UserExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UserExists(string.Empty));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_UserExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UserExists(null));
            Assert.AreEqual("userName", exception.ParamName);
        }
    }
}
