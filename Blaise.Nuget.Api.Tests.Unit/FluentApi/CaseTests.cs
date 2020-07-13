using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class CaseTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly ConnectionModel _connectionModel;
        private readonly string _filePath;
        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _primaryKeyValue;
        private readonly IDataRecord _caseDataRecord;

        private FluentBlaiseApi _sut;

        public CaseTests()
        {
            _filePath = "FilePath1";
            _connectionModel = new ConnectionModel();
            _instrumentName = "Instrument1";
            _serverParkName = "Park1";
            _primaryKeyValue = "Key1";
            _caseDataRecord = new Mock<IDataRecord>().Object;
        }


        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();

            _sut = new FluentBlaiseApi(_blaiseApiMock.Object);
        }

        [Test]
        public void Given_A_Primary_Key_Value_When_I_Call_Case_Then_It_Returns_Same_Instance_Of_Itself_Back()
        {
            //act
            var result = _sut.WithPrimaryKey(_primaryKeyValue);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IFluentBlaiseCaseApi>(result);
            Assert.AreSame(_sut, result);
        }

        [Test]
        public void Given_A_Case_Data_Record_When_I_Call_WithDataRecord_Then_It_Returns_Same_Instance_Of_Itself_Back()
        {
            //act
            var result = _sut.WithDataRecord(_caseDataRecord);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IFluentBlaiseCaseApi>(result);
            Assert.AreSame(_sut, result);
        }

        [Test]
        public void Given_All_Steps_Are_Called_When_I_Call_Add_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>
            {
                {"Key", "Value"}
            };

            _blaiseApiMock.Setup(d => d.CreateNewDataRecord(It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(), It.IsAny<string>()));

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithPrimaryKey(_primaryKeyValue);
            _sut.WithData(fieldData);

            //act
            _sut.Add();

            //assert
            _blaiseApiMock.Verify(
                v => v.CreateNewDataRecord(_connectionModel, _primaryKeyValue, fieldData, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithPrimaryKey(_primaryKeyValue);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid value prior to this to specify the source connection", exception.Message);
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithPrimaryKey(_primaryKeyValue);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual(
                "The 'WithServerPark' step needs to be called with a valid value prior to this to specify the name of the server park",
                exception.Message);
        }

        [Test]
        public void Given_WithInstrument_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithPrimaryKey(_instrumentName);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual(
                "The 'WithInstrument' step needs to be called with a valid value prior to this to specify the name of the instrument",
                exception.Message);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual(
                "The 'WithPrimaryKey' step needs to be called with a valid value prior to this to specify the primary key value of the case",
                exception.Message);
        }

        [Test]
        public void Given_WithData_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual(
                "The 'WithData' step needs to be called with a valid value prior to this to specify the data fields of the case",
                exception.Message);
        }

        [Test]
        public void Given_A_FilePath_Has_Been_Set_When_I_Call_Cases_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(d => d.GetDataSet(It.IsAny<string>()))
                .Returns(It.IsAny<IDataSet>());

            _sut.WithConnection(_connectionModel);
            _sut.WithFile(_filePath);

            //act
            var sutCases = _sut.Cases;

            //assert
            _blaiseApiMock.Verify(v => v.GetDataSet(_filePath), Times.Once);
        }

        [Test]
        public void Given_A_FilePath_Has_Been_Set_When_I_Call_Cases_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataSetMock = new Mock<IDataSet>();

            _blaiseApiMock.Setup(d => d.GetDataSet(_filePath)).Returns(dataSetMock.Object);

            _sut.WithConnection(_connectionModel);
            _sut.WithFile(_filePath);

            //act
            var result = _sut.Cases;

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(dataSetMock.Object, result);
        }

        [Test]
        public void Given_A_FilePath_Has_Not_Been_Set_When_I_Call_Cases_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(d => d.GetDataSet(It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<IDataSet>());

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act
            var sutCases = _sut.Cases;

            //assert
            _blaiseApiMock.Verify(v => v.GetDataSet(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_A_FilePath_Has_Not_Been_Set_When_I_Call_Cases_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataSetMock = new Mock<IDataSet>();

            _blaiseApiMock.Setup(d => d.GetDataSet(It.IsAny<ConnectionModel>(), _instrumentName, _serverParkName)).Returns(dataSetMock.Object);

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act
            var result = _sut.Cases;

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(dataSetMock.Object, result);
        }

        [Test]
        public void Given_A_FilePath_Has_Not_Been_Set_And_WithConnection_Has_Not_Been_Called_When_I_Call_Cases_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutCases = _sut.Cases;
            });

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid value prior to this to specify the source connection", exception.Message);
        }

        [Test]
        public void Given_A_FilePath_Has_Not_Been_Set_And_WithServerPark_Has_Not_Been_Called_When_I_Call_Cases_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutCases = _sut.Cases;
            });
            Assert.AreEqual(
                "The 'WithServerPark' step needs to be called with a valid value prior to this to specify the name of the server park",
                exception.Message);
        }

        [Test]
        public void Given_A_FilePath_Has_Not_Been_Set_And_WithInstrument_Has_Not_Been_Called_When_I_Call_Cases_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutCases = _sut.Cases;
            });
            Assert.AreEqual(
                "The 'WithInstrument' step needs to be called with a valid value prior to this to specify the name of the instrument",
                exception.Message);
        }


        [Test]
        public void Given_Valid_Arguments_When_I_Call_PrimaryKey_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(d => d.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(It.IsAny<string>());

            _sut.WithDataRecord(_caseDataRecord);

            //act
            var primaryKey = _sut.PrimaryKey;

            //assert
            _blaiseApiMock.Verify(v => v.GetPrimaryKeyValue(_caseDataRecord), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_PrimaryKey_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(_primaryKeyValue);

            _sut.WithDataRecord(_caseDataRecord);

            //act
            var result = _sut.PrimaryKey;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_primaryKeyValue, result);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_PrimaryKey_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutPrimaryKey = _sut.PrimaryKey;
            });
            Assert.AreEqual(
                "The 'WithDataRecord' step needs to be called with a valid value prior to this to specify the data record of the case",
                exception.Message);
        }

        [Test]
        public void Given_Case_Has_Been_Called_When_I_Call_Completed_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(It.IsAny<bool>());

            _sut.WithDataRecord(_caseDataRecord);

            //act
            var sutCompleted = _sut.Completed;

            //assert
            _blaiseApiMock.Verify(v => v.CaseHasBeenCompleted(_caseDataRecord), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Case_Has_Been_Called_When_I_Call_Completed_Then_The_Expected_Result_Is_Returned(
            bool caseIsComplete)
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenCompleted(_caseDataRecord)).Returns(caseIsComplete);

            _sut.WithDataRecord(_caseDataRecord);
            //act
            var result = _sut.Completed;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseIsComplete, result);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_Completed_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutCompleted = _sut.Completed;
            });
            Assert.AreEqual(
                "The 'WithDataRecord' step needs to be called with a valid value prior to this to specify the data record of the case",
                exception.Message);
        }

        [Test]
        public void Given_Case_Has_Been_Called_When_I_Call_Processed_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenProcessed(It.IsAny<IDataRecord>())).Returns(It.IsAny<bool>());

            _sut.WithDataRecord(_caseDataRecord);

            //act
            var sutProcessed = _sut.Processed;

            //assert
            _blaiseApiMock.Verify(v => v.CaseHasBeenProcessed(_caseDataRecord), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Case_Has_Been_Called_When_I_Call_Processed_Then_The_Expected_Result_Is_Returned(
            bool caseHasBeenProcessed)
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenProcessed(_caseDataRecord)).Returns(caseHasBeenProcessed);

            _sut.WithDataRecord(_caseDataRecord);
            //act
            var result = _sut.Processed;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseHasBeenProcessed, result);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_Processed_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutProcessed = _sut.Processed;
            });
            Assert.AreEqual(
                "The 'WithDataRecord' step needs to be called with a valid value prior to this to specify the data record of the case",
                exception.Message);
        }

        [Test]
        public void Given_WithData_Has_Been_Called_When_I_Call_Update_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>
            {
                {"Key", "Value"}
            };

            _blaiseApiMock.Setup(d =>
                d.MarkCaseAsComplete(It.IsAny<ConnectionModel>(), It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithData(fieldData);

            //act
            _sut.Update();

            //assert
            _blaiseApiMock.Verify(v => v.UpdateDataRecord(_connectionModel, _caseDataRecord, fieldData, _instrumentName, _serverParkName),
                Times.Once);
        }

        [Test]
        public void
            Given_WithData_Has_Been_Called_But_WithServerPark_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>
            {
                {"Key", "Value"}
            };

            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual(
                "The 'WithServerPark' step needs to be called with a valid value prior to this to specify the name of the server park",
                exception.Message);
        }

        [Test]
        public void
            Given_WithData_Has_Been_Called_But_WithInstrument_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>
            {
                {"Key", "Value"}
            };

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual(
                "The 'WithInstrument' step needs to be called with a valid value prior to this to specify the name of the instrument",
                exception.Message);
        }

        [Test]
        public void
            Given_WithData_Has_Been_Called_But_Case_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>
            {
                {"Key", "Value"}
            };

            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual(
                "The 'WithDataRecord' step needs to be called with a valid value prior to this to specify the data record of the case",
                exception.Message);
        }

        [Test]
        public void
            Given_A_CompleteStatus_But_WithData_Not_Called_When_I_Call_Update_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d =>
                d.MarkCaseAsComplete(It.IsAny<ConnectionModel>(), It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithStatus(CaseStatusType.Completed);

            //act
            _sut.Update();

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsComplete(_connectionModel, _caseDataRecord, _instrumentName, _serverParkName),
                Times.Once);
            _blaiseApiMock.Verify(v => v.MarkCaseAsProcessed(It.IsAny<ConnectionModel>(), It.IsAny<IDataRecord>(), It.IsAny<string>()
                , It.IsAny<string>()), Times.Never);
            _blaiseApiMock.Verify(v => v.UpdateDataRecord(It.IsAny<ConnectionModel>(), It.IsAny<IDataRecord>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void
            Given_A_ProcessedStatus_But_WithData_Not_Called_When_I_Call_Update_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d =>
                d.MarkCaseAsProcessed(It.IsAny<ConnectionModel>(), It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithStatus(CaseStatusType.Processed);

            //act
            _sut.Update();

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsProcessed(_connectionModel, _caseDataRecord, _instrumentName, _serverParkName),
                Times.Once);
            _blaiseApiMock.Verify(v => v.MarkCaseAsComplete(It.IsAny<ConnectionModel>(), _caseDataRecord, _instrumentName, _serverParkName),
                Times.Never);
            _blaiseApiMock.Verify(v => v.UpdateDataRecord(It.IsAny<ConnectionModel>(), It.IsAny<IDataRecord>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestCase(CaseStatusType.Completed)]
        [TestCase(CaseStatusType.Processed)]
        public void
            Given_WithConnection_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown(
                CaseStatusType statusType)
        {
            //arrange
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithStatus(statusType);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid value prior to this to specify the source connection", exception.Message);

        }

        [TestCase(CaseStatusType.Completed)]
        [TestCase(CaseStatusType.Processed)]
        public void
            Given_WithServerPark_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown(
                CaseStatusType statusType)
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithStatus(statusType);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual(
                "The 'WithServerPark' step needs to be called with a valid value prior to this to specify the name of the server park",
                exception.Message);
        }

        [TestCase(CaseStatusType.Completed)]
        [TestCase(CaseStatusType.Processed)]
        public void
            Given_WithInstrument_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown(
                CaseStatusType statusType)
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithStatus(statusType);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual(
                "The 'WithInstrument' step needs to be called with a valid value prior to this to specify the name of the instrument",
                exception.Message);
        }

        [TestCase(CaseStatusType.Completed)]
        [TestCase(CaseStatusType.Processed)]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown(
            CaseStatusType statusType)
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.WithStatus(statusType);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual(
                "The 'WithDataRecord' step needs to be called with a valid value prior to this to specify the data record of the case",
                exception.Message);
        }

        [Test]
        public void Given_Case_Is_Called_When_I_Call_Exists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(p => p.CaseExists(It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<bool>());

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act
            var sutExists = _sut.Exists;

            //assert
            _blaiseApiMock.Verify(v => v.CaseExists(It.IsAny<ConnectionModel>(), _primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Case_Is_Called_When_I_Call_Exists_Then_The_Expected_Result_Is_Returned(bool caseExists)
        {
            //arrange

            _blaiseApiMock.Setup(p => p.CaseExists(It.IsAny<ConnectionModel>(), _primaryKeyValue, _instrumentName, _serverParkName))
                .Returns(caseExists);

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act
            var result = _sut.Exists;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseExists, result);
        }

        [Test]
        public void Given_Case_Is_Called_But_WithConnection_Has_Not_Been_Called_When_I_Call_Exists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Case.Exists;
            });
            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid value prior to this to specify the source connection", exception.Message);
        }

        [Test]
        public void Given_Case_Is_Called_But_WithPrimary_Has_Not_Been_Called_When_I_Call_Exists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Case.Exists;
            });
            Assert.AreEqual(
                "The 'WithPrimaryKey' step needs to be called with a valid value prior to this to specify the primary key value of the case",
                exception.Message);
        }

        [Test]
        public void
            Given_Case_Is_Called_But_WithInstrument_Has_Not_Been_Called_When_I_Call_Exists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Exists;
            });
            Assert.AreEqual(
                "The 'WithInstrument' step needs to be called with a valid value prior to this to specify the name of the instrument",
                exception.Message);
        }

        [Test]
        public void
            Given_Case_Is_Called_But_WithServerPark_Has_Not_Been_Called_When_I_CallExists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Exists;
            });
            Assert.AreEqual(
                "The 'WithServerPark' step needs to be called with a valid value prior to this to specify the name of the server park",
                exception.Message);
        }

        [Test]
        public void Given_A_DataRecord_And_Case_Is_Called_When_I_Call_Exists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(p => p.CaseExists(It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<bool>());

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithDataRecord(_caseDataRecord);

            //act
            var sutExists = _sut.Exists;

            //assert
            _blaiseApiMock.Verify(v => v.CaseExists(It.IsAny<ConnectionModel>(), _caseDataRecord, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_A_DataRecord_And_Case_Is_Called_When_I_Call_Exists_Then_The_Expected_Result_Is_Returned(bool caseExists)
        {
            //arrange

            _blaiseApiMock.Setup(p => p.CaseExists(It.IsAny<ConnectionModel>(), _caseDataRecord, _instrumentName, _serverParkName))
                .Returns(caseExists);

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithDataRecord(_caseDataRecord);

            //act
            var result = _sut.Exists;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseExists, result);
        }

        [Test]
        public void Given_A_DataRecord_And_Case_Is_Called_But_WithConnection_Has_Not_Been_Called_When_I_Call_Exists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Case.WithDataRecord(_caseDataRecord);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Case.Exists;
            });
            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid value prior to this to specify the source connection", exception.Message);
        }

        [Test]
        public void Given_A_DataRecord_And_Case_Is_Called_But_WithInstrument_Has_Not_Been_Called_When_I_Call_Exists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.Case.WithDataRecord(_caseDataRecord);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Exists;
            });
            Assert.AreEqual(
                "The 'WithInstrument' step needs to be called with a valid value prior to this to specify the name of the instrument",
                exception.Message);
        }

        [Test]
        public void Given_A_DataRecord_And_Case_Is_Called_But_WithServerPark_Has_Not_Been_Called_When_I_CallExists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithDataRecord(_caseDataRecord);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Exists;
            });
            Assert.AreEqual(
                "The 'WithServerPark' step needs to be called with a valid value prior to this to specify the name of the server park",
                exception.Message);
        }

        [Test]
        public void Given_Case_Has_Been_Called_When_I_Call_Remove_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act
            _sut.Remove();

            //assert
            _blaiseApiMock.Verify(v => v.RemoveCase(_connectionModel, _primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }


        [Test]
        public void
            Given_WithConnection_Has_Not_Been_Called_When_I_Call_Remove_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Case.Remove());

            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid value prior to this to specify the source connection", exception.Message);
        }

        [Test]
        public void
            Given_WithPrimaryKey_Has_Not_Been_Called_When_I_Call_Remove_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            //_sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Case.Remove());
            Assert.AreEqual(
                "The 'WithPrimaryKey' step needs to be called with a valid value prior to this to specify the primary key value of the case",
                exception.Message);
        }

        [Test]
        public void
            Given_WithInstrument_Has_Not_Been_Called_When_I_Call_Remove_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            //_sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Case.Remove());
            Assert.AreEqual(
                "The 'WithInstrument' step needs to be called with a valid value prior to this to specify the name of the instrument",
                exception.Message);
        }

        [Test]
        public void
            Given_WithServerPark_Has_Not_Been_Called_When_I_Call_Remove_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            //_sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Case.Remove());
            Assert.AreEqual(
                "The 'WithServerPark' step needs to be called with a valid value prior to this to specify the name of the server park",
                exception.Message);
        }

        [Test]
        public void Given_Case_Is_Called_When_I_Call_Get_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetDataRecord(It.IsAny<ConnectionModel>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(It.IsAny<IDataRecord>());

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act
            var result = _sut.Get();

            //assert
            _blaiseApiMock.Verify(v => v.GetDataRecord(It.IsAny<ConnectionModel>(), _primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Case_Is_Called_When_I_Call_Get_Then_The_Expected_DataRecord_Is_Returned()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();

            _blaiseApiMock.Setup(p => p.GetDataRecord(It.IsAny<ConnectionModel>(), _primaryKeyValue, _instrumentName, _serverParkName))
                .Returns(dataRecordMock.Object);

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act
            var result = _sut.Get();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dataRecordMock.Object, result);
        }

        [Test]
        public void
            Given_Case_Is_Called_But_WithConnection_Has_Not_Been_Called_When_I_Call_Get_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Case.Get();
            });
            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid value prior to this to specify the source connection", exception.Message);
        }

        [Test]
        public void
            Given_Case_Is_Called_But_WithPrimaryKey_Has_Not_Been_Called_When_I_Call_Get_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Case.Get();
            });
            Assert.AreEqual(
                "The 'WithPrimaryKey' step needs to be called with a valid value prior to this to specify the primary key value of the case",
                exception.Message);
        }

        [Test]
        public void
            Given_Case_Is_Called_But_WithInstrument_Has_Not_Been_Called_When_I_Call_Get_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Get();
            });
            Assert.AreEqual(
                "The 'WithInstrument' step needs to be called with a valid value prior to this to specify the name of the instrument",
                exception.Message);
        }

        [Test]
        public void Given_Case_Is_Called_But_WithServerPark_Has_Not_Been_Called_When_I_Call_Get_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Get();
            });
        }

        [Test]
        public void Given_A_DataRecord_And_Case_Is_Called_When_I_Call_WebFormStatus_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            dataValueMock.Setup(d => d.EnumerationValue).Returns(1);

            _blaiseApiMock.Setup(b => b.GetFieldValue(_caseDataRecord, FieldNameType.WebFormStatus))
                .Returns(dataValueMock.Object);

            _sut.WithDataRecord(_caseDataRecord);

            //act
            var result = _sut.Case.WebFormStatus;

            //assert
            _blaiseApiMock.Verify(v => v.GetFieldValue(_caseDataRecord, FieldNameType.WebFormStatus), Times.Once);
        }

        [TestCase(0, WebFormStatusType.NotProcessed)]
        [TestCase(1, WebFormStatusType.Complete)]
        [TestCase(2, WebFormStatusType.Partial)]
        [TestCase(3, WebFormStatusType.NotSpecified)]
        public void Given_A_DataRecord_And_Case_Is_Called_When_I_Call_WebFormStatus_Then_The_Expected_Status_Is_Returned(int dataEnumerationValue, WebFormStatusType webFormStatusType)
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            dataValueMock.Setup(d => d.EnumerationValue).Returns(dataEnumerationValue);

            _blaiseApiMock.Setup(b => b.GetFieldValue(_caseDataRecord, FieldNameType.WebFormStatus))
                .Returns(dataValueMock.Object);

            _sut.WithDataRecord(_caseDataRecord);

            //act
            var result = _sut.WebFormStatus;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(webFormStatusType, result);
        }

        [Test]
        public void Given_A_PrimaryKey_And_Case_Is_Called_When_I_Call_WebFormStatus_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            dataValueMock.Setup(d => d.EnumerationValue).Returns(1);

            _blaiseApiMock.Setup(b => b.GetFieldValue(_connectionModel, _primaryKeyValue, _instrumentName, 
                    _serverParkName, FieldNameType.WebFormStatus))
                .Returns(dataValueMock.Object);

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithPrimaryKey(_primaryKeyValue);

            //act
            var result = _sut.Case.WebFormStatus;

            //assert
            _blaiseApiMock.Verify(v => v.GetFieldValue(_connectionModel, _primaryKeyValue, _instrumentName,
                _serverParkName, FieldNameType.WebFormStatus), Times.Once);
        }

        [TestCase(0, WebFormStatusType.NotProcessed)]
        [TestCase(1, WebFormStatusType.Complete)]
        [TestCase(2, WebFormStatusType.Partial)]
        [TestCase(3, WebFormStatusType.NotSpecified)]
        public void Given_A_PrimaryKey_And_Case_Is_Called_When_I_Call_WebFormStatus_Then_The_Expected_Status_Is_Returned(int dataEnumerationValue, WebFormStatusType webFormStatusType)
        {
            //arrange
            var dataValueMock = new Mock<IDataValue>();
            dataValueMock.Setup(d => d.EnumerationValue).Returns(dataEnumerationValue);

            _blaiseApiMock.Setup(b => b.GetFieldValue(_connectionModel, _primaryKeyValue, _instrumentName,
                    _serverParkName, FieldNameType.WebFormStatus))
                .Returns(dataValueMock.Object);

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithPrimaryKey(_primaryKeyValue);

            //act
            var result = _sut.WebFormStatus;

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(webFormStatusType, result);
        }

        [Test]
        public void Given_WithConnection_Has_Not_Been_Called_When_I_Call_WebFormStatus_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var webFormStatusType = _sut.WebFormStatus;

            });
            Assert.AreEqual("The 'WithConnection' step needs to be called with a valid value prior to this to specify the source connection", exception.Message);
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_WebFormStatus_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            _sut.WithConnection(_connectionModel);
            _sut.WithInstrument(_instrumentName);
            _sut.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var webFormStatusType = _sut.WebFormStatus;

            });

            Assert.AreEqual(
                "The 'WithServerPark' step needs to be called with a valid value prior to this to specify the name of the server park",
                exception.Message);
        }

        [Test]
        public void Given_WithInstrument_Has_Not_Been_Called_When_I_Call_WebFormStatus_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithPrimaryKey(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var webFormStatusType = _sut.WebFormStatus;

            });

            Assert.AreEqual(
                "The 'WithInstrument' step needs to be called with a valid value prior to this to specify the name of the instrument",
                exception.Message);
        }

        [Test]
        public void Given_WithPrimaryKey_Has_Not_Been_Called_When_I_Call_WebFormStatus_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.WithConnection(_connectionModel);
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var webFormStatusType = _sut.WebFormStatus;

            });

            Assert.AreEqual(
                "The 'WithPrimaryKey' step needs to be called with a valid value prior to this to specify the primary key value of the case",
                exception.Message);
        }
    }
}
