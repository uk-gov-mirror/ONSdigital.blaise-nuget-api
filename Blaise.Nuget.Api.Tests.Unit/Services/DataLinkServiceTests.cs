using Blaise.Nuget.Api.Core.Interfaces;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.Meta;
using System;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class DataLinkServiceTests
    {
        private Mock<IRemoteDataServerFactory> _connectionFactoryMock;
        private Mock<IParkService> _parkServiceMock;

        private Mock<IRemoteDataServer> _remoteDataServerMock;
        private Mock<IDataLink4> _dataLinkMock;
        private Mock<IDatamodel> _dataModelMock;

        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly Guid _instrumentId;

        private DataLinkService _sut;

        public DataLinkServiceTests()
        {
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _instrumentId = Guid.NewGuid();
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();

            _dataLinkMock = new Mock<IDataLink4>();
            _dataLinkMock.Setup(d => d.Datamodel).Returns(_dataModelMock.Object);

            _remoteDataServerMock = new Mock<IRemoteDataServer>();
            _remoteDataServerMock.Setup(r => r.GetDataLink(_instrumentId, _serverParkName)).Returns(_dataLinkMock.Object);

            _connectionFactoryMock = new Mock<IRemoteDataServerFactory>();
            _connectionFactoryMock.Setup(c => c.GetConnection()).Returns(_remoteDataServerMock.Object);

            _parkServiceMock = new Mock<IParkService>();
            _parkServiceMock.Setup(p => p.GetInstrumentId(_instrumentName, _serverParkName)).Returns(_instrumentId);

            _sut = new DataLinkService(
                _connectionFactoryMock.Object,
                _parkServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_A_Method_With_The_Same_InstrumentName_And_ServerName_More_Than_Once_Then_The_Same_DataLink_Is_Used()
        {
            //arrange
            _parkServiceMock.Setup(p => p.GetInstrumentId(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            //act
            _sut.GetDataModel(_instrumentName, _serverParkName);
            _sut.GetDataModel(_instrumentName, _serverParkName);

            //assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Given_I_Call_A_Method_With_A_Different_InstrumentName_More_Than_Once_Then_A_New_DataLink_Is_Established()
        {
            //arrange
            _parkServiceMock.Setup(p => p.GetInstrumentId(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            //act
            _sut.GetDataModel(_instrumentName, _serverParkName);
            _sut.GetDataModel("NewInstrumentName", _serverParkName);

            //assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Given_I_Call_A_Method_With_A_Different_ServerParkName_More_Than_Once_Then_A_New_DataLink_Is_Established()
        {
            //arrange
            _parkServiceMock.Setup(p => p.GetInstrumentId(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            //act
            _sut.GetDataModel(_instrumentName, _serverParkName);
            _sut.GetDataModel(_instrumentName, "NewServerParkName");

            //assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
        }

        [Test]
        public void Given_I_Call_A_Method_With_A_Different_InstrumentName_And_ServerParkName_More_Than_Once_Then_A_New_DataLink_Is_Established()
        {
            //arrange
            _parkServiceMock.Setup(p => p.GetInstrumentId(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());
            _remoteDataServerMock.Setup(r => r.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>())).Returns(_dataLinkMock.Object);

            //act
            _sut.GetDataModel(_instrumentName, _serverParkName);
            _sut.GetDataModel("NewInstrumentName", "NewServerParkName");

            //assert
            _remoteDataServerMock.Verify(v => v.GetDataLink(It.IsAny<Guid>(), It.IsAny<string>()), Times.Exactly(2));
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
            _parkServiceMock.Verify(v => v.GetInstrumentId(_instrumentName, _serverParkName), Times.Once);
            _remoteDataServerMock.Verify(v => v.GetDataLink(_instrumentId, _serverParkName), Times.Once);
            _dataLinkMock.Verify(v => v.Datamodel, Times.Once);
        }
    }
}
