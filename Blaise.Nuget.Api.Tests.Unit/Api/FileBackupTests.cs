using System;
using System.IO;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Interfaces;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Api
{
    public class FileBackupTests
    {
        private Mock<IDataService> _dataServiceMock;
        private Mock<IParkService> _parkServiceMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IIocProvider> _unityProviderMock;
        private Mock<IConfigurationProvider> _configurationProviderMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _destinationFilePath;

        private IBlaiseApi _sut;

        public FileBackupTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "Instrument1";
            _serverParkName = "Park1";
            _destinationFilePath = "FilePath";
        }
        [SetUp]
        public void SetUpTests()
        {
            _dataServiceMock = new Mock<IDataService>();
            _parkServiceMock = new Mock<IParkService>();
            _surveyServiceMock = new Mock<ISurveyService>();
            _userServiceMock = new Mock<IUserService>();
            _fileServiceMock = new Mock<IFileService>();
            _unityProviderMock = new Mock<IIocProvider>();
            _configurationProviderMock = new Mock<IConfigurationProvider>();

            _sut = new BlaiseApi(
                _dataServiceMock.Object,
                _parkServiceMock.Object,
                _surveyServiceMock.Object,
                _userServiceMock.Object,
                _fileServiceMock.Object,
                _unityProviderMock.Object,
                _configurationProviderMock.Object);
        }

        [Test]
        public void Given_Valid_Parameters_When_I_Call_BackupSurvey_The_Correct_Services_Are_Called()
        {
            //arrange
            const string dataFileName = "OPN2004A.bdix";
            const string metaFileName = "OPN2004A.bmix";
            var backupFilePath = Path.Combine(_destinationFilePath, _instrumentName);

            _surveyServiceMock.Setup(s => s.GetDataFileName(_connectionModel, _instrumentName, _serverParkName)).Returns(dataFileName);
            _surveyServiceMock.Setup(s => s.GetMetaFileName(_connectionModel, _instrumentName, _serverParkName)).Returns(metaFileName);
            
            _fileServiceMock.Setup(f => f.BackupDatabaseFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.BackupSurvey(_connectionModel, _serverParkName, _instrumentName, _destinationFilePath);

            //assert
            _fileServiceMock.Verify(v => v.BackupDatabaseFile(dataFileName, metaFileName, backupFilePath));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.BackupSurvey(_connectionModel, string.Empty, 
                _instrumentName, _destinationFilePath));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.BackupSurvey(_connectionModel, null, 
                _instrumentName, _destinationFilePath));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.BackupSurvey(_connectionModel, _serverParkName,
                string.Empty, _destinationFilePath));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.BackupSurvey(_connectionModel, _serverParkName, 
                null, _destinationFilePath));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationFilePath_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.BackupSurvey(_connectionModel, _serverParkName, 
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'destinationFilePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationFilePath_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.BackupSurvey(_connectionModel, _serverParkName, 
                _instrumentName, null));
            Assert.AreEqual("destinationFilePath", exception.ParamName);
        }
    }
}
