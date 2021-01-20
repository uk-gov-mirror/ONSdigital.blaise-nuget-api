using System;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Tests.Unit.Api.File
{
    public class BlaiseFileApiTests
    {
        private Mock<ICaseService> _caseServiceMock;
        private Mock<IFileService> _fileServiceMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _destinationFilePath;
        private readonly string _instrumentFile;

        private IBlaiseFileApi _sut;

        public BlaiseFileApiTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "Instrument1";
            _serverParkName = "Park1";
            _destinationFilePath = "FilePath";
            _instrumentFile = "OPN2021a.zip";
        }
        [SetUp]
        public void SetUpTests()
        {
            _caseServiceMock = new Mock<ICaseService>();
            _fileServiceMock = new Mock<IFileService>();

            _sut = new BlaiseFileApi(
                _caseServiceMock.Object,
                _fileServiceMock.Object,
                _connectionModel);
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseFileApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseFileApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseFileApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseFileApi(new ConnectionModel()));
        }

        [Test]
        public void Given_Valid_Parameters_When_I_Call_CreateDatabaseFile_The_Correct_Services_Are_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord2>();
            var dataSetMock = new Mock<IDataSet>();

            dataSetMock.Setup(ds => ds.ActiveRecord).Returns(dataRecordMock.Object);
            dataSetMock.Setup(ds => ds.MoveNext());
            dataSetMock.SetupSequence(ds => ds.EndOfSet)
                .Returns(false)
                .Returns(true);

            const string dataBaseFileName = "OPN2004A.bdix";

            _fileServiceMock.Setup(f => f.DatabaseFileExists(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(false);

            _fileServiceMock.Setup(f => f.GetDatabaseFilePath(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataBaseFileName);

            _caseServiceMock.Setup(d => d.GetDataSet(_connectionModel, _instrumentName, _serverParkName))
                .Returns(dataSetMock.Object);

            //act
            _sut.CreateDatabaseFile(_serverParkName, _instrumentName, _destinationFilePath);

            //assert
            _fileServiceMock.Verify(f => f.DeleteDatabaseFile(_destinationFilePath,
                _instrumentName), Times.Never);

            _fileServiceMock.Verify(f => f.GetDatabaseFilePath(_destinationFilePath,
                _instrumentName), Times.Once);

            _caseServiceMock.Verify(ds => ds.WriteDataRecord(dataRecordMock.Object, 
                dataBaseFileName), Times.Once);
        }

        [Test]
        public void Given_Database_Already_Exists_In_DestinationPath_When_I_Call_CreateDatabaseFile_The_Database_Is_Deleted()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord2>();
            var dataSetMock = new Mock<IDataSet>();

            dataSetMock.Setup(ds => ds.ActiveRecord).Returns(dataRecordMock.Object);
            dataSetMock.Setup(ds => ds.MoveNext());
            dataSetMock.SetupSequence(ds => ds.EndOfSet)
                .Returns(false)
                .Returns(true);

            const string dataBaseFileName = "OPN2004A.bdix";

            _fileServiceMock.Setup(f => f.DatabaseFileExists(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
            _fileServiceMock.Setup(f => f.GetDatabaseFilePath(It.IsAny<string>(),
                It.IsAny<string>())).Returns(dataBaseFileName);

            _caseServiceMock.Setup(d => d.GetDataSet(_connectionModel,_instrumentName, _serverParkName))
                .Returns(dataSetMock.Object);

            //act
            _sut.CreateDatabaseFile(_serverParkName, _instrumentName, _destinationFilePath);

            //assert
            _fileServiceMock.Verify(f => f.DeleteDatabaseFile(_destinationFilePath,
                _instrumentName), Times.Once);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateDatabaseFile_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDatabaseFile(string.Empty, 
                _instrumentName, _destinationFilePath));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CreateDatabaseFile_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDatabaseFile(null, 
                _instrumentName, _destinationFilePath));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CreateDatabaseFile_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDatabaseFile(_serverParkName,
                string.Empty, _destinationFilePath));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_CreateDatabaseFile_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDatabaseFile(_serverParkName, 
                null, _destinationFilePath));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationFilePath_When_I_Call_CreateDatabaseFile_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateDatabaseFile(_serverParkName, 
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'destinationFilePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationFilePath_When_I_Call_CreateDatabaseFile_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateDatabaseFile(_serverParkName, 
                _instrumentName, null));
            Assert.AreEqual("destinationFilePath", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Parameters_When_I_Call_UpdateInstrumentFileWithSqlConnection_The_Correct_Services_Are_Called()
        {
            //arrange
            _fileServiceMock.Setup(f => f.UpdateInstrumentPackageWithSqlConnection(It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.UpdateInstrumentFileWithSqlConnection(_instrumentName, _instrumentFile);

            //assert
            _fileServiceMock.Verify(f => f.UpdateInstrumentPackageWithSqlConnection(_instrumentName,
                _instrumentFile), Times.Once);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateInstrumentFileWithSqlConnection(string.Empty, _instrumentFile));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateInstrumentFileWithSqlConnection(null, _instrumentFile));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentFile_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateInstrumentFileWithSqlConnection(_instrumentName, 
                string.Empty));
            Assert.AreEqual("A value for the argument 'instrumentFile' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentFile_When_I_Call_UpdateInstrumentFileWithSqlConnection_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateInstrumentFileWithSqlConnection(_instrumentName, null));
            Assert.AreEqual("instrumentFile", exception.ParamName);
        }
    }
}
