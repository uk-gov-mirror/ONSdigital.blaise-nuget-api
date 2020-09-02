using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class BackupTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _destinationFilePath;
        private readonly string _bucketName;
        private readonly string _folderPath;

        private FluentBlaiseApi _sut;

        public BackupTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "Instrument1";
            _serverParkName = "Park1";
            _destinationFilePath = "FilePath";
            _bucketName = "OpnBucket";
            _folderPath = "FolderPath";
        }

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();

            _sut = new FluentBlaiseApi(_blaiseApiMock.Object);
        }

        [Test]
        public void Given_ToPath_Steps_Are_Setup_When_I_Call_Backup_The_Correct_Services_Are_Called()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);

            //act
            _sut.Backup();

            //assert
            _blaiseApiMock.Verify(v => v.BackupSurveyToFile(It.IsAny<ConnectionModel>(), _serverParkName, _instrumentName, _destinationFilePath), Times.Once);
        }

        [Test]
        public void Given_ToPath_Steps_Are_Setup_But_WithConnection_Has_Not_Been_Called_When_I_Call_Backup_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_ToPath_Steps_Are_Setup_But_WithInstrument_Has_Not_Been_Called_When_I_Call_Backup_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            //_sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithInstrument' step needs to be called with a valid value, check that the step has been called with a valid instrument", exception.Message);
        }

        [Test]
        public void Given_ToPath_Steps_Are_Setup_But_WithServerPark_Has_Not_Been_Called_When_I_Call_Backup_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            //_sut.WithServerPark(_serverParkName);
            _sut.Survey.ToPath(_destinationFilePath);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithServerPark' step needs to be called with a valid value, check that the step has been called with a valid value for the server park", exception.Message);
        }

        [Test]
        public void Given_ToPath_Steps_Are_Setup_But_WithFile_Has_Not_Been_Called_When_I_Call_Backup_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            //_sut.Survey.ToPath(_destinationFilePath);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'ToPath' step needs to be called with a valid value, check that the step has been called with a valid value for the destination file path", exception.Message);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_When_I_Call_Backup_The_Correct_Services_Are_Called()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToBucket(_bucketName);

            //act
            _sut.Backup();

            //assert
            _blaiseApiMock.Verify(v => v.BackupSurveyToBucket(It.IsAny<ConnectionModel>(), _serverParkName, _instrumentName, 
                    _bucketName, null), Times.Once);
        }

        [Test]
        public void Given_ToBucket_AndToFolder_Steps_Are_Setup_When_I_Call_Backup_The_Correct_Services_Are_Called()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToBucket(_bucketName);
            _sut.ToPath(_folderPath);

            //act
            _sut.Backup();

            //assert
            _blaiseApiMock.Verify(v => v.BackupSurveyToBucket(It.IsAny<ConnectionModel>(), _serverParkName, _instrumentName, _bucketName,
                    _folderPath), Times.Once);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_But_WithConnection_Has_Not_Been_Called_When_I_Call_Backup_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToBucket(_bucketName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid model, check that the step has been called with a valid model containing the connection properties of the blaise server you wish to connect to", exception.Message);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_But_WithInstrument_Has_Not_Been_Called_When_I_Call_Backup_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            //_sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Survey.ToBucket(_bucketName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithInstrument' step needs to be called with a valid value, check that the step has been called with a valid instrument", exception.Message);
        }

        [Test]
        public void Given_ToBucket_Steps_Are_Setup_But_WithServerPark_Has_Not_Been_Called_When_I_Call_Backup_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            //_sut.WithServerPark(_serverParkName);
            _sut.Survey.ToBucket(_bucketName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Backup();
            });

            Assert.AreEqual("The 'WithServerPark' step needs to be called with a valid value, check that the step has been called with a valid value for the server park", exception.Message);
        }
    }
}
