﻿using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Tests.Unit.Api
{
    public class FileHandlerTests
    {
        private Mock<IDataService> _dataServiceMock;
        private Mock<IParkService> _parkServiceMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IIocProvider> _unityProviderMock;
        private Mock<IConfigurationProvider> _configurationProviderMock;

        private readonly string _sourceServerName;
        private readonly string _sourceInstrumentName;
        private readonly string _sourceServerParkName;
        private readonly string _primaryKeyValue;
        private readonly string _destinationFilePath;
        private readonly string _destinationInstrumentName;

        private readonly ConnectionModel _sourceConnectionModel;

        private IBlaiseApi _sut;

        public FileHandlerTests()
        {
            _sourceServerName = "Server1";
            _sourceInstrumentName = "Instrument1";
            _sourceServerParkName = "Park1";
            _primaryKeyValue = "Key1";

            _destinationFilePath = "FilePath";
            _destinationInstrumentName = "Instrument2";

            _sourceConnectionModel = new ConnectionModel
            {
                ServerName = _sourceServerName
            };
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
            _configurationProviderMock.Setup(c => c.GetConnectionModel())
                .Returns(_sourceConnectionModel);

            _unityProviderMock.Setup(u => u.Resolve<IDataService>()).Returns(_dataServiceMock.Object);
            _unityProviderMock.Setup(u => u.Resolve<IFileService>()).Returns(_fileServiceMock.Object);
            _unityProviderMock.Setup(u => u.Resolve<ISurveyService>()).Returns(_surveyServiceMock.Object);

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
        public void Given_File_Exists_When_I_Call_CopyCase_The_Correct_Services_Are_Called()
        {
            //arrange
            var databaseFilePath = "FilePath";

            var dataRecordMock = new Mock<IDataRecord>();
            _dataServiceMock.Setup(d => d.GetDataRecord(It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(dataRecordMock.Object);

            _fileServiceMock.Setup(f => f.DatabaseFileExists(_destinationFilePath, _destinationInstrumentName))
                .Returns(true);

            _fileServiceMock.Setup(f => f.GetDatabaseFileName(_destinationFilePath, _destinationInstrumentName))
                .Returns(databaseFilePath);

            //act
            _sut.CopyCase(_sourceConnectionModel,_primaryKeyValue, _sourceInstrumentName, _sourceServerParkName, _destinationFilePath, 
                _destinationInstrumentName);

            //assert
            _fileServiceMock.Verify(v => v.CreateDatabaseFile(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
            _dataServiceMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, databaseFilePath), Times.Once);
        }

        [Test]
        public void Given_File_Does_Not_Exist_When_I_Call_CopyCase_The_Correct_Services_Are_Called()
        {
            //arrange
            var databaseFilePath = "FilePath";
            var metaFileName = "MetaFileName";

            var dataRecordMock = new Mock<IDataRecord>();
            _dataServiceMock.Setup(d => d.GetDataRecord(It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(dataRecordMock.Object);

            _surveyServiceMock.Setup(s => s.GetMetaFileName(It.IsAny<ConnectionModel>(), _sourceInstrumentName, _sourceServerParkName))
                .Returns(metaFileName);

            _fileServiceMock.Setup(f => f.DatabaseFileExists(_destinationFilePath, _destinationInstrumentName))
                .Returns(false);

            _fileServiceMock.Setup(f => f.GetDatabaseFileName(_destinationFilePath, _destinationInstrumentName))
                .Returns(databaseFilePath);

            //act
            _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName, _destinationFilePath,
                _destinationInstrumentName);

            //assert
            Assert.AreEqual(_sourceServerName, _sourceConnectionModel.ServerName);
            _fileServiceMock.Verify(v => v.CreateDatabaseFile(metaFileName, databaseFilePath, _destinationInstrumentName), Times.Once);
            _dataServiceMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, databaseFilePath), Times.Once);
        }

        [Test]
        public void Given_A_Null_SourceConnectionModel_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(null, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("The argument 'sourceConnectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_ForFile_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CopyCase(_sourceConnectionModel, string.Empty, _sourceInstrumentName, _sourceServerParkName,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_ForFile_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(_sourceConnectionModel, null, _sourceInstrumentName, _sourceServerParkName,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_SourceInstrumentName_ForFile_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, string.Empty, _sourceServerParkName,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("A value for the argument 'sourceInstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_SourceInstrumentName_ForFile_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, null, _sourceServerParkName,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("sourceInstrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_SourceServerParkName_ForFile_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, string.Empty,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("A value for the argument 'sourceServerParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_SourceServerParkName_ForFile__When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, null,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("sourceServerParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationFilePath_ForFile_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                string.Empty, _destinationInstrumentName));
            Assert.AreEqual("A value for the argument 'destinationFilePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationFilePath_ForFile__When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                null, _destinationInstrumentName));
            Assert.AreEqual("destinationFilePath", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationInstrumentName_ForFile_When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationFilePath, string.Empty));
            Assert.AreEqual("A value for the argument 'destinationInstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationInstrumentName_ForFile__When_I_Call_CopyCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CopyCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationFilePath, null));
            Assert.AreEqual("destinationInstrumentName", exception.ParamName);
        }

        [Test]
        public void Given_File_Exists_When_I_Call_MoveCase_The_Correct_Services_Are_Called()
        {
            var databaseFilePath = "FilePath";

            var dataRecordMock = new Mock<IDataRecord>();
            _dataServiceMock.Setup(d => d.GetDataRecord(It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(dataRecordMock.Object);

            _fileServiceMock.Setup(f => f.DatabaseFileExists(_destinationFilePath, _destinationInstrumentName))
                .Returns(true);

            _fileServiceMock.Setup(f => f.GetDatabaseFileName(_destinationFilePath, _destinationInstrumentName))
                .Returns(databaseFilePath);

            //act
            _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName, _destinationFilePath,
                _destinationInstrumentName);

            //assert
            _fileServiceMock.Verify(v => v.CreateDatabaseFile(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>()), Times.Never);
            _dataServiceMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, databaseFilePath), Times.Once);
            _dataServiceMock.Verify(v => v.RemoveDataRecord(It.IsAny<ConnectionModel>(), _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName), Times.Once);
        }

        [Test]
        public void Given_File_Does_Not_Exist_When_I_Call_MoveCase_The_Correct_Services_Are_Called()
        {
            //arrange
            var databaseFilePath = "FilePath";
            var metaFileName = "MetaFileName";

            var dataRecordMock = new Mock<IDataRecord>();
            _dataServiceMock.Setup(d => d.GetDataRecord(It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(dataRecordMock.Object);

            _surveyServiceMock.Setup(s => s.GetMetaFileName(It.IsAny<ConnectionModel>(), _sourceInstrumentName, _sourceServerParkName))
                .Returns(metaFileName);

            _fileServiceMock.Setup(f => f.DatabaseFileExists(_destinationFilePath, _destinationInstrumentName))
                .Returns(false);

            _fileServiceMock.Setup(f => f.GetDatabaseFileName(_destinationFilePath, _destinationInstrumentName))
                .Returns(databaseFilePath);

            //act
            _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName, _destinationFilePath,
                _destinationInstrumentName);

            //assert
            Assert.AreEqual(_sourceServerName, _sourceConnectionModel.ServerName);
            _fileServiceMock.Verify(v => v.CreateDatabaseFile(metaFileName, databaseFilePath, _destinationInstrumentName), Times.Once);
            _dataServiceMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, databaseFilePath), Times.Once);
            _dataServiceMock.Verify(v => v.RemoveDataRecord(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName), Times.Once);
        }

        [Test]
        public void Given_A_Null_SourceConnectionModel_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(null, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("The argument 'sourceConnectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_ForFile_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MoveCase(_sourceConnectionModel, string.Empty, _sourceInstrumentName, _sourceServerParkName,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_ForFile_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(_sourceConnectionModel, null, _sourceInstrumentName, _sourceServerParkName,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_SourceInstrumentName_ForFile_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, string.Empty, _sourceServerParkName,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("A value for the argument 'sourceInstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_SourceInstrumentName_ForFile_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, null, _sourceServerParkName,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("sourceInstrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_SourceServerParkName_ForFile_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, string.Empty,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("A value for the argument 'sourceServerParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_SourceServerParkName_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, null,
                _destinationFilePath, _destinationInstrumentName));
            Assert.AreEqual("sourceServerParkName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationFilePath_ForFile_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                string.Empty, _destinationInstrumentName));
            Assert.AreEqual("A value for the argument 'destinationFilePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationFilePath_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                null, _destinationInstrumentName));
            Assert.AreEqual("destinationFilePath", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_DestinationInstrumentName_ForFile_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationFilePath, string.Empty));
            Assert.AreEqual("A value for the argument 'destinationInstrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_DestinationInstrumentName_When_I_Call_MoveCase_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MoveCase(_sourceConnectionModel, _primaryKeyValue, _sourceInstrumentName, _sourceServerParkName,
                _destinationFilePath, null));
            Assert.AreEqual("destinationInstrumentName", exception.ParamName);
        }
    }
}
