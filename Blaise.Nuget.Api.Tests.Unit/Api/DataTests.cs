using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
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
    public class DataTests
    {
        private Mock<IDataService> _dataServiceMock;
        private Mock<IParkService> _parkServiceMock;
        private Mock<ISurveyService> _surveyServiceMock;
        private Mock<IUserService> _userServiceMock;
        private Mock<IFileService> _fileServiceMock;
        private Mock<IIocProvider> _unityProviderMock;
        private Mock<IConfigurationProvider> _configurationProviderMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _primaryKeyValue;
        private readonly string _serverParkName;
        private readonly string _instrumentName;
        private readonly string _filePath;

        private IBlaiseApi _sut;

        public DataTests()
        {
            _connectionModel = new ConnectionModel();
            _primaryKeyValue = "Key1";
            _serverParkName = "Park1";
            _instrumentName = "Instrument1";
            _filePath = "TestFilePath";
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
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _dataServiceMock.Setup(d => d.CaseExists(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            //act
            _sut.CaseExists(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.CaseExists(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Expected_Result_Is_Returned(bool caseExists)
        {
            //arrange
            var primaryKeyValue = "Key1";

            _dataServiceMock.Setup(d => d.CaseExists(_connectionModel, primaryKeyValue, _instrumentName, _serverParkName)).Returns(caseExists);

            //act
            var result = _sut.CaseExists(_connectionModel, primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseExists, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(null, _primaryKeyValue, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_CaseExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CaseExists(_connectionModel, string.Empty, _instrumentName, _serverParkName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(_connectionModel, (string)null, _instrumentName, _serverParkName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CaseExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CaseExists(_connectionModel, _primaryKeyValue, string.Empty,
                _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(_connectionModel, _primaryKeyValue, null,
                _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CaseExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CaseExists(_connectionModel, _primaryKeyValue,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(_connectionModel, _primaryKeyValue,
                _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetPrimaryKeyValue_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(It.IsAny<string>());

            //act
            _sut.GetPrimaryKeyValue(dataRecordMock.Object);

            //assert
            _dataServiceMock.Verify(v => v.GetPrimaryKeyValue(dataRecordMock.Object), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetPrimaryKeyValue_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(_primaryKeyValue);

            //act
            var result = _sut.GetPrimaryKeyValue(dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_primaryKeyValue, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDataSet_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _dataServiceMock.Setup(d => d.GetDataSet(_connectionModel, It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetDataSet(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.GetDataSet(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetDataSet_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataSet(null, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetDataSet_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataSet(_connectionModel, string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetDataSet_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataSet(_connectionModel, null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetDataSet_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataSet(_connectionModel, _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetDataSet_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataSet(_connectionModel, _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_A_File_When_I_Call_GetDataSet_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var filePath = "File1.bdix";

            _dataServiceMock.Setup(d => d.GetDataSet(It.IsAny<string>()));

            //act
            _sut.GetDataSet(filePath);

            //assert
            _dataServiceMock.Verify(v => v.GetDataSet(filePath), Times.Once);
        }

        [Test]
        public void Given_An_Empty_FilePath_When_I_Call_GetDataSet_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataSet(string.Empty));
            Assert.AreEqual("A value for the argument 'filePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_FilePath_When_I_Call_GetDataSet_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataSet(null));
            Assert.AreEqual("filePath", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDataRecord_ByPrimaryKeyValue_Then_The_Correct_Service_Method_Is_Called()
        {

            _dataServiceMock.Setup(d => d.GetDataRecord(_connectionModel, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetDataRecord(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetDataRecord_ByPrimaryKeyValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(null, _primaryKeyValue, _instrumentName,
                _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_GetDataRecord_ByPrimaryKeyValue_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataRecord(_connectionModel, string.Empty, _instrumentName, _serverParkName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_GetDataRecord_ByPrimaryKeyValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(_connectionModel, (string)null, _instrumentName, _serverParkName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetDataRecord_ByPrimaryKeyValue_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataRecord(_connectionModel, _primaryKeyValue,
                string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetDataRecord_ByPrimaryKeyValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var primaryKeyValue = "Key1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(_connectionModel, primaryKeyValue, null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetDataRecord_ByPrimaryKeyValue_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataRecord(_connectionModel, _primaryKeyValue,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetDataRecord_ByPrimaryKeyValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(_connectionModel, _primaryKeyValue,
                _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateNewDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _dataServiceMock.Setup(d => d.CreateNewDataRecord(_connectionModel, It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.CreateNewDataRecord(_connectionModel, _primaryKeyValue, fieldData, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.CreateNewDataRecord(_connectionModel, _primaryKeyValue, fieldData, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_CreateNewDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateNewDataRecord(null, _primaryKeyValue,
                fieldData, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_CreateNewDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateNewDataRecord(_connectionModel, string.Empty, fieldData, _instrumentName, _serverParkName));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_CreateNewDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateNewDataRecord(_connectionModel, null, fieldData, _instrumentName, _serverParkName));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_Dictionary_Of_FieldData_When_I_Call_CreateNewDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateNewDataRecord(_connectionModel, _primaryKeyValue, null,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'fieldData' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_CreateNewDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateNewDataRecord(_connectionModel, _primaryKeyValue, fieldData,
                string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_CallCreateNewDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateNewDataRecord(_connectionModel, _primaryKeyValue, fieldData,
                null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_CreateNewDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateNewDataRecord(_connectionModel, _primaryKeyValue, fieldData,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_CreateNewDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateNewDataRecord(_connectionModel, _primaryKeyValue, fieldData,
                _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateNewDataRecord_For_Local_Connection_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _dataServiceMock.Setup(d => d.CreateNewDataRecord(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()));

            //act
            _sut.CreateNewDataRecord(_filePath, _primaryKeyValue, fieldData);

            //assert
            _dataServiceMock.Verify(v => v.CreateNewDataRecord(_filePath, _primaryKeyValue, fieldData), Times.Once);
        }

        [Test]
        public void Given_An_Empty_FilePath_When_I_Call_CreateNewDataRecord_For_Local_Connection_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateNewDataRecord(string.Empty, _primaryKeyValue, fieldData));
            Assert.AreEqual("A value for the argument 'filePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_FilePath_When_I_Call_CreateNewDataRecord_For_Local_Connection_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateNewDataRecord(null, _primaryKeyValue, fieldData));
            Assert.AreEqual("filePath", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_CreateNewDataRecord_For_Local_Connection_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CreateNewDataRecord(_filePath, string.Empty, fieldData));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_CreateNewDataRecord_For_Local_Connection_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateNewDataRecord(_filePath, null, fieldData));
            Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_Dictionary_Of_FieldData_When_I_Call_CreateNewDataRecord_For_Local_Connection_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CreateNewDataRecord(_filePath, _primaryKeyValue, null));
            Assert.AreEqual("The argument 'fieldData' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            _dataServiceMock.Setup(d => d.UpdateDataRecord(_connectionModel, It.IsAny<IDataRecord>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.UpdateDataRecord(_connectionModel, dataRecordMock.Object, fieldData, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.UpdateDataRecord(_connectionModel, dataRecordMock.Object, fieldData,
                _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_UpdateDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateDataRecord(null, dataRecordMock.Object, fieldData,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_UpdateDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateDataRecord(_connectionModel, null, fieldData,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_Dictionary_Of_FieldData_When_I_Call_UpdateDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateDataRecord(_connectionModel, dataRecordMock.Object, null,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'fieldData' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_UpdateDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateDataRecord(_connectionModel, dataRecordMock.Object, fieldData, string.Empty,
                _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_UpdateDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateDataRecord(_connectionModel, dataRecordMock.Object, fieldData,
                null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_UpdateDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.UpdateDataRecord(_connectionModel, dataRecordMock.Object, fieldData,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_UpdateDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var dataRecordMock = new Mock<IDataRecord>();
            var fieldData = new Dictionary<string, string>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.UpdateDataRecord(_connectionModel, dataRecordMock.Object,
                fieldData, _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
        [TestCase(FieldNameType.HOut)]
        public void Given_Valid_Arguments_When_I_Call_FieldExists_Then_The_Correct_Service_Method_Is_Called(FieldNameType fieldNameType)
        {
            //arrange
            _dataServiceMock.Setup(d => d.FieldExists(_connectionModel, It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<FieldNameType>())).Returns(It.IsAny<bool>());

            //act
            _sut.FieldExists(_connectionModel, _instrumentName, _serverParkName, fieldNameType);

            //CompletedFieldExists
            _dataServiceMock.Verify(v => v.FieldExists(_connectionModel, _instrumentName, _serverParkName, fieldNameType), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_FieldExists_Then_The_Expected_Result_Is_Returned(bool fieldExists)
        {
            //arrange
            _dataServiceMock.Setup(d => d.FieldExists(_connectionModel, It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<FieldNameType>())).Returns(fieldExists);

            //act
            var result = _sut.FieldExists(_connectionModel, _instrumentName, _serverParkName, FieldNameType.HOut);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(null, _instrumentName,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_FieldExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.FieldExists(_connectionModel, string.Empty,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(_connectionModel, null,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_FieldExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.FieldExists(_connectionModel, _instrumentName,
                string.Empty, FieldNameType.HOut));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(_connectionModel, _instrumentName,
                null, FieldNameType.HOut));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [TestCase(FieldNameType.HOut)]
        public void Given_A_DataRecord_When_I_Call_FieldExists_Then_The_Correct_Service_Method_Is_Called(FieldNameType fieldNameType)
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            _dataServiceMock.Setup(d => d.FieldExists(It.IsAny<IDataRecord>(), It.IsAny<FieldNameType>())).Returns(It.IsAny<bool>());

            //act
            _sut.FieldExists(dataRecordMock.Object, fieldNameType);

            //CompletedFieldExists
            _dataServiceMock.Verify(v => v.FieldExists(dataRecordMock.Object, fieldNameType), Times.Once);
        }


        [TestCase(FieldNameType.HOut, true)]
        [TestCase(FieldNameType.HOut, false)]
        public void Given_A_DataRecord_When_I_Call_FieldExists_Then_The_Expected_Result_Is_Returned(FieldNameType fieldNameType, bool exists)
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            _dataServiceMock.Setup(d => d.FieldExists(dataRecordMock.Object, fieldNameType)).Returns(exists);

            //act
            var result = _sut.FieldExists(dataRecordMock.Object, fieldNameType);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exists, result);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(null, FieldNameType.HOut));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }


       
        [TestCase(FieldNameType.HOut)]
        public void Given_A_PrimaryKeyValue_When_I_Call_GetFieldValue_Then_The_Correct_Value(FieldNameType fieldNameType)
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d =>
                    d.GetDataRecord(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName))
                .Returns(dataRecordMock.Object);

            _dataServiceMock.Setup(d => d.GetFieldValue(It.IsAny<IDataRecord>(), It.IsAny<FieldNameType>()))
                .Returns(dataValueMock.Object);

            //act
            var result = _sut.GetFieldValue(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName, fieldNameType);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dataValueMock.Object, result);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_A_Null_ConnectionModel_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(null, _primaryKeyValue, _instrumentName,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_GetFieldValue_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(_connectionModel, string.Empty, _instrumentName,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(_connectionModel, null, _instrumentName,
                _serverParkName, FieldNameType.HOut)); Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_An_Empty_InstrumentName_When_I_Call_GetFieldValue_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(_connectionModel, _primaryKeyValue, string.Empty,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_A_Null_InstrumentName_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(_connectionModel, _primaryKeyValue, null,
                _serverParkName, FieldNameType.HOut));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_An_Empty_ServerParkName_When_I_Call_GetFieldValue_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(_connectionModel, _primaryKeyValue, _instrumentName,
                string.Empty, FieldNameType.HOut));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_A_Null_ServerParkName_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(_connectionModel, _primaryKeyValue, _instrumentName,
                null, FieldNameType.HOut));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetNumberOfCases_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _dataServiceMock.Setup(d => d.GetNumberOfCases(
                _connectionModel, It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetNumberOfCases(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.GetNumberOfCases(
                _connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetNumberOfCases_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            var numberOfCases = 5;
            _dataServiceMock.Setup(d => d.GetNumberOfCases(
                _connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(numberOfCases);

            //act
            var result =_sut.GetNumberOfCases(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(numberOfCases, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetNumberOfCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNumberOfCases(null, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetNumberOfCases_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNumberOfCases(_connectionModel, string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetNumberOfCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNumberOfCases(_connectionModel, null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetNumberOfCases_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNumberOfCases(_connectionModel, _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetNumberOfCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNumberOfCases(_connectionModel, _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_A_File_When_I_Call_GetNumberOfCases_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var filePath = "File1.bdix";

            _dataServiceMock.Setup(d => d.GetNumberOfCases(It.IsAny<string>()));

            //act
            _sut.GetNumberOfCases(filePath);

            //assert
            _dataServiceMock.Verify(v => v.GetNumberOfCases(filePath), Times.Once);
        }

        [Test]
        public void Given_A_File_When_I_Call_GetNumberOfCases_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            var numberOfCases = 5;

            _dataServiceMock.Setup(d => d.GetNumberOfCases(
                It.IsAny<string>())).Returns(numberOfCases);

            //act
            var result = _sut.GetNumberOfCases(_filePath);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(numberOfCases, result);
        }

        [Test]
        public void Given_An_Empty_FilePath_When_I_Call_GetNumberOfCases_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetNumberOfCases(string.Empty));
            Assert.AreEqual("A value for the argument 'filePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_FilePath_When_I_Call_GetNumberOfCases_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetNumberOfCases(null));
            Assert.AreEqual("filePath", exception.ParamName);
        }
    }
}
