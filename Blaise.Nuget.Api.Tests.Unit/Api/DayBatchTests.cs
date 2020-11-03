using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Api
{
    public class DayBatchTests
    {
        private Mock<ICaseService> _caseServiceMock;
        private Mock<IParkService> _parkServiceMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IConfigurationProvider> _configurationProviderMock;

        private readonly string _serverParkName;
        private readonly string _instrumentName;
        private readonly ConnectionModel _connectionModel;

        private IBlaiseApi _sut;

        public DayBatchTests()
        {
            _connectionModel = new ConnectionModel();
            _serverParkName = "Park1";
            _instrumentName = "Instrument1";
        }

        [SetUp]
        public void SetUpTests()
        {
            _caseServiceMock = new Mock<ICaseService>();
            _parkServiceMock = new Mock<IParkService>();
            _surveyServiceMock = new Mock<ISurveyService>();
            _userServiceMock = new Mock<IUserService>();
            _fileServiceMock = new Mock<IFileService>();
            _configurationProviderMock = new Mock<IConfigurationProvider>();

            _sut = new BlaiseApi(
                _caseServiceMock.Object,
                _parkServiceMock.Object,
                _surveyServiceMock.Object,
                _userServiceMock.Object,
                _fileServiceMock.Object,
                _configurationProviderMock.Object);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateDayBatch_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act
            _sut.CreateDayBatch(_connectionModel, _instrumentName, _serverParkName, dayBatchDate);

            //assert
            _surveyServiceMock.Verify(v => v.CreateDayBatch(_connectionModel, _instrumentName, _serverParkName, dayBatchDate), Times.Once);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_CreateDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(null, _instrumentName, _serverParkName, dayBatchDate));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CreateDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDayBatch(_connectionModel, string.Empty, _serverParkName, dayBatchDate));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_CreateDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(_connectionModel, null, _serverParkName, dayBatchDate));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateDayBatch_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDayBatch(_connectionModel, _instrumentName, string.Empty, dayBatchDate));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CreateDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(_connectionModel, _instrumentName, null, dayBatchDate));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
    }
}
