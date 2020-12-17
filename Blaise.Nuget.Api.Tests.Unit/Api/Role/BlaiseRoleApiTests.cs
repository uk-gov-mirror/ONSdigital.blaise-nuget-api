using System;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Api.Role
{
    public class BlaiseRoleApiTests
    {
        private Mock<IRoleService> _roleServiceMock;

        private readonly string _name;
        private readonly string _description;
        private readonly ConnectionModel _connectionModel;

        private IBlaiseRoleApi _sut;

        public BlaiseRoleApiTests()
        {
            _connectionModel = new ConnectionModel();
            _name = "Admin";
            _description = "Test";
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
            _sut.AddRole(_name, _description);

            //assert
            _roleServiceMock.Verify(v => v.AddRole(_connectionModel, _name, _description), Times.Once);
        }

        [Test]
        public void Given_An_Empty_Name_When_I_Call_AddRole_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddRole(string.Empty, _description));
            Assert.AreEqual("A value for the argument 'name' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_Name_When_I_Call_AddRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddRole(null, _description));
            Assert.AreEqual("name", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_AddRole_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AddRole(_name, string.Empty));
            Assert.AreEqual("A value for the argument 'description' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_AddRole_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AddRole(_name, null));
            Assert.AreEqual("description", exception.ParamName);
        }
    }
}
