using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Tests.Unit.Api
{
    public class FileBackupTests
    {
        private Mock<ICaseService> _caseServiceMock;
        private Mock<IParkService> _parkServiceMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IFileService> _fileServiceMock;
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
        public void Given_Valid_Parameters_When_I_Call_BackupSurvey_The_Correct_Services_Are_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord2>();
            var dataSetMock = new Mock<IDataSet>();

            dataSetMock.Setup(ds => ds.ActiveRecord).Returns(dataRecordMock.Object);
            dataSetMock.Setup(ds => ds.MoveNext());
            dataSetMock.SetupSequence(ds => ds.EndOfSet)
                .Returns(false)
                .Returns(true);

            const string metaFileName = "OPN2004A.bmix";
            const string dataBaseFileName = "OPN2004A.bdix";

            _surveyServiceMock.Setup(s => s.GetMetaFileName(_connectionModel, _instrumentName, _serverParkName)).Returns(metaFileName);

            _fileServiceMock.Setup(f => f.DatabaseFileExists(It.IsAny<string>(), It.IsAny<string>())).Returns(false);
            _fileServiceMock.Setup(f => f.CreateDatabaseFile(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(dataBaseFileName);

            _caseServiceMock.Setup(d => d.GetDataSet(_connectionModel, _instrumentName, _serverParkName))
                .Returns(dataSetMock.Object);

            //act
            _sut.BackupSurveyToFile(_connectionModel, _serverParkName, _instrumentName, _destinationFilePath);

            //assert
            _fileServiceMock.Verify(f => f.DeleteDatabaseFile(_destinationFilePath,
                _instrumentName), Times.Never);

            _fileServiceMock.Verify(f => f.CreateDatabaseFile(metaFileName, _destinationFilePath,
                _instrumentName), Times.Once);

            _caseServiceMock.Verify(ds => ds.WriteDataRecord(dataRecordMock.Object, 
                dataBaseFileName), Times.Once);
        }

        [Test]
        public void Given_Database_Already_Exists_In_DestinationPath_When_I_Call_BackupSurvey_The_Database_Is_Deleted()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord2>();
            var dataSetMock = new Mock<IDataSet>();

            dataSetMock.Setup(ds => ds.ActiveRecord).Returns(dataRecordMock.Object);
            dataSetMock.Setup(ds => ds.MoveNext());
            dataSetMock.SetupSequence(ds => ds.EndOfSet)
                .Returns(false)
                .Returns(true);

            const string metaFileName = "OPN2004A.bmix";
            const string dataBaseFileName = "OPN2004A.bdix";

            _surveyServiceMock.Setup(s => s.GetMetaFileName(_connectionModel, _instrumentName, _serverParkName)).Returns(metaFileName);

            _fileServiceMock.Setup(f => f.DatabaseFileExists(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _fileServiceMock.Setup(f => f.CreateDatabaseFile(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<string>())).Returns(dataBaseFileName);

            _caseServiceMock.Setup(d => d.GetDataSet(_connectionModel,_instrumentName, _serverParkName))
                .Returns(dataSetMock.Object);

            //act
            _sut.BackupSurveyToFile(_connectionModel, _serverParkName, _instrumentName, _destinationFilePath);

            //assert
            _fileServiceMock.Verify(f => f.DeleteDatabaseFile(_destinationFilePath,
                _instrumentName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.BackupSurveyToFile(_connectionModel, string.Empty, 
                _instrumentName, _destinationFilePath));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.BackupSurveyToFile(_connectionModel, null, 
                _instrumentName, _destinationFilePath));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.BackupSurveyToFile(_connectionModel, _serverParkName,
                string.Empty, _destinationFilePath));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.BackupSurveyToFile(_connectionModel, _serverParkName, 
                null, _destinationFilePath));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationFilePath_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.BackupSurveyToFile(_connectionModel, _serverParkName, 
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'destinationFilePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationFilePath_When_I_Call_BackupSurvey_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.BackupSurveyToFile(_connectionModel, _serverParkName, 
                _instrumentName, null));
            Assert.AreEqual("destinationFilePath", exception.ParamName);
        }
    }
}
