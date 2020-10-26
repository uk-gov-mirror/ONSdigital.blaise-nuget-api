using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Survey
{
    public class FluentApiSurveyTests
    {
        private readonly ConnectionModel _connectionModel;

		public FluentApiSurveyTests()
        {
            _connectionModel = new ConnectionModel
            {
                Binding = "",
                UserName = "",
                Password = "",
                ServerName = "",
                Port = 0,
                RemotePort = 0,
                ConnectionExpiresInMinutes = 0
            };
        }

        [TestCase(FieldNameType.HOut, true)]
        [TestCase(FieldNameType.NotSpecified, false)]
		public void Given_A_Survey_That_Exists_When_I_Call_HasField_Then_The_Expected_Value_Is_Returned(FieldNameType fieldNameType, bool exists)
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act
            var result = sut
                .WithConnection(_connectionModel)
                .WithInstrument("OPN2004A")
                .WithServerPark("LocalDevelopment")
                .Survey
                .HasField(fieldNameType);

			//assert
			Assert.AreEqual(exists, result);
        }
    }
}
