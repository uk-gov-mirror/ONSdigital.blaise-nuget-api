using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blaise.Nuget.Api.Tests.Unit
{
    public class FluentBlaiseApiTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private FluentBlaiseApi _sut;

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();

            _sut = new FluentBlaiseApi(_blaiseApiMock.Object);
        }

        [Test]
        public void Given_I_Instantiate_FluentBlaiseApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            Assert.DoesNotThrow(() => new FluentBlaiseApi());
        }

        [Test]
        public void When_I_Call_GetServerParkNames_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            _blaiseApiMock.Setup(p => p.GetServerParkNames()).Returns(It.IsAny<List<string>>());

            //act
            _sut.GetServerParkNames();

            //assert
            _blaiseApiMock.Verify(v => v.GetServerParkNames(), Times.Once);
        }

        [Test]
        public void When_I_Call_GetServerParkNames_Then_The_Expected_Server_Park_Names_Are_Returned()
        {
            //arrange
            var serverParksNames = new List<string> { "Park1", "Park2" };

            _blaiseApiMock.Setup(p => p.GetServerParkNames()).Returns(serverParksNames);

            //act
            var result = _sut.GetServerParkNames();

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(serverParksNames, result);
        }


        [Test]
        public void Given_Valid_Arguments_When_I_Call_ServerParkExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "Park1";

            _blaiseApiMock.Setup(p => p.ServerParkExists(It.IsAny<string>())).Returns(It.IsAny<bool>());

            //act
            _sut.ServerParkExists(serverParkName);

            //assert
            _blaiseApiMock.Verify(v => v.ServerParkExists(serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_ServerParkExists_Then_The_Expected_Result_Is_Returned(bool serverParkExists)
        {
            //arrange
            var serverParkName = "Park1";

            _blaiseApiMock.Setup(p => p.ServerParkExists(serverParkName)).Returns(serverParkExists);

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
        public void Given_Valid_Arguments_When_I_Call_GetKey_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            var keyName = "Key1";

            _blaiseApiMock.Setup(d => d.GetKey(It.IsAny<IDatamodel>(), It.IsAny<string>())).Returns(It.IsAny<IKey>());

            //act
            _sut.GetKey(dataModelMock.Object, keyName);

            //assert
            _blaiseApiMock.Verify(v => v.GetKey(dataModelMock.Object, keyName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetKey_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            var keyName = "Key1";
            var keyMock = new Mock<IKey>();

            _blaiseApiMock.Setup(d => d.GetKey(dataModelMock.Object, keyName)).Returns(keyMock.Object);

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
        public void Given_A_Valid_DataRecord_When_I_Call_GetDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();

            _blaiseApiMock.Setup(d => d.GetDataRecord(It.IsAny<IDatamodel>())).Returns(It.IsAny<IDataRecord>());

            //act
            _sut.GetDataRecord(dataModelMock.Object);

            //assert
            _blaiseApiMock.Verify(v => v.GetDataRecord(dataModelMock.Object), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDataRecord_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            var dataRecordMock = new Mock<IDataRecord>();

            _blaiseApiMock.Setup(d => d.GetDataRecord(dataModelMock.Object)).Returns(dataRecordMock.Object);

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
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord((IDatamodel)null));
            Assert.AreEqual("The argument 'dataModel' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_WithServerPark_Then_It_Returns_Same_Instance_Of_Itself_Back()
        {
            //arrange
            var serverParkName = "Park1";

            //act
            var result = _sut.WithServerPark(serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IFluentBlaiseRemoteApi>(result);
            Assert.AreSame(_sut, result);
        }

        [Test]
        public void Given_An_Empty_ServerParkName_When_I_Call_WithServerPark_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.WithServerPark(string.Empty));
            Assert.AreEqual("A value for the argument 'serverParkName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ServerParkName_When_I_Call_WithServerPark_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WithServerPark(null));
            Assert.AreEqual("serverParkName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveys_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "Park1";
            var instrumentName = "Instrument1";

            _blaiseApiMock.Setup(p => p.GetSurveys(It.IsAny<string>())).Returns(It.IsAny<List<string>>());

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act
            _sut.GetSurveys();

            //assert
            _blaiseApiMock.Verify(v => v.GetSurveys(serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetSurveys_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var serverParkName = "Park1";
            var instrumentName = "Instrument1";
            var surveyList = new List<string>
            {
                "Instrument1",
                "Instrument2"
            };

            _blaiseApiMock.Setup(p => p.GetSurveys(serverParkName)).Returns(surveyList);

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act            
            var result = _sut.GetSurveys().ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.True(result.Contains("Instrument1"));
            Assert.True(result.Contains("Instrument2"));
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_GetSurveys_Then_An_NullReferenceException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetSurveys());
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_ForInstrument_Then_It_Returns_Same_Instance_Of_Itself_Back()
        {
            //arrange
            var instrumentName = "Instrument1";

            //act
            var result = _sut.ForInstrument(instrumentName);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IFluentBlaiseRemoteApi>(result);
            Assert.AreSame(_sut, result);
        }

        [Test]
        public void Given_An_Empty_ForInstrument_When_I_Call_ForInstrument_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.ForInstrument(string.Empty));
            Assert.AreEqual("A value for the argument 'instrumentName' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_ForInstrument_When_I_Call_ForInstrument_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.ForInstrument(null));
            Assert.AreEqual("instrumentName", exception.ParamName);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetInstrumentId_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var serverParkName = "Park1";
            var instrumentName = "Instrument1";

            _blaiseApiMock.Setup(p => p.GetInstrumentId(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<Guid>());

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act
            _sut.GetInstrumentId();

            //assert
            _blaiseApiMock.Verify(v => v.GetInstrumentId(instrumentName, serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetInstrumentId_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var serverParkName = "Park1";
            var instrumentName = "Instrument1";
            var instrumentId = Guid.NewGuid();

            _blaiseApiMock.Setup(p => p.GetInstrumentId(instrumentName, serverParkName)).Returns(instrumentId);

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act
            var result = _sut.GetInstrumentId();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(instrumentId, result);
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_GetInstrumentId_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var instrumentName = "Instrument1";

          
            _sut.ForInstrument(instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetInstrumentId());
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_ForInstrument_Has_Not_Been_Called_When_I_Call_GetInstrumentId_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";

            _sut.WithServerPark(serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetInstrumentId());
            Assert.AreEqual("The 'ForInstrument' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_Valid_Instrument_And_ServerPark_When_I_Call_GetDataModel_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _blaiseApiMock.Setup(d => d.GetDataModel(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IDatamodel>());

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act
            _sut.GetDataModel();

            //assert
            _blaiseApiMock.Verify(v => v.GetDataModel(instrumentName, serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDataModel_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";
            var dataModelMock = new Mock<IDatamodel>();

            _blaiseApiMock.Setup(d => d.GetDataModel(instrumentName, serverParkName)).Returns(dataModelMock.Object);

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act
            var result = _sut.GetDataModel();

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(dataModelMock.Object, result);
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_GetDataModel_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var instrumentName = "Instrument1";

            _sut.ForInstrument(instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetDataModel());
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_ForInstrument_Has_Not_Been_Called_When_I_Call_GetDataModel_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";

            _sut.WithServerPark(serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetDataModel());
            Assert.AreEqual("The 'ForInstrument' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDataSet_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _blaiseApiMock.Setup(d => d.GetDataSet(It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<IDataSet>());

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act
            _sut.GetDataSet();

            //assert
            _blaiseApiMock.Verify(v => v.GetDataSet(instrumentName, serverParkName), Times.Once);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_GetDataSet_Then_The_Expected_Result_Is_Returned()
        {
            //arrange
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";
            var dataSetMock = new Mock<IDataSet>();

            _blaiseApiMock.Setup(d => d.GetDataSet(instrumentName, serverParkName)).Returns(dataSetMock.Object);

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act
            var result = _sut.GetDataSet();

            //assert
            Assert.IsNotNull(result);
            Assert.AreSame(dataSetMock.Object, result);
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_GetDataSet_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var instrumentName = "Instrument1";

            _sut.ForInstrument(instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetDataSet());
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_ForInstrument_Has_Not_Been_Called_When_I_Call_GetDataSet_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var serverParkName = "Park1";

            _sut.WithServerPark(serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetDataSet());
            Assert.AreEqual("The 'ForInstrument' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_KeyExists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _blaiseApiMock.Setup(d => d.KeyExists(It.IsAny<IKey>(), It.IsAny<string>(), It.IsAny<string>())).Returns(It.IsAny<bool>());

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act
            _sut.KeyExists(keyMock.Object);

            //assert
            _blaiseApiMock.Verify(v => v.KeyExists(keyMock.Object, instrumentName, serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Valid_Arguments_When_I_Call_KeyExists_Then_The_Expected_Result_Is_Returned(bool keyExists)
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _blaiseApiMock.Setup(d => d.KeyExists(keyMock.Object, instrumentName, serverParkName)).Returns(keyExists);

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act
            var result = _sut.KeyExists(keyMock.Object);

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

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.KeyExists(null));
            Assert.AreEqual("The argument 'key' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_WithServerPark_Has_Not_Been_Called_When_I_Call_KeyExists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";

            _sut.ForInstrument(instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.KeyExists(keyMock.Object));
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_ForInstrument_Has_Not_Been_Called_When_I_Call_KeyExists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var serverParkName = "Park1";

            _sut.WithServerPark(serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.KeyExists(keyMock.Object));
            Assert.AreEqual("The 'ForInstrument' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_Valid_Arguments_When_I_Call_WithFile_Then_It_Returns_Same_Instance_Of_Itself_Back()
        {
            //arrange
            var filePath = "File1";

            //act
            var result = _sut.WithFile(filePath);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IFluentBlaiseLocalApi>(result);
            Assert.AreSame(_sut, result);
        }

        [Test]
        public void Given_An_Empty_FilePath_When_I_Call_WithFile_Then_An_ArgumentException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentException>(() => _sut.WithFile(string.Empty));
            Assert.AreEqual("A value for the argument 'filePath' must be supplied", exception.Message);
        }

        [Test]
        public void Given_A_Null_FilePath_When_I_Call_WithFile_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WithFile(null));
            Assert.AreEqual("filePath", exception.ParamName);
        }

        [Test]
        public void Given_WithFile_Has_Been_Called_When_I_Call_GetDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var filePath = "File1";

            _blaiseApiMock.Setup(d => d.GetDataRecord(It.IsAny<IKey>(), It.IsAny<string>()));

            _sut.WithFile(filePath);

            //act
            _sut.GetDataRecord(keyMock.Object);

            //assert
            _blaiseApiMock.Verify(v => v.GetDataRecord(keyMock.Object, filePath), Times.Once);
            _blaiseApiMock.Verify(v => v.GetDataRecord(It.IsAny<IKey>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Given_WithServerPark_And_ForInstrument_Has_Been_Called_When_I_Call_GetDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _blaiseApiMock.Setup(d => d.GetDataRecord(It.IsAny<IKey>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act
            _sut.GetDataRecord(keyMock.Object);

            //assert
            _blaiseApiMock.Verify(v => v.GetDataRecord(keyMock.Object, instrumentName, serverParkName), Times.Once);
            _blaiseApiMock.Verify(v => v.GetDataRecord(It.IsAny<IKey>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Given_A_Null_Key_When_I_Call_GetDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.GetDataRecord((IKey)null));
            Assert.AreEqual("The argument 'key' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_WithFile_Has_Not_Been_Called_And_WithServerPark_Has_Not_Been_Called_When_I_Call_GetDataRecord_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var instrumentName = "Instrument1";

            _sut.ForInstrument(instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetDataRecord(keyMock.Object));
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_WithServerPark_Has_Been_Called_And_ForInstrument_Has_Not_Been_Called_When_I_Call_GetDataRecord_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var keyMock = new Mock<IKey>();
            var serverParkName = "Park1";

            _sut.WithServerPark(serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.GetDataRecord(keyMock.Object));
            Assert.AreEqual("The 'ForInstrument' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_WithFile_Has_Been_Called_When_I_Call_WriteDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var filePath = "File1";

            _blaiseApiMock.Setup(d => d.WriteDataRecord(It.IsAny<IDataRecord>(), It.IsAny<string>()));

            _sut.WithFile(filePath);

            //act
            _sut.WriteDataRecord(dataRecordMock.Object);

            //assert
            _blaiseApiMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, filePath), Times.Once);
            _blaiseApiMock.Verify(v => v.WriteDataRecord(It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Given_WithServerPark_And_ForInstrument_Has_Been_Called_When_I_Call_WriteDataRecord_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var instrumentName = "Instrument1";
            var serverParkName = "Park1";

            _blaiseApiMock.Setup(d => d.WriteDataRecord(It.IsAny<IDataRecord>(), It.IsAny<string>(), It.IsAny<string>()));

            _sut.WithServerPark(serverParkName);
            _sut.ForInstrument(instrumentName);

            //act
            _sut.WriteDataRecord(dataRecordMock.Object);

            //assert
            _blaiseApiMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, instrumentName, serverParkName), Times.Once);
            _blaiseApiMock.Verify(v => v.WriteDataRecord(It.IsAny<IDataRecord>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void Given_A_Null_DataRecord_When_I_Call_WriteDataRecord_Then_An_ArgumentNullException_Is_Thrown()
        {
            //act && assert
            var exception = Assert.Throws<ArgumentNullException>(() => _sut.WriteDataRecord(null));
            Assert.AreEqual("The argument 'dataRecord' must be supplied", exception.ParamName);
        }

        [Test]
        public void Given_WithFile_Has_Not_Been_Called_And_WithServerPark_Has_Not_Been_Called_When_I_Call_WriteDataRecord_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var instrumentName = "Instrument1";

            _sut.ForInstrument(instrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.WriteDataRecord(dataRecordMock.Object));
            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this", exception.Message);
        }

        [Test]
        public void Given_WithServerPark_Has_Been_Called_And_ForInstrument_Has_Not_Been_Called_When_I_Call_WriteDataRecord_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            var dataRecordMock = new Mock<IDataRecord>();
            var serverParkName = "Park1";

            _sut.WithServerPark(serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.WriteDataRecord(dataRecordMock.Object));
            Assert.AreEqual("The 'ForInstrument' step needs to be called prior to this", exception.Message);
        }

    }
}
