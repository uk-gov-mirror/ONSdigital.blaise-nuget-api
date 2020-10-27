using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Interfaces;
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
        private Mock<IFileService> _fileServiceMock;
        private Mock<IIocProvider> _unityProviderMock;
        private Mock<IConfigurationProvider> _configurationProviderMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _userName;
        private readonly string _password;

        private IBlaiseApi _sut;

        public UserTests()
        {
            _connectionModel = new ConnectionModel();
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
        public void When_I_Call_AddUser_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            var defaultServerPark = "ServerPark1";

            const string role = "King";

            //act
            _sut.AddUser(_connectionModel,_userName, _password, role, serverParkNameList, defaultServerPark);

            //assert
            _userServiceMock.Verify(v => v.AddUser(_connectionModel, _userName, _password, role, serverParkNameList, defaultServerPark), Times.Once);
        }

        [Test]
        public void Given_A_null_ConnectionModel_When_I_Call_AddUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            var defaultServerPark = "ServerPark1";

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(null, _userName, _password, role, serverParkNameList, defaultServerPark));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
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

            var defaultServerPark = "ServerPark1";

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_connectionModel, string.Empty, _password, role, serverParkNameList, defaultServerPark));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_AddUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            var defaultServerPark = "ServerPark1";

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_connectionModel, null, _password, role, serverParkNameList, defaultServerPark));
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

            var defaultServerPark = "ServerPark1";

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_connectionModel, _userName, string.Empty, role, serverParkNameList, defaultServerPark));
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

            var defaultServerPark = "ServerPark1";

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_connectionModel, _userName, null, role, serverParkNameList, defaultServerPark));
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

            var defaultServerPark = "ServerPark1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_connectionModel, _userName, _password, string.Empty, serverParkNameList, defaultServerPark));
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

            var defaultServerPark = "ServerPark1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_connectionModel, _userName, _password, null, serverParkNameList, defaultServerPark));
            Assert.AreEqual("role", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DefaultServerPark_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddUser(_connectionModel, _userName, _password, role, serverParkNameList, string.Empty));
            Assert.AreEqual("A value for the argument 'DefaultServerPark' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_DefaultServerPark_When_I_Call_AddUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";


            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddUser(_connectionModel, _userName, _password, role, serverParkNameList, null));
            Assert.AreEqual("DefaultServerPark", exception.ParamName);
        }

        [Test]
        public void When_I_Call_EditUser_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act
            _sut.EditUser(_connectionModel, _userName, role, serverParkNameList);

            //assert
            _userServiceMock.Verify(v => v.EditUser(_connectionModel, _userName, role, serverParkNameList), Times.Once);
        }


        [Test]
        public void Given_A_null_connectionModel_When_I_Call_EditUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.EditUser(null, _userName, role, serverParkNameList));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_EditUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.EditUser(_connectionModel, string.Empty, role, serverParkNameList));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_EditUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            const string role = "King";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.EditUser(_connectionModel, null, role, serverParkNameList));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Role_When_I_Call_EditUser_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.EditUser(_connectionModel, _userName, string.Empty, serverParkNameList));
            Assert.AreEqual("A value for the argument 'role' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_Role_When_I_Call_EditUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkNameList = new List<string>
            {
                "ServerPark1",
                "ServerPark2",
            };

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.EditUser(_connectionModel, _userName, null, serverParkNameList));
            Assert.AreEqual("role", exception.ParamName);
        }

        [Test]
        public void When_I_Call_ChangePassword_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.ChangePassword(_connectionModel, _userName, _password);

            //assert
            _userServiceMock.Verify(v => v.ChangePassword(_connectionModel, _userName, _password), Times.Once);
        }

        [Test]
        public void Given_A_null_ConnectionModel_When_I_Call_ChangePassword_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ChangePassword(null, _userName, _password));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_ChangePassword_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ChangePassword(_connectionModel, string.Empty, _password));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_ChangePassword_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ChangePassword(_connectionModel, null, _password));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_Password_When_I_Call_ChangePassword_Then_An_ArgumentException_Is_Thrown()
        {
             //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ChangePassword(_connectionModel, _userName, string.Empty));
            Assert.AreEqual("A value for the argument 'password' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_Password_When_I_Call_ChangePassword_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ChangePassword(_connectionModel, _userName, null));
            Assert.AreEqual("password", exception.ParamName);
        }

        [Test]
        public void When_I_Call_UserExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.UserExists(_connectionModel, _userName);

            //assert
            _userServiceMock.Verify(v => v.UserExists(_connectionModel, _userName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void When_I_Call_UserExists_Then_The_Expected_Result_Is_Returned(bool userExists)
        {
            //arrange
            _userServiceMock.Setup(u => u.UserExists(_connectionModel, _userName)).Returns(userExists);

            //act
            var result = _sut.UserExists(_connectionModel, _userName);

            //assert
           Assert.AreEqual(userExists, result);
        }

        [Test]
        public void Given_A_null_ConnectionModel_When_I_Call_UserExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UserExists(null, _userName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }
        [Test]
        public void Given_An_Empty_UserName_When_I_Call_UserExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UserExists(_connectionModel, string.Empty));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_UserExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UserExists(_connectionModel, null));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void When_I_Call_RemoveUser_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.RemoveUser(_connectionModel, _userName);

            //assert
            _userServiceMock.Verify(v => v.RemoveUser(_connectionModel, _userName), Times.Once);
        }

        [Test]
        public void Given_A_null_ConnectionModel_When_I_Call_RemoveUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveUser(null, _userName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_RemoveUser_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.RemoveUser(_connectionModel, string.Empty));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_RemoveUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.RemoveUser(_connectionModel,null));
            Assert.AreEqual("userName", exception.ParamName);
        }

        [Test]
        public void When_I_Call_GetUser_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.GetUser(_connectionModel, _userName);

            //assert
            _userServiceMock.Verify(v => v.GetUser(_connectionModel, _userName), Times.Once);
        }

        [Test]
        public void Given_A_null_ConnectionModel_When_I_Call_GetUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetUser(null, _userName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_UserName_When_I_Call_GetUser_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetUser(_connectionModel, string.Empty));
            Assert.AreEqual("A value for the argument 'userName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_null_UserName_When_I_Call_GetUser_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetUser(_connectionModel, null));
            Assert.AreEqual("userName", exception.ParamName);
        }
    }
}
