using Blaise.Nuget.Api.Core.Interfaces.Providers;
using Blaise.Nuget.Api.Core.Services;
using Moq;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Unit.Services
{
    public class DataModelServiceTests
    {
        private Mock<IRemoteDataLinkProvider> _remoteDataLinkProviderMock;

        private Mock<IDataLink4> _remoteDataLinkMock;
        private Mock<IDatamodel> _dataModelMock;
   

        private readonly string _instrumentName;
        private readonly string _serverParkName;

        private DataModelService _sut;

        public DataModelServiceTests()
        {
            _instrumentName = "TestInstrumentName";
            _serverParkName = "TestServerParkName";
        }

        [SetUp]
        public void SetUpTests()
        {
            _dataModelMock = new Mock<IDatamodel>();

            _remoteDataLinkMock = new Mock<IDataLink4>();
            _remoteDataLinkMock.Setup(d => d.Datamodel).Returns(_dataModelMock.Object);

            _remoteDataLinkProviderMock = new Mock<IRemoteDataLinkProvider>();
            _remoteDataLinkProviderMock.Setup(r => r.GetDataLink(_instrumentName, _serverParkName)).Returns(_remoteDataLinkMock.Object);
            
            _sut = new DataModelService(_remoteDataLinkProviderMock.Object);
        }


        [Test]
        public void Given_I_Call_GetDataModel_I_Get_A_DataModel_Back()
        {
            //act
            var result = _sut.GetDataModel(_instrumentName, _serverParkName);

            //assert
            Assert.NotNull(result);
            Assert.IsInstanceOf<IDatamodel>(result);
        }

        [Test]
        public void Given_I_Call_GetDataModel_I_Get_The_Correct_DataModel_Back()
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
            _remoteDataLinkProviderMock.Verify(v => v.GetDataLink(_instrumentName, _serverParkName), Times.Once);
            _remoteDataLinkMock.Verify(v => v.Datamodel, Times.Once);
        }
    }
}
