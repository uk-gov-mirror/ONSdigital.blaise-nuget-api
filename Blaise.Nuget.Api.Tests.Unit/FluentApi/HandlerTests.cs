using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Moq;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.FluentApi
{
    public class HandlerTests
    {
        private Mock<IBlaiseApi> _blaiseApiMock;

        private readonly string _sourceServerName;
        private readonly string _sourceInstrumentName;
        private readonly string _sourceServerParkName;
        private readonly string _primaryKeyValue;
        private readonly string _destinationServerName;
        private readonly string _destinationInstrumentName;
        private readonly string _destinationServerParkName;
        private readonly string _destinationFilePath;

        private FluentBlaiseApi _sut;

        public HandlerTests()
        {
            _sourceServerName = "Server1";
            _sourceInstrumentName = "Instrument1";
            _sourceServerParkName = "Park1";
            _primaryKeyValue = "Key1";

            _destinationServerName = "Server2";
            _destinationInstrumentName = "Instrument2";
            _destinationServerParkName = "Park2";
            _destinationFilePath = "FilePath";
        }

        [SetUp]
        public void SetUpTests()
        {
            _blaiseApiMock = new Mock<IBlaiseApi>();

            _sut = new FluentBlaiseApi(_blaiseApiMock.Object);
        }

        [Test]
        public void Given_A_ServerName_When_I_Call_WithServer_The_Correct_Services_Are_Called()
        {
            //act
            _sut.WithServer(_sourceServerName);

            //assert
            _blaiseApiMock.Verify(v => v.UseServer(_sourceServerName), Times.Once);
        }

        [Test]
        public void Given_I_Use_ToFile_For_Copy_When_I_Call_Handle_The_Correct_Services_Are_Called()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToFile(_destinationFilePath);
            _sut.ToInstrument(_destinationInstrumentName);

            //act
            _sut.Copy.Handle();

            //assert
            _blaiseApiMock.Verify(v => v.CopyCase(_primaryKeyValue,  _sourceInstrumentName,
                _sourceServerParkName,_destinationFilePath, _destinationInstrumentName), Times.Once);
        }

        [Test]
        public void Given_I_Use_ToFile_But_WithInstrument_Has_Not_Been_Called_For_Copy_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToFile(_destinationFilePath);
            _sut.ToInstrument(_destinationInstrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("The 'WithInstrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToFile_But_WithServerPark_Has_Not_Been_Called_For_Copy_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            //_sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToFile(_destinationFilePath);
            _sut.ToInstrument(_destinationInstrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToFile_But_Case_Has_Not_Been_Called_For_Copy_When_I_Call_Handle_Then_A_NotSupportedException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            //_sut.WithPrimaryKey(_primaryKeyValue);

            _sut.ToFile(_destinationFilePath);
            _sut.ToInstrument(_destinationInstrumentName);

            //act && assert
            var exception = Assert.Throws<NotSupportedException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("You have not declared a step previously where this action is supported", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToFile_But_ToInstrument_Has_Not_Been_Called_For_Copy_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToFile(_destinationFilePath);
            //_sut.ToInstrument(_destinationInstrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("The 'ToInstrument' step needs to be called prior to this to specify the destination name of the instrument", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToFile_And_Move_Is_Called_When_I_Call_Handle_The_Correct_Services_Are_Called()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToFile(_destinationFilePath);
            _sut.ToInstrument(_destinationInstrumentName);

            //act
            _sut.Move.Handle();

            //assert
            _blaiseApiMock.Verify(v => v.MoveCase(_primaryKeyValue, _sourceInstrumentName,
                _sourceServerParkName, _destinationFilePath, _destinationInstrumentName), Times.Once);
        }

        [Test]
        public void Given_I_Use_ToFile_But_WithInstrument_Has_Not_Been_Called_For_Move_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToFile(_destinationFilePath);
            _sut.ToInstrument(_destinationInstrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Move.Handle();
            });

            Assert.AreEqual("The 'WithInstrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToFile_But_WithServerPark_Has_Not_Been_Called_For_Move_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            //_sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToFile(_destinationFilePath);
            _sut.ToInstrument(_destinationInstrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Move.Handle();
            });

            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToFile_But_Case_Has_Not_Been_Called_For_Move_When_I_Call_Handle_Then_A_NotSupportedException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            //_sut.WithPrimaryKey(_primaryKeyValue);

            _sut.ToFile(_destinationFilePath);
            _sut.ToInstrument(_destinationInstrumentName);

            //act && assert
            var exception = Assert.Throws<NotSupportedException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("You have not declared a step previously where this action is supported", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToFile_But_ToInstrument_Has_Not_Been_Called_For_Move_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToFile(_destinationFilePath);
            //_sut.ToInstrument(_destinationInstrumentName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("The 'ToInstrument' step needs to be called prior to this to specify the destination name of the instrument", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToServer_For_Copy_When_I_Call_Handle_The_Correct_Services_Are_Called()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            _sut.ToInstrument(_destinationInstrumentName);
            _sut.ToServerPark(_destinationServerParkName);

            //act
            _sut.Copy.Handle();

            //assert
            _blaiseApiMock.Verify(v => v.CopyCase(_primaryKeyValue, _sourceInstrumentName, 
                    _sourceServerParkName, _destinationServerName, _destinationInstrumentName, 
                    _destinationServerParkName), Times.Once);
        }

        [Test]
        public void Given_I_Use_ToServer_But_WithInstrument_Has_Not_Been_Called_For_Copy_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            _sut.ToInstrument(_destinationInstrumentName);
            _sut.ToServerPark(_destinationServerParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("The 'WithInstrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToServer_But_WithServerPark_Has_Not_Been_Called_For_Copy_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            //_sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            _sut.ToInstrument(_destinationInstrumentName);
            _sut.ToServerPark(_destinationServerParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToServer_But_Case_Has_Not_Been_Called_For_Copy_When_I_Call_Handle_Then_A_NotSupportedException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            //_sut.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            _sut.ToInstrument(_destinationInstrumentName);
            _sut.ToServerPark(_destinationServerParkName);

            //act && assert
            var exception = Assert.Throws<NotSupportedException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("You have not declared a step previously where this action is supported", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToServer_But_ToInstrument_Has_Not_Been_Called_For_Copy_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            //_sut.ToInstrument(_destinationInstrumentName);
            _sut.ToServerPark(_destinationServerParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("The 'ToInstrument' step needs to be called prior to this to specify the destination name of the instrument", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToServer_But_ToServerPark_Has_Not_Been_Called_For_Copy_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            _sut.ToInstrument(_destinationInstrumentName);
            //_sut.ToServerPark(_destinationServerParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("The 'ToServerPark' step needs to be called prior to this to specify the destination name of the server park", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToServer_And_Move_Is_Called_When_I_Call_Handle_The_Correct_Services_Are_Called()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            _sut.ToInstrument(_destinationInstrumentName);
            _sut.ToServerPark(_destinationServerParkName);

            //act
            _sut.Move.Handle();

            //assert
            _blaiseApiMock.Verify(v => v.MoveCase(_primaryKeyValue, _sourceInstrumentName,
                _sourceServerParkName, _destinationServerName, _destinationInstrumentName,
                _destinationServerParkName), Times.Once);
        }

        [Test]
        public void Given_I_Use_ToServer_But_WithInstrument_Has_Not_Been_Called_For_Move_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            //_sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            _sut.ToInstrument(_destinationInstrumentName);
            _sut.ToServerPark(_destinationServerParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Move.Handle();
            });

            Assert.AreEqual("The 'WithInstrument' step needs to be called prior to this to specify the name of the instrument", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToServer_But_WithServerPark_Has_Not_Been_Called_For_Move_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            //_sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            _sut.ToInstrument(_destinationInstrumentName);
            _sut.ToServerPark(_destinationServerParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Move.Handle();
            });

            Assert.AreEqual("The 'WithServerPark' step needs to be called prior to this to specify the name of the server park", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToServer_But_Case_Has_Not_Been_Called_For_Move_When_I_Call_Handle_Then_A_NotSupportedException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            //_sut.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            _sut.ToInstrument(_destinationInstrumentName);
            _sut.ToServerPark(_destinationServerParkName);

            //act && assert
            var exception = Assert.Throws<NotSupportedException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("You have not declared a step previously where this action is supported", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToServer_But_ToInstrument_Has_Not_Been_Called_For_Move_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            //_sut.ToInstrument(_destinationInstrumentName);
            _sut.ToServerPark(_destinationServerParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("The 'ToInstrument' step needs to be called prior to this to specify the destination name of the instrument", exception.Message);
        }

        [Test]
        public void Given_I_Use_ToServer_But_ToServerPark_Has_Not_Been_Called_For_Move_When_I_Call_Handle_Then_An_NullReferenceException_Is_Thrown()
        {
            //arrange
            _sut.WithInstrument(_sourceInstrumentName);
            _sut.WithServerPark(_sourceServerParkName);
            _sut.Case.WithPrimaryKey(_primaryKeyValue);

            _sut.ToServer(_destinationServerName);
            _sut.ToInstrument(_destinationInstrumentName);
            //_sut.ToServerPark(_destinationServerParkName);

            //act && assert
            var exception = Assert.Throws<NullReferenceException>(() =>
            {
                _sut.Copy.Handle();
            });

            Assert.AreEqual("The 'ToServerPark' step needs to be called prior to this to specify the destination name of the server park", exception.Message);
        }

        [Test]
        public void Given_Handle_Has_Been_Called_But_No_HandleType_Is_Set_When_I_Call_Handle_Then_A_NotSupportedException_Is_Thrown()
        {
            //arrange
            _sut.WithPrimaryKey(_primaryKeyValue);

            //act && assert
            var exception = Assert.Throws<NotSupportedException>(() =>
            {
                _sut.Handle();
            });

            Assert.AreEqual("You have not declared a step previously where this action is supported", exception.Message);
        }
    }
}
