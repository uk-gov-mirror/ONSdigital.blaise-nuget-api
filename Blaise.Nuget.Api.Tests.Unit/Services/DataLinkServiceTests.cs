using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class DataLinkServiceTests
    {
        private Mock<IRemoteDataLinkProvider> _remoteDataLinkProviderMock;
        private Mock<ILocalDataLinkProvider> _localDataLinkProviderMock;

        private Mock<IRemoteDataServer> _remoteDataServerMock;
        private Mock<IDataLink4> _remoteDataLinkMock;
        private Mock<IDataLink> _localDataLinkMock;
        private Mock<IDatamodel> _dataModelMock;
        private Mock<IKey> _keyMock;
        private Mock<IDataSet> _dataSetMock;
        private Mock<IDataRecord> _dataRecordMock;        

        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _filePath;
        private readonly Guid _instrumentId;

        private DataLinkService _sut;

        public DataLinkServiceTests()
        {
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _filePath = "c:\\filePath";
            _instrumentId = Guid.NewGuid();
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();
            _keyMock = new Mock<IKey>();
            _dataSetMock = new Mock<IDataSet>();
            _dataRecordMock = new Mock<IDataRecord>();

            _remoteDataLinkMock = new Mock<IDataLink4>();
            _remoteDataLinkMock.Setup(d => d.Datamodel).Returns(_dataModelMock.Object);

            _localDataLinkMock = new Mock<IDataLink>();

            _remoteDataServerMock = new Mock<IRemoteDataServer>();
            _remoteDataServerMock.Setup(r => r.GetDataLink(_instrumentId, _serverParkName)).Returns(_remoteDataLinkMock.Object);

            _remoteDataLinkProviderMock = new Mock<IRemoteDataLinkProvider>();
            _remoteDataLinkProviderMock.Setup(r => r.GetDataLink(_instrumentName, _serverParkName)).Returns(_remoteDataLinkMock.Object);

            _localDataLinkProviderMock = new Mock<ILocalDataLinkProvider>();
            _localDataLinkProviderMock.Setup(r => r.GetDataLink(_filePath)).Returns(_localDataLinkMock.Object);

            _sut = new DataLinkService(
                _remoteDataLinkProviderMock.Object,
                _localDataLinkProviderMock.Object);
        }


        [Test]
        public void Given_I_Call_GetDataModel_I_Get_A_DataModel_Back()
        {
            //act
            var result = _sut.GetDataModel(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDatamodel>(result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_I_Get_The_Correct_DataModel_Back()
        {
            //act
            var result = _sut.GetDataModel(_instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(_dataModelMock.Object, result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_Then_The_Correct_Services_Are_Called()
        {
            //act
             _sut.GetDataModel(_instrumentName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_instrumentName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.Datamodel, Times.Once);
        }

        [Test]
        public void Given_I_Call_KeyExists_I_Get_A_Boolean_Back()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.KeyExists(_keyMock.Object)).Returns(It.IsAny<bool>());

            //act
            var result = _sut.KeyExists(_keyMock.Object, _instrumentName, _serverParkName);

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
            var result = _sut.KeyExists(_keyMock.Object, _instrumentName, _serverParkName);

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
            _sut.KeyExists(_keyMock.Object, _instrumentName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_instrumentName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.KeyExists(_keyMock.Object), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetDataSet_I_Get_A_DataSet_Back()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.Read(It.IsAny<string>())).Returns(_dataSetMock.Object);

            //act
            var result = _sut.GetDataSet(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDataSet>(result);
        }

        [Test]
        public void Given_I_Call_GetDataSet_I_Get_The_Correct_DataSet_Back()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.Read(It.IsAny<string>())).Returns(_dataSetMock.Object);

            //act
            var result = _sut.GetDataSet(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreSame(_dataSetMock.Object, result);
        }

        [Test]
        public void Given_I_Call_GetDataSet_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.Read(It.IsAny<string>())).Returns(It.IsAny<IDataSet>());

            //act
            _sut.GetDataSet(_instrumentName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_instrumentName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.Read(null), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetDataRecord_I_Get_A_DataRecord_Back()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.ReadRecord(It.IsAny<IKey>())).Returns(_dataRecordMock.Object);

            //act
            var result = _sut.GetDataRecord(_keyMock.Object, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDataRecord>(result);
        }

        [Test]
        public void Given_An_InstrumentName_And_ServerParkName_When_I_Call_GetDataRecord_I_Get_The_Correct_DataRecord_Back()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.ReadRecord(It.IsAny<IKey>())).Returns(_dataRecordMock.Object);

            //act
            var result = _sut.GetDataRecord(_keyMock.Object, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreSame(_dataRecordMock.Object, result);
        }

        [Test]
        public void Given_An_InstrumentName_And_ServerParkName_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.ReadRecord(It.IsAny<IKey>())).Returns(It.IsAny<IDataRecord>());

            //act
            _sut.GetDataRecord(_keyMock.Object, _instrumentName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_instrumentName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.ReadRecord(_keyMock.Object), Times.Once);
        }

        [Test]
        public void Given_A_FilePath_When_I_Call_GetDataRecord_I_Get_The_Correct_DataRecord_Back()
        {
            //arrange
            _localDataLinkMock.Setup(d => d.ReadRecord(It.IsAny<IKey>())).Returns(_dataRecordMock.Object);

            //act
            var result = _sut.GetDataRecord(_keyMock.Object, _filePath);

            //assert
            Assert.NotNull(result);
            Assert.AreSame(_dataRecordMock.Object, result);
        }

        [Test]
        public void Given_A_FilePath_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _localDataLinkMock.Setup(d => d.ReadRecord(It.IsAny<IKey>())).Returns(It.IsAny<IDataRecord>());

            //act
            _sut.GetDataRecord(_keyMock.Object, _filePath);

            //assert
            _localDataLinkProviderMock.Verify(v => v.GetDataLink(_filePath), Times.Once);
            _localDataLinkMock.Verify(v => v.ReadRecord(_keyMock.Object), Times.Once);
        }

        [Test]
        public void Given_An_InstrumentName_And_ServerParkName_When_I_Call_WriteDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _remoteDataLinkMock.Setup(d => d.Write(It.IsAny<IDataRecord>()));

            //act
            _sut.WriteDataRecord(_dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_instrumentName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.Write(_dataRecordMock.Object), Times.Once);
        }

        [Test]
        public void Given_A_FilePath_When_I_Call_WriteDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _localDataLinkMock.Setup(d => d.Write(It.IsAny<IDataRecord>()));

            //act
            _sut.WriteDataRecord(_dataRecordMock.Object, _filePath);

            //assert
            _localDataLinkProviderMock.Verify(v => v.GetDataLink(_filePath), Times.Once);
            _localDataLinkMock.Verify(v => v.Write(_dataRecordMock.Object), Times.Once);
        }
    }
}
