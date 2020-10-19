using System;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.Cati.Runtime;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class DayBatchServiceTests
    {
        private Mock<IRemoteCatiServerProvider> _remoteCatiProviderMock;
        private Mock<IRemoteCatiManagementServer> _catiManagementServerMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;

        private DayBatchService _sut;

        public DayBatchServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
        }

        [SetUp]
        public void SetUpTests()
        {
            _catiManagementServerMock = new Mock<IRemoteCatiManagementServer>();
            _catiManagementServerMock.Setup(c => c.SelectServerPark(It.IsAny<string>()));
            _catiManagementServerMock.Setup(c => c.LoadCatiInstrumentManager(It.IsAny<string>()).CreateDaybatch(It.IsAny<DateTime>()));

            _remoteCatiProviderMock = new Mock<IRemoteCatiServerProvider>();
            _remoteCatiProviderMock.Setup(r => r.GetRemoteConnection(_connectionModel))
                .Returns(_catiManagementServerMock.Object);

            _sut = new DayBatchService(_remoteCatiProviderMock.Object);
        }

        [Test]
        public void Given_I_Call_CreateDayBatch_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act
            _sut.CreateDayBatch(_connectionModel, _instrumentName, _serverParkName, dayBatchDate);

            //assert
            _remoteCatiProviderMock.Verify(v => v.GetRemoteConnection(_connectionModel), Times.Once);
            _catiManagementServerMock.Verify(v => v.SelectServerPark(_serverParkName), Times.Once);
            _catiManagementServerMock.Verify(v => v
                .LoadCatiInstrumentManager(_instrumentName)
                .CreateDaybatch(dayBatchDate), Times.Once);
        }
    }
}
