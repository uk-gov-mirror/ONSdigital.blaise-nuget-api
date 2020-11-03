using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class CaseServiceTests
    {
        private Mock<IDataModelService> _dataModelServiceMock;
        private Mock<IKeyService> _keyServiceMock;
        private Mock<IDataRecordService> _dataRecordServiceMock;
        private Mock<IFieldService> _fieldServiceMock;
        private Mock<IDataMapperService> _mapperServiceMock;

        private Mock<IDatamodel> _dataModelMock;
        private Mock<IKey> _keyMock;
        private Mock<IDataRecord> _dataRecordMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _filePath;
        private readonly string _keyName;
        private readonly SurveyType _surveyType;

        private CaseService _sut;

        public CaseServiceTests()
        {
            _connectionModel = new ConnectionModel();
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _filePath = "c:\\filePath";
            _keyName = "TestKeyName";
            _surveyType = SurveyType.NotMapped;
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();
            _keyMock = new Mock<IKey>();
            _dataRecordMock = new Mock<IDataRecord>();

            _dataModelServiceMock = new Mock<IDataModelService>();
            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, _instrumentName, _serverParkName)).Returns(_dataModelMock.Object);
            _dataModelServiceMock.Setup(d => d.GetDataModel(_filePath)).Returns(_dataModelMock.Object);
            _dataModelServiceMock.Setup(d => d.GetSurveyType(_connectionModel, _instrumentName, _serverParkName)).Returns(_surveyType);

            _keyServiceMock = new Mock<IKeyService>();
            _keyServiceMock.Setup(d => d.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName)).Returns(true);
            _keyServiceMock.Setup(d => d.GetKey(_dataModelMock.Object, _keyName)).Returns(_keyMock.Object);
            _keyServiceMock.Setup(d => d.GetPrimaryKey(_dataModelMock.Object)).Returns(_keyMock.Object);

            _dataRecordServiceMock = new Mock<IDataRecordService>();
            _dataRecordServiceMock.Setup(d => d.GetDataRecord(_dataModelMock.Object)).Returns(_dataRecordMock.Object);

            _fieldServiceMock = new Mock<IFieldService>();

            _mapperServiceMock = new Mock<IDataMapperService>();

            _sut = new CaseService(
                _dataModelServiceMock.Object,
                _dataRecordServiceMock.Object,
                _keyServiceMock.Object,
                _fieldServiceMock.Object,
                _mapperServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_GetDataModel_Then_A_DataModel_Is_Returned()
        {
            //act
            var result = _sut.GetDataModel(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDatamodel>(result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_Then_The_Correct_DataModel_Is_Returned()
        {
            //act
            var result = _sut.GetDataModel(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(_dataModelMock.Object, result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataModel(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetDataModel_For_Local_Connection_Then_A_DataModel_Is_Returned()
        {
            //act
            var result = _sut.GetDataModel(_filePath);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDatamodel>(result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_For_Local_Connection_Then_The_Correct_DataModel_Is_Returned()
        {
            //act
            var result = _sut.GetDataModel(_filePath);

            //assert
            Assert.AreEqual(_dataModelMock.Object, result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_For_Local_Connection_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataModel(_filePath);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_filePath), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetSurveyType_Then_A_SurveyType_Is_Returned()
        {
            //act
            var result = _sut.GetSurveyType(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<SurveyType>(result);
        }

        [Test]
        public void Given_I_Call_GetSurveyType_Then_The_Correct_SurveyType_Is_Returned()
        {
            //act
            var result = _sut.GetSurveyType(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(_surveyType, result);
        }

        [Test]
        public void Given_I_Call_GetSurveyType_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetSurveyType(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetSurveyType(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetKey_Then_A_Key_Is_Returned()
        {
            //act
            var result = _sut.GetKey(_dataModelMock.Object, _keyName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IKey>(result);
        }

        [Test]
        public void Given_I_Call_GetKey_Then_The_Correct_Key_Is_Returned()
        {
            //act
            var result = _sut.GetKey(_dataModelMock.Object, _keyName);

            //assert
            Assert.AreSame(_keyMock.Object, result);
        }

        [Test]
        public void Given_I_Call_GetKey_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetKey(_dataModelMock.Object, _keyName);

            //assert
            _keyServiceMock.Verify(v => v.GetKey(_dataModelMock.Object, _keyName), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetPrimaryKey_Then_A_Key_Is_Returned()
        {
            //act
            var result = _sut.GetPrimaryKey(_dataModelMock.Object);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IKey>(result);
        }

        [Test]
        public void Given_I_Call_GetPrimaryKey_Then_The_Correct_Key_Is_Returned()
        {
            //act
            var result = _sut.GetPrimaryKey(_dataModelMock.Object);

            //assert
            Assert.AreSame(_keyMock.Object, result);
        }

        [Test]
        public void Given_I_Call_GetPrimaryKey_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetPrimaryKey(_dataModelMock.Object);

            //assert
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
        }

        [Test]
        public void Given_I_Call_KeyExists_Then_A_Bool_Is_Returned()
        {
            //act
            var result = _sut.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<bool>(result);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_I_Call_KeyExists_Then_A_The_Correct_Value_Is_Returned(bool keyExists)
        {
            //arrange 
            _keyServiceMock.Setup(d => d.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName)).Returns(keyExists);

            //act
            var result = _sut.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(keyExists, result);
        }

        [Test]
        public void Given_I_Call_KeyExists_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName);

            //assert
            _keyServiceMock.Verify(v => v.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetPrimaryKeyValue_Then_The_Correct_Key_Is_Returned()
        {
            //act
            var primaryKey = "Key1";
            _keyServiceMock.Setup(k => k.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(primaryKey);

            var result = _sut.GetPrimaryKeyValue(_dataRecordMock.Object);

            //assert
            Assert.AreSame(primaryKey, result);
        }

        [Test]
        public void Given_I_Call_GetPrimaryKeyValue_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            _keyServiceMock.Setup(k => k.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(It.IsAny<string>());

            //act
            _sut.GetPrimaryKeyValue(_dataRecordMock.Object);

            //assert
            _keyServiceMock.Verify(v => v.GetPrimaryKeyValue(_dataRecordMock.Object), Times.Once);
        }

        [Test]
        public void Given_I_Call_AssignPrimaryKeyValue_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var primaryKeyValue = "Key1";
            _keyServiceMock.Setup(k => k.AssignPrimaryKeyValue(It.IsAny<IKey>(), It.IsAny<string>()));
            //act

            _sut.AssignPrimaryKeyValue(_keyMock.Object, primaryKeyValue);

            //assert
            _keyServiceMock.Verify(v => v.AssignPrimaryKeyValue(_keyMock.Object, primaryKeyValue), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetDataSet_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataSet(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataSet(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_File_I_Call_GetDataSet_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataSet(_filePath);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataSet(_filePath), Times.Once);
        }

        [Test]
        public void Given_A_DataModel_When_I_Call_GetDataRecord_Then_A_DataRecord_Is_Returned()
        {
            //act
            var result = _sut.GetDataRecord(_dataModelMock.Object);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDataRecord>(result);
        }

        [Test]
        public void Given_A_DataModel_When_I_Call_GetDataRecord_Then_The_Correct_DataRecord_Is_Returned()
        {
            //act
            var result = _sut.GetDataRecord(_dataModelMock.Object);

            //assert
            Assert.AreSame(_dataRecordMock.Object, result);
        }

        [Test]
        public void Given_A_DataModel_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataRecord(_dataModelMock.Object);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_dataModelMock.Object), Times.Once);
        }

        [Test]
        public void Given_A_Key_And_An_InstrumentName_And_ServerParkName_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataRecord(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_KeyValue_And_An_InstrumentName_And_ServerParkName_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataRecord(_connectionModel, _keyName, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Filepath_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataRecord(_keyMock.Object, _filePath);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_keyMock.Object, _filePath), Times.Once);
        }

        [Test]
        public void Given_An_InstrumentName_And_ServerParkName_When_I_Call_WriteDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Filepath_When_I_Call_WriteDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.WriteDataRecord(_dataRecordMock.Object, _filePath);

            //assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_dataRecordMock.Object, _filePath), Times.Once);
        }

        [TestCase(FieldNameType.HOut)]
        public void Given_Valid_Arguments_When_I_Call_FieldExists_Then_The_Correct_Services_Are_Called(FieldNameType fieldNameType)
        {
            //act
            _sut.FieldExists(_connectionModel, _instrumentName, _serverParkName, fieldNameType);

            //assert
            _fieldServiceMock.Verify(v => v.FieldExists(_connectionModel, _instrumentName, _serverParkName, fieldNameType), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_FieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            //arrange
            _fieldServiceMock.Setup(f => f.FieldExists(_connectionModel, It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<FieldNameType>())).Returns(fieldExists);

            //act
            var result = _sut.FieldExists(_connectionModel, _instrumentName, _serverParkName, FieldNameType.HOut);

            //assert
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var primaryKeyValue = "Key1";

            //act
            _sut.CaseExists(_connectionModel, primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_connectionModel, _instrumentName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _keyServiceMock.Verify(v => v.AssignPrimaryKeyValue(_keyMock.Object, primaryKeyValue), Times.Once);
            _keyServiceMock.Verify(v => v.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Expected_Value_Is_Returned(bool caseExists)
        {
            //arrange
            var primaryKeyValue = "Key1";
            _keyServiceMock.Setup(k => k.KeyExists(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName))
                .Returns(caseExists);

            //act
            var result = _sut.CaseExists(_connectionModel, primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(caseExists, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateNewDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var primaryKeyValue = "Key1";
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(), It.IsAny<IDatamodel>(),
                    It.IsAny<IKey>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(_dataRecordMock.Object);

            //act
            _sut.CreateNewDataRecord(_connectionModel, primaryKeyValue, fieldData, _instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_connectionModel, _instrumentName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_dataModelMock.Object), Times.Once);
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, _dataModelMock.Object, _keyMock.Object, primaryKeyValue, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateNewDataRecord_For_Local_Connection_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var primaryKeyValue = "Key1";
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(), It.IsAny<IDatamodel>(),
                    It.IsAny<IKey>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>()))
                .Returns(_dataRecordMock.Object);

            //act
            _sut.CreateNewDataRecord(_filePath, primaryKeyValue, fieldData);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_filePath), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_dataModelMock.Object), Times.Once);
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, _dataModelMock.Object, _keyMock.Object, primaryKeyValue, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_dataRecordMock.Object, _filePath), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(), 
                    It.IsAny<Dictionary<string, string>>())).Returns(_dataRecordMock.Object);

            //act
            _sut.UpdateDataRecord(_connectionModel, _dataRecordMock.Object, fieldData, _instrumentName, _serverParkName);

            //assert
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, _dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_KeyValue_And_An_InstrumentName_And_ServerParkName_When_I_Call_RemoveDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.RemoveDataRecord(_connectionModel, _keyName, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.DeleteDataRecord(_connectionModel, _keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(FieldNameType.HOut)]
        public void Given_I_Call_GetFieldValue_Then_The_Correct_DataModel_Is_Returned(FieldNameType fieldNameType)
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            var fieldMock = new Mock<IField>();

            fieldMock.Setup(f => f.DataValue).Returns(dataValueMock.Object);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, fieldNameType)).Returns(fieldMock.Object);

            //act
            var result = _sut.GetFieldValue(_dataRecordMock.Object, fieldNameType);

            //assert
            Assert.AreEqual(dataValueMock.Object, result);
        }

        [TestCase(FieldNameType.HOut)]
        public void Given_I_Call_GetFieldValue_Then_The_Correct_Services_Are_Called(FieldNameType fieldNameType)
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            var fieldMock = new Mock<IField>();

            fieldMock.Setup(f => f.DataValue).Returns(dataValueMock.Object);
            _fieldServiceMock.Setup(f => f.GetField(_dataRecordMock.Object, fieldNameType)).Returns(fieldMock.Object);

            //act
            _sut.GetFieldValue(_dataRecordMock.Object, fieldNameType);

            //assert
            _fieldServiceMock.Verify(v => v.GetField(_dataRecordMock.Object, fieldNameType), Times.Once);
        }

        [TestCase(FieldNameType.HOut, true)]
        [TestCase(FieldNameType.HOut, false)]
        public void Given_I_Call_FieldExists_Then_The_Correct_DataModel_Is_Returned(FieldNameType fieldNameType, bool exists)
        {
            //arrange
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, fieldNameType)).Returns(exists);

            //act
            var result = _sut.FieldExists(_dataRecordMock.Object, fieldNameType);

            //assert
            Assert.AreEqual(exists, result);
        }

        [TestCase(FieldNameType.HOut)]
        public void Given_I_Call_FieldExists_Then_The_Correct_Services_Are_Called(FieldNameType fieldNameType)
        {
            //arrange
            _fieldServiceMock.Setup(f => f.FieldExists(_dataRecordMock.Object, fieldNameType)).Returns(true);

            //act
            _sut.FieldExists(_dataRecordMock.Object, fieldNameType);

            //assert
            _fieldServiceMock.Verify(v => v.FieldExists(_dataRecordMock.Object, fieldNameType), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetNumberOfCases_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetNumberOfCases(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetNumberOfRecords(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetNumberOfCases_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            var numberOfCases = 5;
            _dataRecordServiceMock.Setup(dr => dr.GetNumberOfRecords(
                _connectionModel, _instrumentName, _serverParkName)).Returns(numberOfCases);

            //act
            var result =_sut.GetNumberOfCases(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(numberOfCases, result);
        }

        [Test]
        public void Given_A_File_I_Call_GetNumberOfCases_For_Local_Connection_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetNumberOfCases(_filePath);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetNumberOfRecords(_filePath), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetNumberOfCases_For_Local_Connection_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            var numberOfCases = 5;
            _dataRecordServiceMock.Setup(dr => dr.GetNumberOfRecords(
                _filePath)).Returns(numberOfCases);

            //act
            var result = _sut.GetNumberOfCases(_filePath);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(numberOfCases, result);
        }
    }
}
