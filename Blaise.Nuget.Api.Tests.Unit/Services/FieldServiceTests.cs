
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Extensions;
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

        private readonly string _completedFieldName = FieldNameType.Completed.ToString();
        private readonly string _processedFieldName = FieldNameType.Processed.ToString();

        private readonly ConnectionModel _connectionModel;
        private readonly string _instrumentName;
        private readonly string _serverParkName;

        private FieldService _sut;

        public FieldServiceTests()
        {
            _connectionModel = new ConnectionModel();
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

        [TestCase(FieldNameType.Completed)]
        [TestCase(FieldNameType.Processed)]
        [TestCase(FieldNameType.WebFormStatus)]
        [TestCase(FieldNameType.CaseId)]
        [TestCase(FieldNameType.HOut)]
        public void Given_I_Call_FieldExists_Then_The_Correct_Services_Are_Called(FieldNameType fieldNameType)
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            dataModelMock.As<IDefinitionScope2>();
            dataModelMock.As<IDefinitionScope2>().Setup(d => d.FieldExists(fieldNameType.ToString())).Returns(It.IsAny<bool>());

            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataModelMock.Object);

            //act
            _sut.FieldExists(_connectionModel, _instrumentName, _serverParkName, fieldNameType);

            //assert
            _dataModelServiceMock.Verify(d => d.GetDataModel(_connectionModel, _instrumentName, _serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_I_Call_FieldExists_Then_The_Correct_Value_Is_Returned(bool fieldExists)
        {
            //arrange
            var dataModelMock = new Mock<IDatamodel>();
            dataModelMock.As<IDefinitionScope2>();
            dataModelMock.As<IDefinitionScope2>().Setup(d => d.FieldExists(FieldNameType.Completed.ToString())).Returns(fieldExists);
            _dataModelServiceMock.Setup(d => d.GetDataModel(_connectionModel, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(dataModelMock.Object);

            //act
            var result = _sut.FieldExists(_connectionModel, _instrumentName, _serverParkName, FieldNameType.Completed);

            //assert
            Assert.NotNull(result);
            Assert.AreEqual(fieldExists, result);
        }

        [TestCase(FieldNameType.CaseId, true)]
        [TestCase(FieldNameType.HOut, false)]
        public void Given_A_DataRecord_When_I_Call_FieldExists_Then_The_Correct_Value_Is_Returned(FieldNameType fieldNameType, bool fieldExists)
        {
            //arrange
            var iFieldMock = new Mock<IField>();

            if (fieldExists)
            {
                iFieldMock.Setup(f => f.FullName).Returns(fieldNameType.FromDescription());
            }
            else
            {
                iFieldMock.Setup(f => f.FullName).Returns("Does Not Exist");
            }

            var dataRecord2Mock = new Mock<IDataRecord2>();
            dataRecord2Mock.Setup(d => d.GetDataFields()).Returns(new List<IField> { iFieldMock .Object});

            //act
            var result = _sut.FieldExists(dataRecord2Mock.Object, fieldNameType);

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
            dataRecordMock.Setup(d => d.GetField(_completedFieldName)).Returns(fieldMock.Object);

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
            dataRecordMock.Setup(d => d.GetField(_completedFieldName)).Returns(fieldMock.Object);

            //act
            _sut.MarkCaseAsComplete(_connectionModel, dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, dataRecordMock.Object, _instrumentName, _serverParkName));
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
            dataRecordMock.Setup(d => d.GetField(_processedFieldName)).Returns(fieldMock.Object);

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
            dataRecordMock.Setup(d => d.GetField(_processedFieldName)).Returns(fieldMock.Object);

            //act
            _sut.MarkCaseAsProcessed(_connectionModel, dataRecordMock.Object, _instrumentName, _serverParkName);

            //assert
            _dataRecordServiceMock.Verify(v => v.WriteDataRecord(_connectionModel, dataRecordMock.Object, _instrumentName, _serverParkName));
        }

        [TestCase(FieldNameType.Completed)]
        [TestCase(FieldNameType.Processed)]
        [TestCase(FieldNameType.WebFormStatus)]
        public void Given_I_Call_GetField_Then_The_Correct_Field_Is_Returned(FieldNameType fieldNameType)
        {
            //arrange
            var fieldMock = new Mock<IField>();

            var dataRecordMock = new Mock<IDataRecord>();
            dataRecordMock.Setup(d => d.GetField(fieldNameType.ToString())).Returns(fieldMock.Object);

            //act
           var result = _sut.GetField(dataRecordMock.Object, fieldNameType);

            //assert
            Assert.AreEqual(fieldMock.Object, result);
        }
    }
}
