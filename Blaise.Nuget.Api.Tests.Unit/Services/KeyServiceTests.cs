using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using Blaise.Nuget.Api.Contracts.Models;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class KeyServiceTests
    {
        private Mock<IRemoteDataLinkProvider> _remoteDataLinkProviderMock;

        private Mock<IRemoteDataServer> _remoteDataServerMock;
        private Mock<IDataLink4> _remoteDataLinkMock;
        private Mock<IDatamodel> _dataModelMock;
        private Mock<IKey> _keyMock;
        private Mock<IDataRecord> _dataRecordMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly Guid _instrumentId;

        private KeyService _sut;

        public KeyServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _instrumentId = Guid.NewGuid();
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();
            _keyMock = new Mock<IKey>();
            _dataRecordMock = new Mock<IDataRecord>();

            _remoteDataLinkMock = new Mock<IDataLink4>();
            _remoteDataLinkMock.Setup(d => d.Datamodel).Returns(_dataModelMock.Object);

            _remoteDataServerMock = new Mock<IRemoteDataServer>();
            _remoteDataServerMock.Setup(r => r.GetDataLink(_instrumentId, _serverParkName))
                .Returns(_remoteDataLinkMock.Object);

            _remoteDataLinkProviderMock = new Mock<IRemoteDataLinkProvider>();
            _remoteDataLinkProviderMock.Setup(r => r.GetDataLink(_connectionModel, _instrumentName, _serverParkName))
                .Returns(_remoteDataLinkMock.Object);

            _sut = new KeyService(_remoteDataLinkProviderMock.Object);
        }

        [Test]
        public void Given_I_Call_KeyExists_I_Get_A_Boolean_Back()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.KeyExists(_keyMock.Object)).Returns(It.IsAny<bool>());

            //act
            var result = _sut.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<bool>(result);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_I_Call_KeyExists_I_Get_The_Correct_Boolean_Back(bool keyExists)
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.KeyExists(_keyMock.Object)).Returns(keyExists);

            //act
            var result = _sut.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(keyExists, result);
        }

        [Test]
        public void Given_I_Call_KeyExists_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.KeyExists(_keyMock.Object)).Returns(It.IsAny<bool>());

            //act
            _sut.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_connectionModel, _instrumentName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.KeyExists(_keyMock.Object), Times.Once);
        }

        [TestCase("Key1", "Key1")]
        [TestCase(" Key1 ", "Key1")]
        [TestCase("Key1 ", "Key1")]
        [TestCase(" Key1", "Key1")]
        public void Given_I_Call_GetPrimaryKeyValue_I_Get_The_Correct_Value_Back(string primaryKeyValue, string expectedValue)
        {
            //arrange
            _dataRecordMock.Setup(d => d.Keys[0].KeyValue).Returns(primaryKeyValue);

            //act
            var result = _sut.GetPrimaryKeyValue(_dataRecordMock.Object);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(expectedValue, result);
        }
    }
}
