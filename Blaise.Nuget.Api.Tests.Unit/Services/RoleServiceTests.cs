using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Models;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class RoleServiceTests
    {
        private Mock<ISecurityManagerFactory> _securityFactoryMock;
        private Mock<IRolePermissionMapper> _mapperMock;

        private Mock<ISecurityServer> _securityServerMock;

        private readonly ConnectionModel _connectionModel;

        private RoleService _sut;

        public RoleServiceTests()
        {
            _connectionModel = new ConnectionModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            _securityServerMock = new Mock<ISecurityServer>();

            _securityFactoryMock = new Mock<ISecurityManagerFactory>();
            _securityFactoryMock.Setup(r => r.GetConnection(_connectionModel))
                .Returns(_securityServerMock.Object);

            _mapperMock = new Mock<IRolePermissionMapper>();

            //setup service under test
            _sut = new RoleService(_securityFactoryMock.Object, _mapperMock.Object);
        }

        [Test]
        public void Given_I_Call_AddRole_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var name = "Admin";
            var description = "Test";
            var permissions = new List<string>();
            var actionPermissions = new List<ActionPermissionModel>();

            var roleId = 1;
            _securityServerMock.Setup(s => s.AddRole(name, description)).Returns(roleId);
            _mapperMock.Setup(m => m.MapToActionPermissionModels(permissions)).Returns(actionPermissions);

            //act
            _sut.AddRole(_connectionModel, name, description, permissions);

            //assert
            _securityServerMock.Verify(v => v.AddRole(name, description));
            _mapperMock.Verify(v => v.MapToActionPermissionModels(permissions));
            _securityServerMock.Verify(v => v.UpdateRolePermissions(roleId, actionPermissions));
        }

        [Test]
        public void Given_I_Call_GetRoles_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var roleMock = new Mock<IRole>();
            var roles = new List<IRole> {roleMock.Object};

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);

            //act
            _sut.GetRoles(_connectionModel);

            //assert
            _securityServerMock.Verify(v => v.GetRoles(), Times.Once());
        }

        [Test]
        public void Given_I_Call_GetRoles_Then_The_Correct_List_Of_Roles_Are_Returned()
        {
            //arrange
            var roleMock = new Mock<IRole>();
            var roles = new List<IRole> { roleMock.Object };

            _securityServerMock.Setup(s => s.GetRoles()).Returns(roles);

            //act
           var result = _sut.GetRoles(_connectionModel);

            //assert
            Assert.AreSame(roles, result);
        }
    }
}
