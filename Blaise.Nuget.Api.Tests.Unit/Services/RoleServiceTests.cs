using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Security;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class RoleServiceTests
    {
        private Mock<ISecurityManagerFactory> _securityFactoryMock;
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

            //setup service under test
            _sut = new RoleService(_securityFactoryMock.Object);
        }

        [Test]
        public void Given_I_Call_AddRole_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var name = "Admin";
            var description = "Test";

            //act
            _sut.AddRole(_connectionModel, name, description);

            //assert
            _securityServerMock.Verify(v => v.AddRole(name, description));
        }
    }
}
