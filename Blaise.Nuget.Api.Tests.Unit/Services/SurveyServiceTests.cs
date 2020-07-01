using Blaise.Nuget.Api.Contracts.Exceptions;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.ServerManager;
using System;
using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Core.Interfaces.Services;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class SurveyServiceTests
    {
        private Mock<IParkService> _parkServiceMock;

        private Mock<ISurvey> _surveyMock;
        private Mock<ISurveyCollection> _surveyCollectionMock;
        private Mock<IServerPark> _serverParkMock;

        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly Guid _instrumentId;

        private SurveyService _sut;

        public SurveyServiceTests()
        {
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
            _instrumentId = Guid.NewGuid();
        }

        [SetUp]
        public void SetUpTests()
        {
            //setup surveys
            _surveyMock = new Mock<ISurvey>();
            _surveyMock.Setup(s => s.Name).Returns(_instrumentName);
            _surveyMock.Setup(s => s.InstrumentID).Returns(_instrumentId);

            var surveyItems = new List<ISurvey> { _surveyMock.Object };

            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());

            //setup server parks
            _serverParkMock = new Mock<IServerPark>();
            _serverParkMock.Setup(s => s.Name).Returns("TestServerParkName");
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);

            _parkServiceMock = new Mock<IParkService>();
            _parkServiceMock.Setup(p => p.GetServerPark(_serverParkName)).Returns(_serverParkMock.Object);
            _parkServiceMock.Setup(p => p.GetServerParkNames()).Returns(new List<string> {_serverParkName});

            //setup service under test
            _sut = new SurveyService(_parkServiceMock.Object);
        }

        [Test]
        public void Given_I_Call_GetSurveyNames_Then_I_Get_A_Correct_List_Of_Survey_Names_Returned()
        {
            //act
            var result = _sut.GetSurveyNames(_serverParkName).ToList();

            //assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            Assert.True(result.Contains(_instrumentName));
        }

        [Test]
        public void Given_No_Surveys_When_I_Call_GetSurveyNames_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            var surveyItems = new List<ISurvey>();
            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);
            _parkServiceMock.Setup(s => s.GetServerPark(It.IsAny<string>())).Returns(_serverParkMock.Object);


            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetSurveyNames(_serverParkName));
            Assert.AreEqual($"No surveys found for server park '{_serverParkName}'", exception.Message);
        }

        [Test]
        public void Given_No_Surveys_Are_On_A_ServerPark_When_I_Call_GetSurveyNames_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            var serverParkName = "ServerParkDoesNotExist";
            var surveyItems = new List<ISurvey>();

            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());

            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);
            _parkServiceMock.Setup(p => p.GetServerPark(It.IsAny<string>())).Returns(_serverParkMock.Object);

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetSurveyNames(serverParkName));
            Assert.AreEqual($"No surveys found for server park '{serverParkName}'", exception.Message);
        }

        [Test]
        public void Given_I_Call_GetSurveys_Then_I_Get_A_Correct_List_Of_Surveys_Returned()
        {
            //act
            var result = _sut.GetSurveys(_serverParkName).ToList();

            //assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(1, result.Count);
            Assert.True(result.Contains(_surveyMock.Object));
        }

        [Test]
        public void Given_No_Surveys_When_I_Call_GetSurveys_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            var surveyItems = new List<ISurvey>();
            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());
            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetSurveys(_serverParkName));
            Assert.AreEqual($"No surveys found for server park '{_serverParkName}'", exception.Message);
        }

        [Test]
        public void Given_No_Surveys_Are_On_A_ServerPark_When_I_Call_GetSurveys_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            var serverParkName = "ServerParkDoesNotExist";
            var surveyItems = new List<ISurvey>();

            _surveyCollectionMock = new Mock<ISurveyCollection>();
            _surveyCollectionMock.Setup(s => s.GetEnumerator()).Returns(() => surveyItems.GetEnumerator());

            _serverParkMock.Setup(s => s.Surveys).Returns(_surveyCollectionMock.Object);
            _parkServiceMock.Setup(p => p.GetServerPark(It.IsAny<string>())).Returns(_serverParkMock.Object);

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetSurveys(serverParkName));
            Assert.AreEqual($"No surveys found for server park '{serverParkName}'", exception.Message);
        }

        [Test]
        public void Given_A_Survey_Exists_When_I_Call_GetSurvey_Then_I_Get_The_Expected_Survey_Back()
        {
            //act
            var result = _sut.GetSurvey(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<ISurvey>(result);
            Assert.AreEqual(_surveyMock.Object, result);
        }

        [Test]
        public void Given_A_Survey_Does_Not_Exist_When_I_Call_GetSurveys_Then_A_Data_Not_Found_Exception_Is_Thrown()
        {
            //arrange
            _surveyMock.Setup(s => s.Name).Returns("DoesNotExist");

            //act && assert
            var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetSurvey(_instrumentName, _serverParkName));
            Assert.AreEqual($"No survey found for instrument name '{_instrumentName}'", exception.Message);
        }

        [Test]
        public void Given_I_Call_GetAllSurveys_Then_I_Get_A_Correct_List_Of_Surveys_Returned()
        {
            //arrange
            var serverPark1Name = "ServerPark1";
            var serverPark2Name = "ServerPark2";

            var survey1Mock = new Mock<ISurvey>();
            var survey2Mock = new Mock<ISurvey>();
            var survey3Mock = new Mock<ISurvey>();

            var survey1Items = new List<ISurvey> { survey1Mock.Object, survey2Mock.Object };
            var survey2Items = new List<ISurvey> { survey3Mock.Object};

            var surveyCollection1Mock = new Mock<ISurveyCollection>();
            surveyCollection1Mock.Setup(s => s.GetEnumerator()).Returns(() => survey1Items.GetEnumerator());

            var surveyCollection2Mock = new Mock<ISurveyCollection>();
            surveyCollection2Mock.Setup(s => s.GetEnumerator()).Returns(() => survey2Items.GetEnumerator());

            var serverPark1Mock = new Mock<IServerPark>();
            serverPark1Mock.Setup(s => s.Name).Returns(serverPark1Name);
            serverPark1Mock.Setup(s => s.Surveys).Returns(surveyCollection1Mock.Object);
            
            var serverPark2Mock = new Mock<IServerPark>();
            serverPark2Mock.Setup(s => s.Name).Returns(serverPark2Name);
            serverPark2Mock.Setup(s => s.Surveys).Returns(surveyCollection2Mock.Object);

            _parkServiceMock.Setup(p => p.GetServerParkNames()).Returns(new List<string> { serverPark1Name, serverPark2Name });
            _parkServiceMock.Setup(p => p.GetServerPark(serverPark1Name)).Returns(serverPark1Mock.Object);
            _parkServiceMock.Setup(p => p.GetServerPark(serverPark2Name)).Returns(serverPark2Mock.Object);

            //act
            var result = _sut.GetAllSurveys().ToList();

            //assert
            Assert.NotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(3, result.Count);
            Assert.True(result.Contains(survey1Mock.Object));
            Assert.True(result.Contains(survey2Mock.Object));
            Assert.True(result.Contains(survey3Mock.Object));
        }


        [Test]
        public void Given_I_Call_GetInstrumentId_Then_I_Get_A_Guid_Returned()
        {
            //act
            var result = _sut.GetInstrumentId(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<Guid>(result);
        }

        [Test]
        public void Given_I_Call_GetInstrumentId_Then_I_Get_The_Correct_InstrumentId_Returned()
        {
            //act
            var result = _sut.GetInstrumentId(_instrumentName, _serverParkName);

            //assert
            Assert.AreEqual(_instrumentId, result);
        }

        [Test]
        public void Given_I_Call_GetInstrumentId_And_The_Instrument_DoesNot_Exist_Then_An_ArgumentOutOfRangeException_Is_Thrown()
        {
            //arrange
            var instrumentName = "InstrumentThatDoesNotExist";

            //act && assert
           var exception = Assert.Throws<DataNotFoundException>(() => _sut.GetInstrumentId(instrumentName, _serverParkName));
            Assert.AreEqual($"Instrument '{instrumentName}' not found on server park '{_serverParkName}'", exception.Message);
        }

        [Test]
        public void Given_I_Call_GetMetaFileName_Then_The_Correct_Name_Is_Returned()
        {
            //arrange
            var metaFileName = "MetaFileName";

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.MetaFileName).Returns(metaFileName);


            var configurationItems = new List<IConfiguration> { configurationMock.Object };

            var configurationCollectionMock = new Mock<IMachineConfigurationCollection>();
            configurationCollectionMock.Setup(s => s.Configurations).Returns(configurationItems);

            _surveyMock.Setup(s => s.Configuration).Returns(configurationCollectionMock.Object);

            //act
            var result = _sut.GetMetaFileName(_instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(metaFileName, result);
        }

        [Test]
        public void Given_I_Call_GetDataFileName_Then_The_Correct_Name_Is_Returned()
        {
            //arrange
            var dataFileName = "DataFileName";

            var configurationMock = new Mock<IConfiguration>();
            configurationMock.Setup(c => c.DataFileName).Returns(dataFileName);


            var configurationItems = new List<IConfiguration> { configurationMock.Object };

            var configurationCollectionMock = new Mock<IMachineConfigurationCollection>();
            configurationCollectionMock.Setup(s => s.Configurations).Returns(configurationItems);

            _surveyMock.Setup(s => s.Configuration).Returns(configurationCollectionMock.Object);

            //act
            var result = _sut.GetDataFileName(_instrumentName, _serverParkName);

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(dataFileName, result);
        }
    }
}
