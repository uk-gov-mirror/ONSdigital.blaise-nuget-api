using Blaise.Nuget.Api.Core.Interfaces;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class ParkServiceTests
    {
        private Mock<IConnectedServerFactory> _connectionFactoryMock;

        private Mock<ISurvey> _surveyMock;
        private Mock<ISurveyCollection> _surveyCollectionMock;
        private Mock<IServerPark> _serverParkMock;
        private Mock<IConnectedServer> _connectedServerMock;
        private Mock<IServerParkCollection> _serverParkCollectionMock;

        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly Guid _instrumentId;

        private ParkService _sut;

        public ParkServiceTests()
        {
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _instrumentId = Guid.NewGuid();
        }

        [SetUp]
        public void SetUpTests()
        {
            //setup surveys
            _surveyMock = new Mock<ISurvey>();
            _surveyMock.Setup(s => s.Name).Returns(_instrumentName);
            _surveyMock.Setup(s => s.InstrumentID).Returns(_instrumentId);

            var surveyItems = new List<ISurvey> { _surveyMock.Object };

            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());

            //setup server parks
            _serverParkMock = new Mock<IServerPark>();
            _serverParkMock.Setup(s => s.Name).Returns("TestServerParkName");
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);

            var serverParkItems = new List<IServerPark> { _serverParkMock.Object };

            _serverParkCollectionMock = new Mock<IServerParkCollection>();
            _serverParkCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => serverParkItems.GetEnumerator());

            //setup connection
            _connectedServerMock = new Mock<IConnectedServer>();
            _connectedServerMock.Setup(c => c.ServerParks).Returns(_serverParkCollectionMock.Object);
            _connectedServerMock.Setup(c => c.GetServerPark(_serverParkName)).Returns(_serverParkMock.Object);

            _connectionFactoryMock = new Mock<IConnectedServerFactory>();
            _connectionFactoryMock.Setup(c => c.GetConnection()).Returns(_connectedServerMock.Object);

            //setup service under test
            _sut = new ParkService(_connectionFactoryMock.Object);
        }

        [Test]
        public void Given_I_Call_GetServerParkNames_Then_I_Get_An_IEnumerable_Of_Strings_Returned()
        {
            //act
            var result = _sut.GetServerParkNames();

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
        }

        [Test]
        public void Given_I_Call_GetServerParkNames_Then_I_Get_A_Correct_List_Of_ServerParkNames_Returned()
        {
            //arrange
            var serverParkMock1 = new Mock<IServerPark>();
            var serverParkMock2 = new Mock<IServerPark>();

            serverParkMock1.Setup(s => s.Name).Returns("ServerParkName1");
            serverParkMock2.Setup(s => s.Name).Returns("ServerParkName2");

            var serverParkItems = new List<IServerPark> { serverParkMock1.Object, serverParkMock2.Object };

            _serverParkCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => serverParkItems.GetEnumerator());

            //act
            var result = _sut.GetServerParkNames();

            //assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count());
            Assert.True(result.Contains("ServerParkName1"));
            Assert.True(result.Contains("ServerParkName2"));
        }

        [Test]
        public void Given_I_Call_GetInstrumentId_Then_I_Get_A_Guid_Returned()
        {
            //act
            var result = _sut.GetInstrumentId(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Guid>(result);
        }

        [Test]
        public void Given_I_Call_GetInstrumentId_Then_I_Get_The_Correct_InstrumentId_Returned()
        {
            //act
            var result = _sut.GetInstrumentId(_instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(_instrumentId, result);
        }

        [Test]
        public void Given_I_Call_GetInstrumentId_And_The_Instrument_Doesnt_Exist_Then_An_ArgumentOutOfRangeException_Is_Thrown()
        {
            //arrange
            var instrumentName = "InstrumentThatDoesntExist";

            //act && assert
           Assert.Throws<ArgumentOutOfRangeException>(() => _sut.GetInstrumentId(instrumentName, _serverParkName));
        }
    }
}
