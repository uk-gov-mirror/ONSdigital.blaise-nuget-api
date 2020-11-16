using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit
{
    public class BlaiseApiTests
    {
        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseBackupApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseBackupApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseBackupApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
           // Assert.DoesNotThrow(() => new BlaiseBackupApi(new ConnectionModel()));
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseCaseApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseCaseApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseCaseApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseCaseApi(new ConnectionModel()));
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseServerParkApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseServerParkApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseServerParkApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseServerParkApi(new ConnectionModel()));
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseUserApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseUserApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseUserApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseUserApi(new ConnectionModel()));
        }

        [Test]
        public void Given_No_ConnectionModel_When_I_Instantiate_BlaiseSurveyApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseSurveyApi());
        }

        [Test]
        public void Given_A_ConnectionModel_When_I_Instantiate_BlaiseSurveyApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new BlaiseSurveyApi(new ConnectionModel()));
        }
    }
}
