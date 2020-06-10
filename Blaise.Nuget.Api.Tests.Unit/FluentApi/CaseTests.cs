using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class CaseTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _primaryKeyValue;
        private readonly IDataRecord _caseDataRecord;

        private FluentBlaiseApi _sut;

        public CaseTests()
        {
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
        public void Given_A_Primary_Key_Value_When_I_Call_WithCase_Then_It_Returns_Same_Instance_Of_Itself_Back()
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

            _blaiseApiMock.Setup(d => d.CreateNewDataRecord(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithPrimaryKey(_primaryKeyValue);
            _sut.WithData(fieldData);

            //act
            _sut.Add();

            //assert
            _blaiseApiMock.Verify(v => v.CreateNewDataRecord(_primaryKeyValue, fieldData, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.WithInstrument(_instrumentName);
            _sut.WithPrimaryKey(_primaryKeyValue);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_WithInstrument_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.WithServerPark(_serverParkName);
            _sut.WithPrimaryKey(_instrumentName);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual("The 'WithInstrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_WithCase_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual("The 'WithPrimaryKey' step needs to be called prior to this to specify the primary key value of the case", exception.Message);
        }

        [Test]
        public void Given_WithData_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add());
            Assert.AreEqual("The 'WithData' step needs to be called prior to this to specify the data fields of the case", exception.Message);
        }


        [Test]
        public void Given_Valid_Arguments_When_I_Call_Cases_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(d => d.GetDataSet(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IDataSet>());

            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act
            var sutCases = _sut.Cases;

            //assert
            _blaiseApiMock.Verify(v => v.GetDataSet(_instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_Cases_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataSetMock = new Mock<IDataSet>();

            _blaiseApiMock.Setup(d => d.GetDataSet(_instrumentName, _serverParkName)).Returns(dataSetMock.Object);

            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);

            //act
            var result = _sut.Cases;

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(dataSetMock.Object, result);
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_Cases_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutCases = _sut.Cases;
            });
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_WithInstrument_Has_Not_Been_Called_When_I_Call_Cases_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutCases = _sut.Cases;
            });
            Assert.AreEqual("The 'WithInstrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
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
        public void Given_WithCase_Has_Not_Been_Called_When_I_Call_PrimaryKey_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutPrimaryKey = _sut.PrimaryKey;
            });
            Assert.AreEqual("The 'WithDataRecord' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_WithCase_Has_Been_Called_When_I_Call_Completed_Then_The_Correct_Service_Method_Is_Called()
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
        public void Given_WithCase_Has_Been_Called_When_I_Call_Completed_Then_The_Expected_Result_Is_Returned(bool caseIsComplete)
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
        public void Given_WithCase_Has_Not_Been_Called_When_I_Call_Completed_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutCompleted = _sut.Completed;
            });
            Assert.AreEqual("The 'WithDataRecord' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_WithCase_Has_Been_Called_When_I_Call_Processed_Then_The_Correct_Service_Method_Is_Called()
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
        public void Given_WithCase_Has_Been_Called_When_I_Call_Processed_Then_The_Expected_Result_Is_Returned(bool caseHasBeenProcessed)
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
        public void Given_WithCase_Has_Not_Been_Called_When_I_Call_Processed_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutProcessed = _sut.Processed;
            });
            Assert.AreEqual("The 'WithDataRecord' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_WithData_Has_Been_Called_When_I_Call_Update_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>
            {
                {"Key", "Value"}
            };

            _blaiseApiMock.Setup(d => d.MarkCaseAsComplete(It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithData(fieldData);

            //act
            _sut.Update();

            //assert
            _blaiseApiMock.Verify(v => v.UpdateDataRecord(_caseDataRecord, fieldData, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_WithData_Has_Been_Called_But_WithServerPark_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>
            {
                {"Key", "Value"}
            };

            _sut.WithInstrument(_instrumentName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_WithData_Has_Been_Called_But_WithInstrument_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>
            {
                {"Key", "Value"}
            };

            _sut.WithServerPark(_serverParkName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithInstrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_WithData_Has_Been_Called_But_WithCase_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>
            {
                {"Key", "Value"}
            };

            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.WithData(fieldData);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithDataRecord' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_A_CompleteStatus_But_WithData_Not_Called_When_I_Call_Update_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.MarkCaseAsComplete(It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithStatus(StatusType.Completed);

            //act
            _sut.Update();

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsComplete(_caseDataRecord, _instrumentName, _serverParkName), Times.Once);
            _blaiseApiMock.Verify(v => v.MarkCaseAsProcessed(It.IsAny<IDataRecord>(), It.IsAny<string>()
                , It.IsAny<string>()), Times.Never);
            _blaiseApiMock.Verify(v => v.UpdateDataRecord(It.IsAny<IDataRecord>(), It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Given_A_ProcessedStatus_But_WithData_Not_Called_When_I_Call_Update_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.MarkCaseAsProcessed(It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithStatus(StatusType.Processed);

            //act
            _sut.Update();

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsProcessed(_caseDataRecord, _instrumentName, _serverParkName), Times.Once);
            _blaiseApiMock.Verify(v => v.MarkCaseAsComplete(_caseDataRecord, _instrumentName, _serverParkName), Times.Never);
            _blaiseApiMock.Verify(v => v.UpdateDataRecord(It.IsAny<IDataRecord>(), It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestCase(StatusType.Completed)]
        [TestCase(StatusType.Processed)]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown(StatusType statusType)
        {
            //arrange
            _sut.WithInstrument(_instrumentName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithStatus(statusType);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [TestCase(StatusType.Completed)]
        [TestCase(StatusType.Processed)]
        public void Given_WithInstrument_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown(StatusType statusType)
        {
            //arrange
            _sut.WithServerPark(_serverParkName);
            _sut.WithDataRecord(_caseDataRecord);
            _sut.WithStatus(statusType);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithInstrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [TestCase(StatusType.Completed)]
        [TestCase(StatusType.Processed)]
        public void Given_WithCase_Has_Not_Been_Called_When_I_Call_Update_Then_An_NullReferenceException_Is_Thrown(StatusType statusType)
        {
            //arrange
            _sut.WithInstrument(_instrumentName);
            _sut.WithServerPark(_serverParkName);
            _sut.WithStatus(statusType);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Update());
            Assert.AreEqual("The 'WithDataRecord' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_WithCase_Is_Called_When_I_Call_Exists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(p => p.CaseExists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act
            var sutExists = _sut.Exists;

            //assert
            _blaiseApiMock.Verify(v => v.CaseExists(_primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_WithCase_Is_Called_When_I_Call_Exists_Then_The_Expected_Result_Is_Returned(bool caseExists)
        {
            //arrange

            _blaiseApiMock.Setup(p => p.CaseExists(_primaryKeyValue, _instrumentName, _serverParkName)).Returns(caseExists);

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
        public void Given_WithCase_Is_Called_But_WithInstrument_Has_Not_Been_Called_When_I_Call_Exists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            _sut.WithServerPark(_serverParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Exists;
            });
            Assert.AreEqual("The 'WithInstrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_WithCase_Is_Called_But_WithServerPark_Has_Not_Been_Called_When_I_CallExists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                var sutExists = _sut.Exists;
            });
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_Case_Has_Been_Called_When_I_Call_Remove_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act
            _sut.Remove();

            //assert
            _blaiseApiMock.Verify(v => v.RemoveCase(_primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_WithPrimaryKey_Has_Not_Been_Called_When_I_Call_Remove_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            //_sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Case.Remove());
            Assert.AreEqual("The 'WithPrimaryKey' step needs to be called prior to this to specify the primary key value of the case", exception.Message);
        }

        [Test]
        public void Given_WithInstrument_Has_Not_Been_Called_When_I_Call_Remove_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithServerPark(_serverParkName);
            //_sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Case.Remove());
            Assert.AreEqual("The 'WithInstrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_Remove_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithServerPark(_serverParkName);
            _sut.WithInstrument(_instrumentName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Case.Remove());
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }
    }
}
