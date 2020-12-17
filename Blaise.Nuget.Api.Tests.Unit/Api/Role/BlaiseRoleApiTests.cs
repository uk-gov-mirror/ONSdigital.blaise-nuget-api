using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Tests.Unit.Api.Role
{
    public class BlaiseRoleApiTests
    {
        private Mock<IRoleService> _roleServiceMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _name;
        private readonly string _description;
        private readonly List<string> _permissions;
        
        private IBlaiseRoleApi _sut;

        public BlaiseRoleApiTests()
        {
            _connectionModel = new ConnectionModel();
            _name = "Admin";
            _description = "Test";
            _permissions = new List<string> {"Permission1"};
        }

        [SetUp]
        public void SetUpTests()
        {
            _roleServiceMock = new Mock<IRoleService>();

            _sut = new BlaiseRoleApi(_roleServiceMock.Object, _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseRoleApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseRoleApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseRoleApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseRoleApi(new ConnectionModel()));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_AddRole_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.AddRole(_name, _description, _permissions);

            //assert
            _roleServiceMock.Verify(v => v.AddRole(_connectionModel, _name, _description, _permissions), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_AddRole_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddRole(string.Empty, _description, _permissions));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_AddRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddRole(null, _description, _permissions));
            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_AddRole_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddRole(_name, string.Empty, _permissions));
            Assert.AreEqual("A value for the argument 'description' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_AddRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddRole(_name, null, _permissions));
            Assert.AreEqual("description", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetRoles_Then_The_Correct_Service_Method_Is_Called()
        {
            //act
            _sut.GetRoles();

            //assert
            _roleServiceMock.Verify(v => v.GetRoles(_connectionModel), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetRoles_Then_The_Correct_List_Of_Roles_Are_Returned()
        {
            //arrange
            var roleMock = new Mock<IRole>();
            var roles = new List<IRole> { roleMock.Object };

            _roleServiceMock.Setup(r => r.GetRoles(_connectionModel)).Returns(roles);

            //act
            var result = _sut.GetRoles();

            //assert
            Assert.AreSame(roles, result);
        }
    }
}
