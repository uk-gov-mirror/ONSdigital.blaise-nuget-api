using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class ServerParkTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly string _instrumentName;
        private readonly string _serverParkName;
        private readonly string _primaryKeyValue;

        private FluentBlaiseApi _sut;

        public ServerParkTests()
        {
            _instrumentName = "Instrument1";
            _serverParkName = "Park1";
            _primaryKeyValue = "Key1";
        }

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();

            _sut = new FluentBlaiseApi(_blaiseApiMock.Object);
        }

        [Test]
        public void Given_Case_Is_Not_Called_When_I_Call_Exists_Then_The_Correct_Service_Method_Is_Called()
        {
            //arrange

            _blaiseApiMock.Setup(p => p.ServerParkExists(It.IsAny<string>())).Returns(It.IsAny<bool>());

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);

            //act
            _sut.Exists();

            //assert
            _blaiseApiMock.Verify(v => v.ServerParkExists(_serverParkName), Times.Once);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Given_Case_Is_Not_Called_When_I_Call_Exists_Then_The_Expected_Result_Is_Returned(bool serverParkexists)
        {
            //arrange

            _blaiseApiMock.Setup(p => p.ServerParkExists(_serverParkName)).Returns(serverParkexists);

            _sut.ServerPark(_serverParkName);
            _sut.Instrument(_instrumentName);

            //act
            var result = _sut.Exists();

            //assert
            Assert.IsNotNull(result);
            Assert.AreEqual(serverParkexists, result);
        }

        [Test]
        public void Given_Case_Is_Not_Called_But_Instrument_Has_Not_Been_Called_When_I_Call_Exists_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange

            _sut.ServerPark(_serverParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() => _sut.Exists());
            Assert.AreEqual("The 'Instrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }
    }
}
