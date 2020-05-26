
using Blaise.Nuget.Api.Core.Interfaces.Services;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataRecord;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class FieldServiceTests
    {
        private Mock<IDataRecordService> _dataRecordServiceMock;
        private Mock<IDataModelService> _dataModelServiceMock;

        private const string CompletedFieldName = "Completed";
        private const string ProcessedFieldName = "Processed";

        private readonly string _instrumentName;
        private readonly string _serverParkName;

        private FieldService _sut;

        public FieldServiceTests()
        {
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataRecordServiceMock = new Mock<IDataRecordService>();
            _dataModelServiceMock = new Mock<IDataModelService>();

            _sut = new FieldService(_dataRecordServiceMock.Object, _dataModelServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_CompletedFieldExists_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            dataModelMock.As<IDefinitionScope2>();
            dataModelMock.As<IDefinitionScope2>().Setup(d => d.FieldExists("CompletedFieldName")).Returns(It.IsAny<bool>());

            _dataModelServiceMock.Setup(d => d.GetDataModel(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataModelMock.Object);

            //act
            _sut.CompletedFieldExists(_instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(d => d.GetDataModel(_instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_I_Call_CompletedFieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            dataModelMock.As<IDefinitionScope2>();
            dataModelMock.As<IDefinitionScope2>().Setup(d => d.FieldExists(CompletedFieldName)).Returns(fieldExists);
            _dataModelServiceMock.Setup(d => d.GetDataModel(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataModelMock.Object);

            //act
            var result = _sut.CompletedFieldExists(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(fieldExists, result);
        }

        [TestCase(2, false)]
        [TestCase(1,true)]
        [TestCase(0,false)]
        public void Given_I_Call_CaseHasBeenCompleted_Then_The_Correct_Value_Is_Returned(int value, bool fieldExists)
        {
            //arrange
            var fieldMock = new Mock<IField>();
            fieldMock.Setup(c => c.DataValue.IntegerValue).Returns(value);

            var dataRecordMock = new Mock<IDataRecord>();
            dataRecordMock.Setup(d => d.GetField(CompletedFieldName)).Returns(fieldMock.Object);

            //act
            var result = _sut.CaseHasBeenCompleted(dataRecordMock.Object);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(fieldExists, result);
        }

        [Test]
        public void Given_I_Call_MarkCaseAsComplete_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var fieldMock = new Mock<IField>();
            fieldMock.Setup(c => c.DataValue.IntegerValue);

            var dataRecordMock = new Mock<IDataRecord>();
            dataRecordMock.Setup(d => d.GetField(CompletedFieldName)).Returns(fieldMock.Object);

            //act
            _sut.MarkCaseAsComplete(dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, _instrumentName, _serverParkName));
        }

        [Test]
        public void Given_I_Call_ProcessedFieldExists_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            dataModelMock.As<IDefinitionScope2>();
            dataModelMock.As<IDefinitionScope2>().Setup(d => d.FieldExists(ProcessedFieldName)).Returns(It.IsAny<bool>());

            _dataModelServiceMock.Setup(d => d.GetDataModel(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataModelMock.Object);

            //act
            _sut.ProcessedFieldExists(_instrumentName, _serverParkName);

            //assert
            _dataModelServiceMock.Verify(d => d.GetDataModel(_instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_I_Call_ProcessedFieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            dataModelMock.As<IDefinitionScope2>();
            dataModelMock.As<IDefinitionScope2>().Setup(d => d.FieldExists(ProcessedFieldName)).Returns(fieldExists);
            _dataModelServiceMock.Setup(d => d.GetDataModel(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataModelMock.Object);

            //act
            var result = _sut.ProcessedFieldExists(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(fieldExists, result);
        }

        [TestCase(2, false)]
        [TestCase(1, true)]
        [TestCase(0, false)]
        public void Given_I_Call_CaseHasBeenProcessed_Then_The_Correct_Value_Is_Returned(int value, bool fieldExists)
        {
            //arrange
            var fieldMock = new Mock<IField>();
            fieldMock.Setup(c => c.DataValue.EnumerationValue).Returns(value);

            var dataRecordMock = new Mock<IDataRecord>();
            dataRecordMock.Setup(d => d.GetField(ProcessedFieldName)).Returns(fieldMock.Object);

            //act
            var result = _sut.CaseHasBeenProcessed(dataRecordMock.Object);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(fieldExists, result);
        }


        [Test]
        public void Given_I_Call_MarkCaseAsProcessed_Then_The_Correct_Services_Are_Called()
        {
            //arrange
            var fieldMock = new Mock<IField>();
            fieldMock.Setup(c => c.DataValue.IntegerValue);

            var dataRecordMock = new Mock<IDataRecord>();
            dataRecordMock.Setup(d => d.GetField(ProcessedFieldName)).Returns(fieldMock.Object);

            //act
            _sut.MarkCaseAsProcessed(dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(dataRecordMock.Object, _instrumentName, _serverParkName));
        }
    }
}
