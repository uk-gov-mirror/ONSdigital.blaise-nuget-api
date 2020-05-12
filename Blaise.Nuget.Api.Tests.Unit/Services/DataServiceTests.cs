using Blaise.Nuget.Api.Core.Interfaces;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class DataServiceTests
    {
        private Mock<IDataLinkService> _dataLinkServiceMock;
        private Mock<IDataManagerService> _dataManagerServiceMock;

        private Mock<IDatamodel> _dataModelMock;
        private Mock<IKey> _keyMock;
        private Mock<IDataRecord> _dataRecordMock;

        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _keyName;

        private DataService _sut;

        public DataServiceTests()
        {
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _keyName = "TestKeyName";
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();
            _keyMock = new Mock<IKey>();
            _dataRecordMock = new Mock<IDataRecord>();

            _dataLinkServiceMock = new Mock<IDataLinkService>();
            _dataLinkServiceMock.Setup(d => d.GetDataModel(_instrumentName, _serverParkName)).Returns(_dataModelMock.Object);
            _dataLinkServiceMock.Setup(d => d.KeyExists(_keyMock.Object, _instrumentName, _serverParkName)).Returns(true);

            _dataManagerServiceMock = new Mock<IDataManagerService>();
            _dataManagerServiceMock.Setup(d => d.GetKey(_dataModelMock.Object, _keyName)).Returns(_keyMock.Object);
            _dataManagerServiceMock.Setup(d => d.GetDataRecord(_dataModelMock.Object)).Returns(_dataRecordMock.Object);

            _sut = new DataService(
                _dataLinkServiceMock.Object,
                _dataManagerServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_GetDataModel_Then_A_DataModel_Is_Returned()
        {
            //act
            var result = _sut.GetDataModel(_instrumentName, _serverParkName);

            //asert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDatamodel>(result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_Then_The_Correct_DataModel_Is_Returned()
        {
            //act
            var result = _sut.GetDataModel(_instrumentName, _serverParkName);

            //asert
            Assert.AreEqual(_dataModelMock.Object, result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataModel(_instrumentName, _serverParkName);

            //asert
            _dataLinkServiceMock.Verify(v => v.GetDataModel(_instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetKey_Then_A_Key_Is_Returned()
        {
            //act
            var result = _sut.GetKey(_dataModelMock.Object, _keyName);

            //asert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IKey>(result);
        }

        [Test]
        public void Given_I_Call_GetKey_Then_The_Correct_Key_Is_Returned()
        {
            //act
            var result = _sut.GetKey(_dataModelMock.Object, _keyName);

            //asert
            Assert.AreSame(_keyMock.Object, result);
        }

        [Test]
        public void Given_I_Call_GetKey_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetKey(_dataModelMock.Object, _keyName);

            //asert
            _dataManagerServiceMock.Verify(v => v.GetKey(_dataModelMock.Object, _keyName), Times.Once);
        }

        [Test]
        public void Given_I_Call_KeyExists_Then_A_Bool_Is_Returned()
        {
            //act
            var result = _sut.KeyExists(_keyMock.Object, _instrumentName, _serverParkName);

            //asert
            Assert.NotNull(result);
            Assert.IsInstanceOf<bool>(result);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_I_Call_KeyExists_Then_A_The_Correct_Value_Is_Returned(bool keyExists)
        {
            //arrange 
            _dataLinkServiceMock.Setup(d => d.KeyExists(_keyMock.Object, _instrumentName, _serverParkName)).Returns(keyExists);

            //act
            var result = _sut.KeyExists(_keyMock.Object, _instrumentName, _serverParkName);

            //asert
            Assert.AreEqual(keyExists, result);
        }

        [Test]
        public void Given_I_Call_KeyExists_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.KeyExists(_keyMock.Object, _instrumentName, _serverParkName);

            //asert
            _dataLinkServiceMock.Verify(v => v.KeyExists(_keyMock.Object, _instrumentName, _serverParkName), Times.Once);
        }

        [Test]
        public void Given_I_Call_GetDataRecord_Then_A_DataRecord_Is_Returned()
        {
            //act
            var result = _sut.GetDataRecord(_dataModelMock.Object);

            //asert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDataRecord>(result);
        }

        [Test]
        public void Given_I_Call_GetDataRecord_Then_The_Correct_DataRecord_Is_Returned()
        {
            //act
            var result = _sut.GetDataRecord(_dataModelMock.Object);

            //asert
            Assert.AreSame(_dataRecordMock.Object, result);
        }

        [Test]
        public void Given_I_Call_GetDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.GetDataRecord(_dataModelMock.Object);

            //asert
            _dataManagerServiceMock.Verify(v => v.GetDataRecord(_dataModelMock.Object), Times.Once);
        }

        [Test]
        public void Given_I_Call_WriteDataRecord_Then_The_Correct_Services_Are_Called()
        {
            //act
            _sut.WriteDataRecord(_dataRecordMock.Object, _instrumentName, _serverParkName);

            //asert
            _dataLinkServiceMock.Verify(v => v.WriteDataRecord(_dataRecordMock.Object, _instrumentName, _serverParkName), Times.Once);
        }
    }
}
