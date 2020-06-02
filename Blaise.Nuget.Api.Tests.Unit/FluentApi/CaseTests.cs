using System;
using System.Collections.Generic;
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
        public void Given_Valid_Arguments_When_I_Call_Create_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _blaiseApiMock.Setup(d => d.CreateNewDataRecord(It.IsAny<string>(), It.IsAny<Dictionary<string, string>>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);
            _sut.Case(_primaryKeyValue);

            //act
            _sut.Create(fieldData);

            //assert
            _blaiseApiMock.Verify(v => v.CreateNewDataRecord(_primaryKeyValue, fieldData, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_ServerPark_Has_Not_Been_Called_When_I_Call_Create_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.Instrument(_instrumentName);
            _sut.Case(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Create(fieldData));
            Assert.AreEqual("The 'ServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_Instrument_Has_Not_Been_Called_When_I_Call_Create_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.ServerPark(_serverParkName);
            _sut.Case(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Create(fieldData));
            Assert.AreEqual("The 'Instrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_Create_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var fieldData = new Dictionary<string, string>();

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Create(fieldData));
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
        public void Given_Case_Has_Been_Called_When_I_Call_Completed_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenCompleted(It.IsAny<IDataRecord>())).Returns(It.IsAny<bool>());

            _sut.Case(_caseDataRecord);

            //act
            _sut.Completed();

            //assert
            _blaiseApiMock.Verify(v => v.CaseHasBeenCompleted(_caseDataRecord), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Case_Has_Been_Called_When_I_Call_Completed_Then_The_Expected_Result_Is_Returned(bool caseIsComplete)
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenCompleted(_caseDataRecord)).Returns(caseIsComplete);

            _sut.Case(_caseDataRecord);
            //act
            var result = _sut.Completed();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseIsComplete, result);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_Completed_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Completed());
            Assert.AreEqual("The 'Case' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_Case_Has_Been_Called_When_I_Call_Processed_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenProcessed(It.IsAny<IDataRecord>())).Returns(It.IsAny<bool>());

            _sut.Case(_caseDataRecord);

            //act
            _sut.Processed();

            //assert
            _blaiseApiMock.Verify(v => v.CaseHasBeenProcessed(_caseDataRecord), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Case_Has_Been_Called_When_I_Call_Processed_Then_The_Expected_Result_Is_Returned(bool caseHasBeenProcessed)
        {
            //arrange
            _blaiseApiMock.Setup(d => d.CaseHasBeenProcessed(_caseDataRecord)).Returns(caseHasBeenProcessed);

            _sut.Case(_caseDataRecord);
            //act
            var result = _sut.Processed();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(caseHasBeenProcessed, result);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_Processed_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Processed());
            Assert.AreEqual("The 'Case' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_The_Necessary_Steps_Has_Been_Called_When_I_Call_MarkAsComplete_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.MarkCaseAsComplete(It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);
            _sut.Case(_caseDataRecord);

            //act
            _sut.MarkAsComplete();

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsComplete(_caseDataRecord, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_ServerPark_Has_Not_Been_Called_When_I_Call_MarkAsComplete_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.Instrument(_instrumentName);
            _sut.Case(_caseDataRecord);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.MarkAsComplete());
            Assert.AreEqual("The 'ServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_Instrument_Has_Not_Been_Called_When_I_Call_MarkAsComplete_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.ServerPark(_serverParkName);
            _sut.Case(_caseDataRecord);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.MarkAsComplete());
            Assert.AreEqual("The 'Instrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_MarkAsComplete_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.Instrument(_instrumentName);
            _sut.ServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.MarkAsComplete());
            Assert.AreEqual("The 'Case' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }

        [Test]
        public void Given_The_Necessary_Steps_Has_Been_Called_When_I_Call_MarkAsProcessed_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(d => d.MarkCaseAsProcessed(It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);
            _sut.Case(_caseDataRecord);

            //act
            _sut.MarkAsProcessed();

            //assert
            _blaiseApiMock.Verify(v => v.MarkCaseAsProcessed(_caseDataRecord, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_ServerPark_Has_Not_Been_Called_When_I_Call_MarkAsProcessed_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.Instrument(_instrumentName);
            _sut.Case(_caseDataRecord);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.MarkAsProcessed());
            Assert.AreEqual("The 'ServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_Instrument_Has_Not_Been_Called_When_I_Call_MarkAsProcessed_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.ServerPark(_serverParkName);
            _sut.Case(_caseDataRecord);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.MarkAsProcessed());
            Assert.AreEqual("The 'Instrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_Case_Has_Not_Been_Called_When_I_Call_MarkAsProcessed_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.Instrument(_instrumentName);
            _sut.ServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.MarkAsProcessed());
            Assert.AreEqual("The 'Case' step needs to be called prior to this to specify the data record of the case", exception.Message);
        }
    }
}
