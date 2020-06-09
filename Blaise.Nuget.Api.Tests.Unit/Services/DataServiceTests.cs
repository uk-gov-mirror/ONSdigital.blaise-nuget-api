using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Core.Interfaces.Mappers;
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class DataServiceTests
    {
        private Mock<IDataModelService> _dataModelServiceMock;
        private Mock<IKeyService> _keyServiceMock;
        private Mock<IDataRecordService> _dataRecordServiceMock;
        private Mock<IFieldService> _fieldServiceMock;
        private Mock<IDataMapperService> _mapperServiceMock;

        private Mock<IDatamodel> _dataModelMock;
        private Mock<IKey> _keyMock;
        private Mock<IDataRecord> _dataRecordMock;

        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _filePath;
        private readonly string _keyName;
        private readonly SurveyType _surveyType;

        private DataService _sut;

        public DataServiceTests()
        {
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
            _dataModelServiceMock.Setup(d => d.GetDataModel(_instrumentName, _serverParkName)).Returns(_dataModelMock.Object);
            _dataModelServiceMock.Setup(d => d.GetSurveyType(_instrumentName, _serverParkName)).Returns(_surveyType);

            _keyServiceMock = new Mock<IKeyService>();
            _keyServiceMock.Setup(d => d.KeyExists(_keyMock.Object, _instrumentName, _serverParkName)).Returns(true);
            _keyServiceMock.Setup(d => d.GetKey(_dataModelMock.Object, _keyName)).Returns(_keyMock.Object);
            _keyServiceMock.Setup(d => d.GetPrimaryKey(_dataModelMock.Object)).Returns(_keyMock.Object);

            _dataRecordServiceMock = new Mock<IDataRecordService>();
            _dataRecordServiceMock.Setup(d => d.GetDataRecord(_dataModelMock.Object)).Returns(_dataRecordMock.Object);

            _fieldServiceMock = new Mock<IFieldService>();

            _mapperServiceMock = new Mock<IDataMapperService>();

            _sut = new DataService(
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
            var result = _sut.GetDataModel(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDatamodel>(result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_Then_The_Correct_DataModel_Is_Returned()
        {
            //act
            var result = _sut.GetDataModel(_instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(_dataModelMock.Object, result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataModel(_instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetSurveyType_Then_A_SurveyType_Is_Returned()
        {
            //act
            var result = _sut.GetSurveyType(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<SurveyType>(result);
        }

        [Test]
        public void Given_I_Call_GetSurveyType_Then_The_Correct_SurveyType_Is_Returned()
        {
            //act
            var result = _sut.GetSurveyType(_instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(_surveyType, result);
        }

        [Test]
        public void Given_I_Call_GetSurveyType_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetSurveyType(_instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetSurveyType(_instrumentName, _serverParkName), Times.Once);
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
            var result = _sut.KeyExists(_keyMock.Object, _instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<bool>(result);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_I_Call_KeyExists_Then_A_The_Correct_Value_Is_Returned(bool keyExists)
        {
            //arrange 
            _keyServiceMock.Setup(d => d.KeyExists(_keyMock.Object, _instrumentName, _serverParkName)).Returns(keyExists);

            //act
            var result = _sut.KeyExists(_keyMock.Object, _instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(keyExists, result);
        }

        [Test]
        public void Given_I_Call_KeyExists_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.KeyExists(_keyMock.Object, _instrumentName, _serverParkName);

            //assert
            _keyServiceMock.Verify(v => v.KeyExists(_keyMock.Object, _instrumentName, _serverParkName), Times.Once);
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
            _sut.GetDataSet(_instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataSet(_instrumentName, _serverParkName), Times.Once);
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
            _sut.GetDataRecord(_keyMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_KeyValue_And_An_InstrumentName_And_ServerParkName_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataRecord(_keyName, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_keyMock.Object, _instrumentName, _serverParkName), Times.Once);
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
            _sut.WriteDataRecord(_dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Filepath_When_I_Call_WriteDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.WriteDataRecord(_dataRecordMock.Object, _filePath);

            //assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_dataRecordMock.Object, _filePath), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CompletedFieldExists_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.CompletedFieldExists(_instrumentName, _serverParkName);

            //assert
            _fieldServiceMock.Verify(v => v.CompletedFieldExists(_instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CompletedFieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            //arrange
            _fieldServiceMock.Setup(f => f.CompletedFieldExists(It.IsAny<string>(), It.IsAny<string>())).Returns(fieldExists);

            //act
            var result = _sut.CompletedFieldExists(_instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_CaseHasBeenCompleted_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.CaseHasBeenCompleted(_dataRecordMock.Object);

            //assert
            _fieldServiceMock.Verify(v => v.CaseHasBeenCompleted(_dataRecordMock.Object), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_DataRecord_When_I_Call_CaseHasBeenCompleted_Then_The_Correct_Value_Is_Returned(bool caseIsComplete)
        {
            //arrange
            _fieldServiceMock.Setup(f => f.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(caseIsComplete);

            //act
            var result =_sut.CaseHasBeenCompleted(_dataRecordMock.Object);

            //assert
           Assert.AreEqual(caseIsComplete, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_MarkCaseAsComplete_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.MarkCaseAsComplete(_dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _fieldServiceMock.Verify(v => v.MarkCaseAsComplete(_dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_ProcessedFieldExists_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.ProcessedFieldExists(_instrumentName, _serverParkName);

            //assert
            _fieldServiceMock.Verify(v => v.ProcessedFieldExists(_instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_ProcessedFieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            //arrange
            _fieldServiceMock.Setup(f => f.ProcessedFieldExists(It.IsAny<string>(), It.IsAny<string>())).Returns(fieldExists);

            //act
            var result = _sut.ProcessedFieldExists(_instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_A_DataRecord_When_I_Call_CaseHasBeenProcessed_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.CaseHasBeenProcessed(_dataRecordMock.Object);

            //assert
            _fieldServiceMock.Verify(v => v.CaseHasBeenProcessed(_dataRecordMock.Object), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_DataRecord_When_I_Call_CaseHasBeenProcessed_Then_The_Correct_Value_Is_Returned(bool caseIsProcessed)
        {
            //arrange
            _fieldServiceMock.Setup(f => f.CaseHasBeenProcessed(It.IsAny<IDataRecord>())).Returns(caseIsProcessed);

            //act
            var result = _sut.CaseHasBeenProcessed(_dataRecordMock.Object);

            //assert
            Assert.AreEqual(caseIsProcessed, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_MarkCaseAsProcessed_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.MarkCaseAsProcessed(_dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _fieldServiceMock.Verify(v => v.MarkCaseAsProcessed(_dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var primaryKeyValue = "Key1";

            //act
            _sut.CaseExists(primaryKeyValue, _instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_instrumentName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _keyServiceMock.Verify(v => v.AssignPrimaryKeyValue(_keyMock.Object, primaryKeyValue), Times.Once);
            _keyServiceMock.Verify(v => v.KeyExists(_keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CaseExists_Then_The_Expected_Value_Is_Returned(bool caseExists)
        {
            //arrange
            var primaryKeyValue = "Key1";
            _keyServiceMock.Setup(k => k.KeyExists(_keyMock.Object, _instrumentName, _serverParkName))
                .Returns(caseExists);

            //act
            var result = _sut.CaseExists(primaryKeyValue, _instrumentName, _serverParkName);

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
            _sut.CreateNewDataRecord(primaryKeyValue, fieldData, _instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(v => v.GetDataModel(_instrumentName, _serverParkName), Times.Once);
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataModelMock.Object), Times.Once);
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_dataModelMock.Object), Times.Once);
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, _dataModelMock.Object, _keyMock.Object, primaryKeyValue, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_UpdateDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _mapperServiceMock.Setup(m => m.MapDataRecordFields(It.IsAny<IDataRecord>(), 
                    It.IsAny<Dictionary<string, string>>())).Returns(_dataRecordMock.Object);

            //act
            _sut.UpdateDataRecord(_dataRecordMock.Object, fieldData, _instrumentName, _serverParkName);

            //assert
            _mapperServiceMock.Verify(v => v.MapDataRecordFields(_dataRecordMock.Object, fieldData), Times.Once);
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }
    }
}
