using Blaise.Nuget.Api.Core.Interfaces.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using StatNeth.Blaise.API.ServerManager;

namespace Blaise.Nuget.Api.Tests.Unit
{
    public class BlaiseApiTests
    {
        private Mock<IDataService> _dataServiceMock;
        private Mock<IParkService> _parkServiceMock;

        private BlaiseApi _sut;

        [SetUp]
        public void SetUpTests()
        {
            _dataServiceMock = new Mock<IDataService>();
            _parkServiceMock = new Mock<IParkService>();

            _sut = new BlaiseApi(
                _dataServiceMock.Object,
                _parkServiceMock.Object);
        }

        [Test]
        public void Given_I_Instantiate_BlaiseApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            Assert.DoesNotThrow(() => new BlaiseApi());
        }

        [Test]
        public void When_I_Call_GetServerParkNames_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _parkServiceMock.Setup(p => p.GetServerParkNames()).Returns(It.IsAny<List<string>>());

            //act
            _sut.GetServerParkNames();

            //assert
            _parkServiceMock.Verify(v => v.GetServerParkNames());
        }

        [Test]
        public void When_I_Call_GetServerParkNames_Then_The_Expected_Server_Park_Names_Are_Returned()
        {
            //arrange
            var serverParksNames = new List<string> { "Park1", "Park2" };

            _parkServiceMock.Setup(p => p.GetServerParkNames()).Returns(serverParksNames);

            //act
            var result = _sut.GetServerParkNames();

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(serverParksNames, result);
        }

        [Test]
        public void When_I_Call_GetAllSurveys_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _parkServiceMock.Setup(p => p.GetAllSurveys()).Returns(It.IsAny<List<ISurvey>>());

            //act
            _sut.GetAllSurveys();

            //assert
            _parkServiceMock.Verify(v => v.GetAllSurveys());
        }

        [Test]
        public void When_I_Call_GetAllSurveys_Then_The_Expected_Surveys_Are_Returned()
        {
            //arrange
            var survey1Mock = new Mock<ISurvey>();
            var survey2Mock = new Mock<ISurvey>();
            var survey3Mock = new Mock<ISurvey>();

            var surveys = new List<ISurvey> { survey1Mock.Object, survey2Mock.Object, survey3Mock.Object };

            _parkServiceMock.Setup(p => p.GetAllSurveys()).Returns(surveys);

            //act
            var result = _sut.GetAllSurveys().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(3, result.Count);
            Assert.True(result.Contains(survey1Mock.Object));
            Assert.True(result.Contains(survey2Mock.Object));
            Assert.True(result.Contains(survey3Mock.Object));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveyNames_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "Park1";

            _parkServiceMock.Setup(p => p.GetSurveyNames(It.IsAny<string>())).Returns(It.IsAny<List<string>>());

            //act
            _sut.GetSurveyNames(serverParkName);

            //assert
            _parkServiceMock.Verify(v => v.GetSurveyNames(serverParkName));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveyNames_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            const string serverParkName = "Park1";
            var surveyList = new List<string>
            {
                "Instrument1",
                "Instrument2"
            };

            _parkServiceMock.Setup(p => p.GetSurveyNames(serverParkName)).Returns(surveyList);

            //act            
            var result = _sut.GetSurveyNames(serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.True(result.Contains("Instrument1"));
            Assert.True(result.Contains("Instrument2"));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetSurveyNames_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetSurveyNames(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetSurveyNames_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveyNames(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveys_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "Park1";

            _parkServiceMock.Setup(p => p.GetSurveys(It.IsAny<string>())).Returns(It.IsAny<List<ISurvey>>());

            //act
            _sut.GetSurveys(serverParkName);

            //assert
            _parkServiceMock.Verify(v => v.GetSurveys(serverParkName));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveys_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            const string serverParkName = "Park1";
            var survey1Mock = new Mock<ISurvey>();
            var survey2Mock = new Mock<ISurvey>();
            var surveyList = new List<ISurvey>
            {
                survey1Mock.Object,
                survey2Mock.Object
            };

            _parkServiceMock.Setup(p => p.GetSurveys(serverParkName)).Returns(surveyList);

            //act            
            var result = _sut.GetSurveys(serverParkName).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.True(result.Contains(survey1Mock.Object));
            Assert.True(result.Contains(survey2Mock.Object));
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetSurveys_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetSurveys(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetSurveys_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetSurveys(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_ServerParkExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "Park1";

            _parkServiceMock.Setup(p => p.ServerParkExists(It.IsAny<string>())).Returns(It.IsAny<bool>());

            //act
            _sut.ServerParkExists(serverParkName);

            //assert
            _parkServiceMock.Verify(v => v.ServerParkExists(serverParkName));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_ServerParkExists_Then_The_Expected_Result_Is_Returned(bool serverParkExists)
        {
            //arrange
            var serverParkName = "Park1";

            _parkServiceMock.Setup(p => p.ServerParkExists(serverParkName)).Returns(serverParkExists);

            //act
            var result = _sut.ServerParkExists(serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(serverParkExists, result);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_ServerParkExists_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ServerParkExists(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_ServerParkExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ServerParkExists(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetInstrumentId_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _parkServiceMock.Setup(p => p.GetInstrumentId(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());

            //act
            _sut.GetInstrumentId(instrumentName, serverParkName);

            //assert
            _parkServiceMock.Verify(v => v.GetInstrumentId(instrumentName, serverParkName));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetInstrumentId_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";
            var instrumentId = Guid.NewGuid();

            _parkServiceMock.Setup(p => p.GetInstrumentId(instrumentName, serverParkName)).Returns(instrumentId);

            //act
            var result = _sut.GetInstrumentId(instrumentName, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(instrumentId, result);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetInstrumentId_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstrumentId(string.Empty, serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetInstrumentId_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstrumentId(null, serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetInstrumentId_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetInstrumentId(instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetInstrumentId_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetInstrumentId(instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Instrument_AndServerPark_When_I_Call_GetDataModel_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _dataServiceMock.Setup(d => d.GetDataModel(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IDatamodel>());

            //act
            _sut.GetDataModel(instrumentName, serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.GetDataModel(instrumentName, serverParkName));
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDataModel_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";
            var dataModelMock = new Mock<IDatamodel>();

            _dataServiceMock.Setup(d => d.GetDataModel(instrumentName, serverParkName)).Returns(dataModelMock.Object);

            //act
            var result = _sut.GetDataModel(instrumentName, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(dataModelMock.Object, result);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetDataModel_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataModel(string.Empty, serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetDataModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataModel(null, serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetDataModel_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataModel(instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetDataModel_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataModel(instrumentName, null));
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
            _dataServiceMock.Verify(v => v.GetKey(dataModelMock.Object, keyName));
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
        public void Given_Valid_Arguments_When_I_Call_KeyExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _dataServiceMock.Setup(d => d.KeyExists(It.IsAny<IKey>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            //act
            _sut.KeyExists(keyMock.Object, instrumentName, serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.KeyExists(keyMock.Object, instrumentName, serverParkName));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_KeyExists_Then_The_Expected_Result_Is_Returned(bool keyExists)
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _dataServiceMock.Setup(d => d.KeyExists(keyMock.Object, instrumentName, serverParkName)).Returns(keyExists);

            //act
            var result = _sut.KeyExists(keyMock.Object, instrumentName, serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(keyExists, result);
        }

        [Test]
        public void Given_A_Null_Key_When_I_Call_KeyExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.KeyExists(null, instrumentName, serverParkName));
            Assert.AreEqual("The argument 'key' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_KeyExists_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.KeyExists(keyMock.Object, string.Empty, serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_KeyExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.KeyExists(keyMock.Object, null, serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_KeyExists_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.KeyExists(keyMock.Object, instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_KeyExists_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.KeyExists(keyMock.Object, instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
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
            _dataServiceMock.Verify(v => v.GetDataRecord(dataModelMock.Object));
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
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _dataServiceMock.Setup(d => d.GetDataSet(It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetDataSet(instrumentName, serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.GetDataSet(instrumentName, serverParkName));
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetDataSet_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataSet(string.Empty, serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetDataSet_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataSet(null, serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetDataSet_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataSet(instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetDataSet_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataSet(instrumentName, null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _dataServiceMock.Setup(d => d.GetDataRecord(It.IsAny<IKey>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.GetDataRecord(keyMock.Object, instrumentName, serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.GetDataRecord(keyMock.Object, instrumentName, serverParkName));
        }

        [Test]
        public void Given_A_Null_Key_When_I_Call_GetDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(null, instrumentName, serverParkName));
            Assert.AreEqual("The argument 'key' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_GetDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataRecord(keyMock.Object, string.Empty, serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_GetDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(keyMock.Object, null, serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_GetDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.GetDataRecord(keyMock.Object, instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_GetDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord(keyMock.Object, instrumentName, null));
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
            _dataServiceMock.Verify(v => v.GetDataRecord(keyMock.Object, filePath));
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
        public void Given_Valid_Arguments_When_I_Call_WriteDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _dataServiceMock.Setup(d => d.WriteDataRecord(It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            //act
            _sut.WriteDataRecord(dataRecordMock.Object, instrumentName, serverParkName);

            //assert
            _dataServiceMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, instrumentName, serverParkName));
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_WriteDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WriteDataRecord(null, instrumentName, serverParkName));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_InstrumentName_When_I_Call_WriteDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.WriteDataRecord(dataRecordMock.Object, string.Empty, serverParkName));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_InstrumentName_When_I_Call_WriteDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var serverParkName = "Park1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WriteDataRecord(dataRecordMock.Object, null, serverParkName));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_WriteDataRecord_Then_An_ArgumentException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.WriteDataRecord(dataRecordMock.Object, instrumentName, string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_WriteDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //arrange 
            var dataRecordMock = new Mock<IDataRecord>();
            var instrumentName = "Instrument1";

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WriteDataRecord(dataRecordMock.Object, instrumentName, null));
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
            _dataServiceMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, filePath));
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
    }
}
