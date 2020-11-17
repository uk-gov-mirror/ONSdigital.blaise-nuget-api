using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Api.Cati
{
    public class BlaiseCatiApiTests
    {
        private Mock<ISurveyService> _surveyServiceMock;

        private readonly string _serverParkName;
        private readonly string _instrumentName;
        private readonly ConnectionModel _connectionModel;

        private IBlaiseCatiApi _sut;

        public BlaiseCatiApiTests()
        {
            _connectionModel = new ConnectionModel();
            _serverParkName = "Park1";
            _instrumentName = "Instrument1";
        }

        [SetUp]
        public void SetUpTests()
        {
            _surveyServiceMock = new Mock<ISurveyService>();

            _sut = new BlaiseCatiApi(_surveyServiceMock.Object, _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseCatiApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseCatiApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseCatiApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseCatiApi(new ConnectionModel()));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateDayBatchForSurvey_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act
            _sut.CreateDayBatch(_instrumentName, _serverParkName, dayBatchDate);

            //assert
            _surveyServiceMock.Verify(v => v.CreateDayBatch(_connectionModel, _instrumentName, _serverParkName, dayBatchDate), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CreateDayBatchForSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDayBatch(string.Empty, _serverParkName, dayBatchDate));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_CreateDayBatchForSurvey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(null, _serverParkName, dayBatchDate));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateDayBatchForSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDayBatch(_instrumentName, string.Empty, dayBatchDate));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CreateDayBatch_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dayBatchDate = DateTime.Now;

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDayBatch(_instrumentName, null, dayBatchDate));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
    }
}
