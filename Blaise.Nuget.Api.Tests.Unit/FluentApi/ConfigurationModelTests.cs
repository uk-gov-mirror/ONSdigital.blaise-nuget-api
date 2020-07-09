using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class ConfigurationModelTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly ConnectionModel _connectionModel;

        private FluentBlaiseApi _sut;

        public ConfigurationModelTests()
        {
            _connectionModel = new ConnectionModel();
        }

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();
            _blaiseApiMock.Setup(b => b.GetDefaultConnectionModel()).Returns(_connectionModel);

            _sut = new FluentBlaiseApi(_blaiseApiMock.Object);
        }

        [Test]
        public void Given_I_Call_GetDefaultConnectionModel_The_Correct_Model_Is_Returned()
        {
            //act
            var result = _sut.DefaultConnection;

            //assert
            Assert.AreSame(_connectionModel, result);
        }

        [Test]
        public void Given_I_Call_GetDefaultConnectionModel_The_Correct_Services_Are_Called()
        {
            //act
            var result = _sut.DefaultConnection;

            //assert
            _blaiseApiMock.Verify(v => v.GetDefaultConnectionModel(), Times.Once);
        }
    }
}
