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

        private Mock<IDatamodel> _dataModelMock;
        private Mock<IKey> _keyMock;
        private Mock<IDataRecord> _dataRecordMock;

        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _filePath;
        private readonly string _keyName;

        private DataService _sut;

        public DataServiceTests()
        {
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _filePath = "c:\\filePath";
            _keyName = "TestKeyName";
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();
            _keyMock = new Mock<IKey>();
            _dataRecordMock = new Mock<IDataRecord>();

            _dataModelServiceMock = new Mock<IDataModelService>();
            _dataModelServiceMock.Setup(d => d.GetDataModel(_instrumentName, _serverParkName)).Returns(_dataModelMock.Object);

            _keyServiceMock = new Mock<IKeyService>();
            _keyServiceMock.Setup(d => d.KeyExists(_keyMock.Object, _instrumentName, _serverParkName)).Returns(true);
            _keyServiceMock.Setup(d => d.GetKey(_dataModelMock.Object, _keyName)).Returns(_keyMock.Object);

            _dataRecordServiceMock = new Mock<IDataRecordService>();

            _dataRecordServiceMock.Setup(d => d.GetDataRecord(_dataModelMock.Object)).Returns(_dataRecordMock.Object);

            _sut = new DataService(
                _dataModelServiceMock.Object,
                _dataRecordServiceMock.Object,
                _keyServiceMock.Object);
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
        public void Given_I_Call_GetPrimaryKey_Then_The_Correct_Key_Is_Returned()
        {
            //act
            var primaryKey = "Key1";
            _keyServiceMock.Setup(k => k.GetPrimaryKey(It.IsAny<IDataRecord>())).Returns(primaryKey);

            var result = _sut.GetPrimaryKey(_dataRecordMock.Object);

            //assert
            Assert.AreSame(primaryKey, result);
        }

        [Test]
        public void Given_I_Call_GetPrimaryKey_Then_The_Correct_Services_Are_Called()
        {
            //act
            _keyServiceMock.Setup(k => k.GetPrimaryKey(It.IsAny<IDataRecord>())).Returns(It.IsAny<string>());
            _sut.GetPrimaryKey(_dataRecordMock.Object);

            //assert
            _keyServiceMock.Verify(v => v.GetPrimaryKey(_dataRecordMock.Object), Times.Once);
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
        public void Given_I_Call_GetDataRecord_Then_A_DataRecord_Is_Returned()
        {
            //act
            var result = _sut.GetDataRecord(_dataModelMock.Object);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDataRecord>(result);
        }

        [Test]
        public void Given_I_Call_GetDataRecord_Then_The_Correct_DataRecord_Is_Returned()
        {
            //act
            var result = _sut.GetDataRecord(_dataModelMock.Object);

            //assert
            Assert.AreSame(_dataRecordMock.Object, result);
        }

        [Test]
        public void Given_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataRecord(_dataModelMock.Object);

            //assert
            _dataRecordServiceMock.Verify(v => v.GetDataRecord(_dataModelMock.Object), Times.Once);
        }

        [Test]
        public void Given_An_InstrumentName_And_ServerParkName_When_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataRecord(_keyMock.Object, _instrumentName, _serverParkName);

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
    }
}
