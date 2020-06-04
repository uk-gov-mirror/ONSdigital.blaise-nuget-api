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
        public void Given_I_Call_Case_Then_It_Returns_Same_Instance_Of_Itself_Back()
        {
            //act
            var result = _sut.Case();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IFluentBlaiseCaseApi>(result);
            Assert.AreSame(_sut, result);
        }

        [Test]
        public void Given_A_Primary_Key_Value_When_I_Call_Case_Then_It_Returns_Same_Instance_Of_Itself_Back()
        {
            //act
            var result = _sut.Case(_primaryKeyValue);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IFluentBlaiseCaseApi>(result);
            Assert.AreSame(_sut, result);
        }

        [Test]
        public void Given_A_Case_Data_Record_When_I_Call_Case_Then_It_Returns_Same_Instance_Of_Itself_Back()
        {
            //act
            var result = _sut.Case(_caseDataRecord);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IFluentBlaiseCaseApi>(result);
            Assert.AreSame(_sut, result);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_Add_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _blaiseApiMock.Setup(d => d.CreateNewDataRecord(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);
            _sut.Case(_primaryKeyValue);

            //act
            _sut.Add(fieldData);

            //assert
            _blaiseApiMock.Verify(v => v.CreateNewDataRecord(_primaryKeyValue, fieldData, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_ServerPark_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.Instrument(_instrumentName);
            _sut.Case(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add(fieldData));
            Assert.AreEqual("The 'ServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_Instrument_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.ServerPark(_serverParkName);
            _sut.Case(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add(fieldData));
            Assert.AreEqual("The 'Instrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_Add_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Add(fieldData));
            Assert.AreEqual("The 'Case' step needs to be called prior to this to specify the primary key value of the case", exception.Message);
        }


        [Test]
        public void Given_Valid_Arguments_When_I_Call_Cases_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(d => d.GetDataSet(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IDataSet>());

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);

            //act
            _sut.Cases();

            //assert
            _blaiseApiMock.Verify(v => v.GetDataSet(_instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_Cases_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataSetMock = new Mock<IDataSet>();

            _blaiseApiMock.Setup(d => d.GetDataSet(_instrumentName, _serverParkName)).Returns(dataSetMock.Object);

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);

            //act
            var result = _sut.Cases();

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(dataSetMock.Object, result);
        }

        [Test]
        public void Given_ServerPark_Has_Not_Been_Called_When_I_Call_Cases_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.Instrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Cases());
            Assert.AreEqual("The 'ServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_Instrument_Has_Not_Been_Called_When_I_Call_Cases_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.ServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Cases());
            Assert.AreEqual("The 'Instrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }


        [Test]
        public void Given_Valid_Arguments_When_I_Call_PrimaryKeyValue_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(d => d.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(It.IsAny<string>());

            _sut.Case(_caseDataRecord);

            //act
            _sut.PrimaryKeyValue();

            //assert
            _blaiseApiMock.Verify(v => v.GetPrimaryKeyValue(_caseDataRecord), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_PrimaryKeyValue_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.GetPrimaryKeyValue(It.IsAny<IDataRecord>())).Returns(_primaryKeyValue);

            _sut.Case(_caseDataRecord);

            //act
            var result = _sut.PrimaryKeyValue();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_primaryKeyValue, result);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_PrimaryKeyValue_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.PrimaryKeyValue());
            Assert.AreEqual("The 'Case' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_Case_Has_Been_Called_When_I_Call_IsComplete_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(It.IsAny<bool>());

            _sut.Case(_caseDataRecord);

            //act
            _sut.IsComplete();

            //assert
            _blaiseApiMock.Verify(v => v.CaseHasBeenCompleted(_caseDataRecord), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Case_Has_Been_Called_When_I_Call_IsComplete_Then_The_Expected_Result_Is_Returned(bool caseIsComplete)
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenCompleted(_caseDataRecord)).Returns(caseIsComplete);

            _sut.Case(_caseDataRecord);
            //act
            var result = _sut.IsComplete();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseIsComplete, result);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_IsComplete_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.IsComplete());
            Assert.AreEqual("The 'Case' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_Case_Has_Been_Called_When_I_Call_HasBeenProcessed_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenProcessed(It.IsAny<IDataRecord>())).Returns(It.IsAny<bool>());

            _sut.Case(_caseDataRecord);

            //act
            _sut.HasBeenProcessed();

            //assert
            _blaiseApiMock.Verify(v => v.CaseHasBeenProcessed(_caseDataRecord), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Case_Has_Been_Called_When_I_Call_HasBeenProcessed_Then_The_Expected_Result_Is_Returned(bool caseHasBeenProcessed)
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenProcessed(_caseDataRecord)).Returns(caseHasBeenProcessed);

            _sut.Case(_caseDataRecord);
            //act
            var result = _sut.HasBeenProcessed();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseHasBeenProcessed, result);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_HasBeenProcessed_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.HasBeenProcessed());
            Assert.AreEqual("The 'Case' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_A_CompleteStatus_When_I_Call_SetStatusAs_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.MarkCaseAsComplete(It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);
            _sut.Case(_caseDataRecord);

            //act
            _sut.SetStatusAs(StatusType.Completed);

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsComplete(_caseDataRecord, _instrumentName, _serverParkName), Times.Once);
            _blaiseApiMock.Verify(v => v.MarkCaseAsProcessed(_caseDataRecord, _instrumentName, _serverParkName), Times.Never);
        }

        [Test]
        public void Given_A_ProcessedStatus_When_I_Call_SetStatusAs_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.MarkCaseAsProcessed(It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);
            _sut.Case(_caseDataRecord);

            //act
            _sut.SetStatusAs(StatusType.Processed);

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsProcessed(_caseDataRecord, _instrumentName, _serverParkName), Times.Once);
            _blaiseApiMock.Verify(v => v.MarkCaseAsComplete(_caseDataRecord, _instrumentName, _serverParkName), Times.Never);
        }

        [TestCase(StatusType.Completed)]
        [TestCase(StatusType.Processed)]
        public void Given_ServerPark_Has_Not_Been_Called_When_I_Call_SetStatusAs_Then_An_NullReferenceException_Is_Thrown(StatusType statusType)
        {
            //arrange
            _sut.Instrument(_instrumentName);
            _sut.Case(_caseDataRecord);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.SetStatusAs(statusType));
            Assert.AreEqual("The 'ServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [TestCase(StatusType.Completed)]
        [TestCase(StatusType.Processed)]
        public void Given_Instrument_Has_Not_Been_Called_When_I_Call_SetStatusAs_Then_An_NullReferenceException_Is_Thrown(StatusType statusType)
        {
            //arrange
            _sut.ServerPark(_serverParkName);
            _sut.Case(_caseDataRecord);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.SetStatusAs(statusType));
            Assert.AreEqual("The 'Instrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [TestCase(StatusType.Completed)]
        [TestCase(StatusType.Processed)]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_SetStatusAs_Then_An_NullReferenceException_Is_Thrown(StatusType statusType)
        {
            //arrange
            _sut.Instrument(_instrumentName);
            _sut.ServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.SetStatusAs(statusType));
            Assert.AreEqual("The 'Case' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_Case_Is_Called_When_I_Call_CaseExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(p => p.CaseExists(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);
            _sut.Case(_primaryKeyValue);

            //act
            _sut.Exists();

            //assert
            _blaiseApiMock.Verify(v => v.CaseExists(_primaryKeyValue, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Case_Is_Called_When_I_Call_CaseExists_Then_The_Expected_Result_Is_Returned(bool caseExists)
        {
            //arrange

            _blaiseApiMock.Setup(p => p.CaseExists(_primaryKeyValue, _instrumentName, _serverParkName)).Returns(caseExists);

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);
            _sut.Case(_primaryKeyValue);

            //act
            var result = _sut.Exists();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseExists, result);
        }

        [Test]
        public void Given_Case_Is_Called_But_Instrument_Has_Not_Been_Called_When_I_Call_CaseExists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            _sut.ServerPark(_serverParkName);
            _sut.Case(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Exists());
            Assert.AreEqual("The 'Instrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_Case_Is_Called_But_ServerPark_Has_Not_Been_Called_When_I_Call_CaseExists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            _sut.Instrument(_instrumentName);
            _sut.Case(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Exists());
            Assert.AreEqual("The 'ServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }
    }
}
