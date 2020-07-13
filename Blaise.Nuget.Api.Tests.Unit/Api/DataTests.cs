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
using StatNeth.Blaise.API.Meta;

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

        private IBlaiseApi _sut;

        public DataTests()
        {
            _connectionModel = new ConnectionModel();
            _primaryKeyValue = "Key1";
            _serverParkName = "Park1";
            _instrumentName = "Instrument1";
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
        public void Given_Valid_Instrument_AndServerPark_When_I_Call_GetDataModel_Then_The_Correct_Service_Method_Is_Called()
        {

            _dataServiceMock.Setup(d => d.GetDataModel(It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IDatamodel>());

            //act
            _sut.GetDataModel(_connectionModel, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.GetDataModel(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDataModel_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();

            _dataServiceMock.Setup(d => d.GetDataModel(_connectionModel, _instrumentName, _serverParkName)).Returns(dataModelMock.Object);

            //act
            var result = _sut.GetDataModel(_connectionModel, _instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(dataModelMock.Object, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetDataModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataModel(null, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetDataModel_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataModel(_connectionModel, string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetDataModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataModel(_connectionModel, null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetDataModel_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataModel(_connectionModel, _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetDataModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataModel(_connectionModel, _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetKey_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            var keyName = "Key1";

            _dataServiceMock.Setup(d => d.GetKey(It.IsAny<IDatamodel>(), It.IsAny<string>())).Returns(It.IsAny<IKey>());

            //act
            _sut.GetKey(dataModelMock.Object, keyName);

            //assert
            _dataServiceMock.Verify(v => v.GetKey(dataModelMock.Object, keyName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetKey_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            var keyName = "Key1";
            var keyMock = new Mock<IKey>();

            _dataServiceMock.Setup(d => d.GetKey(dataModelMock.Object, keyName)).Returns(keyMock.Object);

            //act
            var result = _sut.GetKey(dataModelMock.Object, keyName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(keyMock.Object, result);
        }

        [Test]
        public void Given_A_Null_DataModel_When_I_Call_GetKey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var keyName = "Key1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetKey(null, keyName));
            Assert.AreEqual("The argument 'dataModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_KeyName_When_I_Call_GetKey_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var dataModelMock = new Mock<IDatamodel>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetKey(dataModelMock.Object, string.Empty));
            Assert.AreEqual("A value for the argument 'keyName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_KeyName_When_I_Call_GetKey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var dataModelMock = new Mock<IDatamodel>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetKey(dataModelMock.Object, null));
            Assert.AreEqual("keyName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetPrimaryKey_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();

            _dataServiceMock.Setup(d => d.GetPrimaryKey(It.IsAny<IDatamodel>())).Returns(It.IsAny<IKey>());

            //act
            _sut.GetPrimaryKey(dataModelMock.Object);

            //assert
            _dataServiceMock.Verify(v => v.GetPrimaryKey(dataModelMock.Object), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetPrimaryKey_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            var keyMock = new Mock<IKey>();

            _dataServiceMock.Setup(d => d.GetPrimaryKey(dataModelMock.Object)).Returns(keyMock.Object);

            //act
            var result = _sut.GetPrimaryKey(dataModelMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(keyMock.Object, result);
        }

        [Test]
        public void Given_A_Null_DataModel_When_I_Call_GetPrimaryKey_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetPrimaryKey(null));
            Assert.AreEqual("The argument 'dataModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_KeyExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var keyMock = new Mock<IKey>();

            _dataServiceMock.Setup(d => d.KeyExists(It.IsAny<ConnectionModel>(), It.IsAny<IKey>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            //act
            _sut.KeyExists(_connectionModel, keyMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.KeyExists(_connectionModel, keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_KeyExists_Then_The_Expected_Result_Is_Returned(bool keyExists)
        {
            //arrange
            var keyMock = new Mock<IKey>();

            _dataServiceMock.Setup(d => d.KeyExists(_connectionModel, keyMock.Object, _instrumentName, _serverParkName)).Returns(keyExists);

            //act
            var result = _sut.KeyExists(_connectionModel, keyMock.Object, _instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(keyExists, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_KeyExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.KeyExists(null, keyMock.Object, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_Key_When_I_Call_KeyExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.KeyExists(_connectionModel, null, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'key' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_KeyExists_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.KeyExists(_connectionModel, keyMock.Object, string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_KeyExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.KeyExists(_connectionModel, keyMock.Object, null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_KeyExists_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.KeyExists(_connectionModel, keyMock.Object, _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_KeyExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.KeyExists(_connectionModel, keyMock.Object, _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
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
        public void Given_A_DataRecord_When_I_Call_CaseExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecord = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.GetPrimaryKeyValue(dataRecord.Object)).Returns(_primaryKeyValue);

            _dataServiceMock.Setup(d => d.CaseExists(It.IsAny<ConnectionModel>(),
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            //act
            _sut.CaseExists(_connectionModel, dataRecord.Object, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.CaseExists(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_DataRecord_When_I_Call_CaseExists_Then_The_Expected_Result_Is_Returned(bool caseExists)
        {
            //arrange
            var dataRecord = new Mock<IDataRecord>();
            var primaryKeyValue = "Key1";

            _dataServiceMock.Setup(d => d.GetPrimaryKeyValue(dataRecord.Object)).Returns(primaryKeyValue);

            _dataServiceMock.Setup(d => d.CaseExists(_connectionModel, primaryKeyValue, _instrumentName, _serverParkName)).Returns(caseExists);

            //act
            var result = _sut.CaseExists(_connectionModel, dataRecord.Object, _instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseExists, result);
        }

        [Test]
        public void Given_A_DataRecord_And_A_Null_ConnectionModel_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecord = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(null, dataRecord.Object, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(_connectionModel, (IDataRecord)null, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_A_DataRecord_And_An_Empty_InstrumentName_When_I_Call_CaseExists_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var dataRecord = new Mock<IDataRecord>();
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CaseExists(_connectionModel, dataRecord.Object, string.Empty, serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_DataRecord_And_A_Null_InstrumentName_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var dataRecord = new Mock<IDataRecord>();
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(_connectionModel, dataRecord.Object, null, serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_A_DataRecord_And_An_Empty_ServerParkName_When_I_Call_CaseExists_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var dataRecord = new Mock<IDataRecord>();
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.CaseExists(_connectionModel, dataRecord.Object, instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_DataRecord_And_A_Null_ServerParkName_When_I_Call_CaseExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var dataRecord = new Mock<IDataRecord>();
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseExists(_connectionModel, dataRecord.Object, instrumentName, null));
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
        public void Given_A_Null_Key_When_I_Call_GetPrimaryKeyValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetPrimaryKeyValue(null));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_AssignPrimaryKeyValue_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var keyMock = new Mock<IKey>();

            _dataServiceMock.Setup(d => d.AssignPrimaryKeyValue(It.IsAny<IKey>(), It.IsAny<string>()));

            //act
            _sut.AssignPrimaryKeyValue(keyMock.Object, _primaryKeyValue);

            //assert
            _dataServiceMock.Verify(v => v.AssignPrimaryKeyValue(keyMock.Object, _primaryKeyValue), Times.Once);
        }

        [Test]
        public void Given_A_Null_Key_When_I_Call_AssignPrimaryKeyValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AssignPrimaryKeyValue(null, _primaryKeyValue));
            Assert.AreEqual("The argument 'key' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_primaryKey_When_I_Call_AssignPrimaryKeyValue_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.AssignPrimaryKeyValue(keyMock.Object, string.Empty));
            Assert.AreEqual("A value for the argument 'primaryKey' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_primaryKey_When_I_Call_AssignPrimaryKeyValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.AssignPrimaryKeyValue(keyMock.Object, null));
            Assert.AreEqual("primaryKey", exception.ParamName);
        }

        [Test]
        public void Given_A_Valid_DataRecord_When_I_Call_GetDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();

            _dataServiceMock.Setup(d => d.GetDataRecord(It.IsAny<IDatamodel>())).Returns(It.IsAny<IDataRecord>());

            //act
            _sut.GetDataRecord(dataModelMock.Object);

            //assert
            _dataServiceMock.Verify(v => v.GetDataRecord(dataModelMock.Object), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDataRecord_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.GetDataRecord(dataModelMock.Object)).Returns(dataRecordMock.Object);

            //act
            var result = _sut.GetDataRecord(dataModelMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(dataRecordMock.Object, result);
        }

        [Test]
        public void Given_A_Null_DataModel_When_I_Call_GetDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(null));
            Assert.AreEqual("The argument 'dataModel' must be supplied", exception.ParamName);
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
        public void Given_Valid_Arguments_When_I_Call_GetDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var keyMock = new Mock<IKey>();

            _dataServiceMock.Setup(d => d.GetDataRecord(_connectionModel, It.IsAny<IKey>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetDataRecord(_connectionModel, keyMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.GetDataRecord(_connectionModel, keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_GetDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(null, keyMock.Object, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_Key_When_I_Call_GetDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(_connectionModel, (IKey)null, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'key' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataRecord(_connectionModel, keyMock.Object, string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(_connectionModel, keyMock.Object, null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataRecord(_connectionModel, keyMock.Object, _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(_connectionModel, keyMock.Object, _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_A_Valid_FilePath_When_I_Call_GetDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var filePath = "FilePath";

            _dataServiceMock.Setup(d => d.GetDataRecord(It.IsAny<IKey>(), It.IsAny<string>()));

            //act
            _sut.GetDataRecord(keyMock.Object, filePath);

            //assert
            _dataServiceMock.Verify(v => v.GetDataRecord(keyMock.Object, filePath), Times.Once);
        }

        [Test]
        public void Given_A_Valid_FilePath_But_A_Null_Key_When_I_Call_GetDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var filePath = "FilePath";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(null, filePath));
            Assert.AreEqual("The argument 'key' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_FilePath_When_I_Call_GetDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataRecord(keyMock.Object, string.Empty));
            Assert.AreEqual("A value for the argument 'filePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_FilePath_When_I_Call_GetDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(keyMock.Object, null));
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
        public void Given_Valid_Arguments_When_I_Call_WriteDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.WriteDataRecord(_connectionModel, It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.WriteDataRecord(_connectionModel, dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_WriteDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WriteDataRecord(null, dataRecordMock.Object, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_WriteDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WriteDataRecord(_connectionModel, null, _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_WriteDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.WriteDataRecord(_connectionModel, dataRecordMock.Object, string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_WriteDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WriteDataRecord(_connectionModel, dataRecordMock.Object, null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_WriteDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.WriteDataRecord(_connectionModel, dataRecordMock.Object, _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_WriteDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WriteDataRecord(_connectionModel, dataRecordMock.Object, _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_A_Valid_FilePath_When_I_Call_WriteDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var filePath = "FilePath";

            _dataServiceMock.Setup(d => d.WriteDataRecord(It.IsAny<IDataRecord>(), It.IsAny<string>()));

            //act
            _sut.WriteDataRecord(dataRecordMock.Object, filePath);

            //assert
            _dataServiceMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, filePath), Times.Once);
        }

        [Test]
        public void Given_A_Valid_FilePath_But_A_Null_DataRecord_When_I_Call_WriteDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var filePath = "FilePath";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WriteDataRecord(null, filePath));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_FilePath_When_I_Call_WriteDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.WriteDataRecord(dataRecordMock.Object, string.Empty));
            Assert.AreEqual("A value for the argument 'filePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_FilePath_When_I_Call_WriteDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WriteDataRecord(dataRecordMock.Object, null));
            Assert.AreEqual("filePath", exception.ParamName);
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

        [TestCase(FieldNameType.Completed)]
        [TestCase(FieldNameType.Processed)]
        [TestCase(FieldNameType.WebFormStatus)]
        [TestCase(FieldNameType.CaseId)]
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
            var result = _sut.FieldExists(_connectionModel, _instrumentName, _serverParkName, FieldNameType.WebFormStatus);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(null, _instrumentName,
                _serverParkName, FieldNameType.WebFormStatus));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_FieldExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.FieldExists(_connectionModel, string.Empty,
                _serverParkName, FieldNameType.WebFormStatus));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(_connectionModel, null,
                _serverParkName, FieldNameType.WebFormStatus));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_FieldExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.FieldExists(_connectionModel, _instrumentName,
                string.Empty, FieldNameType.WebFormStatus));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_FieldExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(_connectionModel, _instrumentName,
                null, FieldNameType.WebFormStatus));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [TestCase(FieldNameType.Completed)]
        [TestCase(FieldNameType.Processed)]
        [TestCase(FieldNameType.WebFormStatus)]
        [TestCase(FieldNameType.CaseId)]
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


        [TestCase(FieldNameType.Completed, true)]
        [TestCase(FieldNameType.Completed, false)]
        [TestCase(FieldNameType.Processed, true)]
        [TestCase(FieldNameType.Processed, false)]
        [TestCase(FieldNameType.WebFormStatus, true)]
        [TestCase(FieldNameType.WebFormStatus, false)]
        [TestCase(FieldNameType.CaseId, true)]
        [TestCase(FieldNameType.CaseId, false)]
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
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.FieldExists(null, FieldNameType.WebFormStatus));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CaseHasBeenCompleted_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(It.IsAny<bool>());

            //act
            _sut.CaseHasBeenCompleted(dataRecordMock.Object);

            //assert
            _dataServiceMock.Verify(v => v.CaseHasBeenCompleted(dataRecordMock.Object), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CaseHasBeenCompleted_Then_The_Expected_Result_Is_Returned(bool caseIsComplete)
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(caseIsComplete);

            //act
            var result = _sut.CaseHasBeenCompleted(dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseIsComplete, result);
        }

        [Test]
        public void Given_A_Null_Key_When_I_Call_CaseHasBeenCompleted_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseHasBeenCompleted(null));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_CaseHasBeenProcessed_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.CaseHasBeenProcessed(It.IsAny<IDataRecord>())).Returns(It.IsAny<bool>());

            //act
            _sut.CaseHasBeenProcessed(dataRecordMock.Object);

            //assert
            _dataServiceMock.Verify(v => v.CaseHasBeenProcessed(dataRecordMock.Object), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_CaseHasBeenProcessed_Then_The_Expected_Result_Is_Returned(bool caseIsProcessed)
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.CaseHasBeenProcessed(It.IsAny<IDataRecord>())).Returns(caseIsProcessed);

            //act
            var result = _sut.CaseHasBeenProcessed(dataRecordMock.Object);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseIsProcessed, result);
        }

        [Test]
        public void Given_A_Null_Key_When_I_Call_CaseHasBeenProcessed_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.CaseHasBeenProcessed(null));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_MarkCaseAsComplete_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.MarkCaseAsComplete(_connectionModel, It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.MarkCaseAsComplete(_connectionModel, dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.MarkCaseAsComplete(_connectionModel, dataRecordMock.Object,
                _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_MarkCaseAsComplete_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MarkCaseAsComplete(null, dataRecordMock.Object,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_MarkCaseAsComplete_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MarkCaseAsComplete(_connectionModel, null,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_MarkCaseAsComplete_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MarkCaseAsComplete(_connectionModel, dataRecordMock.Object, string.Empty,
                _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_MarkCaseAsComplete_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MarkCaseAsComplete(_connectionModel, dataRecordMock.Object, null,
                _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_MarkCaseAsComplete_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MarkCaseAsComplete(_connectionModel, dataRecordMock.Object,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_MarkCaseAsComplete_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MarkCaseAsComplete(_connectionModel, dataRecordMock.Object,
                _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_MarkCaseAsProcessed_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.MarkCaseAsProcessed(_connectionModel, It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.MarkCaseAsProcessed(_connectionModel, dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.MarkCaseAsProcessed(It.IsAny<ConnectionModel>(),
                dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_Null_ConnectionModel_When_I_Call_MarkCaseAsProcessed_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MarkCaseAsProcessed(null, dataRecordMock.Object,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_MarkCaseAsProcessed_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MarkCaseAsProcessed(_connectionModel, null,
                _instrumentName, _serverParkName));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_MarkCaseAsProcessed_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MarkCaseAsProcessed(_connectionModel, dataRecordMock.Object,
                string.Empty, _serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_MarkCaseAsProcessed_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MarkCaseAsProcessed(_connectionModel, dataRecordMock.Object,
                null, _serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_MarkCaseAsProcessed_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.MarkCaseAsProcessed(_connectionModel, dataRecordMock.Object,
                _instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_MarkCaseAsProcessed_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var dataRecordMock = new Mock<IDataRecord>();

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.MarkCaseAsProcessed(_connectionModel, dataRecordMock.Object,
                _instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [TestCase(FieldNameType.Completed)]
        [TestCase(FieldNameType.Processed)]
        [TestCase(FieldNameType.WebFormStatus)]
        public void Given_A_DataRecord_When_I_Call_GetFieldValue_Then_The_Correct_Service_Method_Is_Called(FieldNameType fieldNameType)
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.GetFieldValue(It.IsAny<IDataRecord>(), It.IsAny<FieldNameType>()));

            //act
            _sut.GetFieldValue(dataRecordMock.Object, fieldNameType);

            //assert
            _dataServiceMock.Verify(v => v.GetFieldValue(dataRecordMock.Object, fieldNameType), Times.Once);
        }

        [TestCase(FieldNameType.Completed)]
        [TestCase(FieldNameType.Processed)]
        [TestCase(FieldNameType.WebFormStatus)]
        public void Given_A_DataRecord_When_I_Call_GetFieldValue_Then_The_Correct_Value(FieldNameType fieldNameType)
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d => d.GetFieldValue(It.IsAny<IDataRecord>(), It.IsAny<FieldNameType>()))
                .Returns(dataValueMock.Object);

            //act
            var result = _sut.GetFieldValue(dataRecordMock.Object, fieldNameType);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dataValueMock.Object, result);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception =
                Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(null, FieldNameType.WebFormStatus));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [TestCase(FieldNameType.Completed)]
        [TestCase(FieldNameType.Processed)]
        [TestCase(FieldNameType.WebFormStatus)]
        public void Given_A_PrimaryKeyValue_When_I_Call_GetFieldValue_Then_The_Correct_Service_Method_Is_Called(FieldNameType fieldNameType)
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _dataServiceMock.Setup(d =>
                    d.GetDataRecord(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName))
                .Returns(dataRecordMock.Object);

            _dataServiceMock.Setup(d => d.GetFieldValue(It.IsAny<IDataRecord>(), It.IsAny<FieldNameType>()));

            //act
            _sut.GetFieldValue(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName, fieldNameType);

            //assert
            _dataServiceMock.Verify(v => v.GetFieldValue(dataRecordMock.Object, fieldNameType), Times.Once);
        }

        [TestCase(FieldNameType.Completed)]
        [TestCase(FieldNameType.Processed)]
        [TestCase(FieldNameType.WebFormStatus)]
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
                _serverParkName, FieldNameType.WebFormStatus));
            Assert.AreEqual("The argument 'connectionModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_PrimaryKeyValue_When_I_Call_GetFieldValue_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(_connectionModel, string.Empty, _instrumentName,
                _serverParkName, FieldNameType.WebFormStatus));
            Assert.AreEqual("A value for the argument 'primaryKeyValue' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_PrimaryKeyValue_When_I_Call_GetFieldValue_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(_connectionModel, null, _instrumentName,
                _serverParkName, FieldNameType.WebFormStatus)); Assert.AreEqual("primaryKeyValue", exception.ParamName);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_An_Empty_InstrumentName_When_I_Call_GetDataModel_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(_connectionModel, _primaryKeyValue, string.Empty,
                _serverParkName, FieldNameType.WebFormStatus));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_A_Null_InstrumentName_When_I_Call_GetDataModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(_connectionModel, _primaryKeyValue, null,
                _serverParkName, FieldNameType.WebFormStatus));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_An_Empty_ServerParkName_When_I_Call_GetDataModel_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetFieldValue(_connectionModel, _primaryKeyValue, _instrumentName,
                string.Empty, FieldNameType.WebFormStatus));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_PrimaryKeyValue_And_A_Null_ServerParkName_When_I_Call_GetDataModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetFieldValue(_connectionModel, _primaryKeyValue, _instrumentName,
                null, FieldNameType.WebFormStatus));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }
    }
}
