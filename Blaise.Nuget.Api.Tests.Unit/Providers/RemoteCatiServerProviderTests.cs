using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Factories;
using Blaise.Nuget.Api.Core.Providers;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Cati.Runtime;

namespace Blaise.Nuget.Api.Tests.Unit.Providers
{
    public class RemoteCatiServerProviderTests
    {
        private Mock<IRemoteCatiServerFactory> _connectionFactoryMock;
        private Mock<IRemoteCatiManagementServer> _remoteDataServerMock;

        private readonly ConnectionModel _connectionModel;

        private RemoteCatiServerProvider _sut;

        public RemoteCatiServerProviderTests()
        {
            _connectionModel = new ConnectionModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            _remoteDataServerMock = new Mock<IRemoteCatiManagementServer>();

            _connectionFactoryMock = new Mock<IRemoteCatiServerFactory>();
            _connectionFactoryMock.Setup(c => c.GetConnection(_connectionModel)).Returns(_remoteDataServerMock.Object);


            _sut = new RemoteCatiServerProvider(_connectionFactoryMock.Object);
        }

        [Test]
        public void Given_I_Call_GetRemoteConnection_Then_A_Remote_Connection_Is_Returned()
        {
            //arrange && act
            var result = _sut.GetRemoteConnection(_connectionModel);

            //assert
           Assert.IsNotNull(result);
           Assert.IsInstanceOf<IRemoteCatiManagementServer>(result);
        }

        [Test]
        public void Given_I_Call_GetRemoteConnection_Then_The_Correct_Services_Are_Called()
        {
            //arrange && act
            _sut.GetRemoteConnection(_connectionModel);

            //assert
            _connectionFactoryMock.Verify(v => v.GetConnection(_connectionModel), Times.Once);
        }
    }
}
