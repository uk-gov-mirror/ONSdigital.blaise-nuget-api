
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    public class CreateCaseTests
    {
        private readonly BlaiseCaseApi _sut;
        private readonly string _primaryKey;

        public CreateCaseTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKey = "9000001";
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Valid_Arguments_When_I_Call_CreateCase_Then_The_Case_Is_Created()
        {
            //arrange
            var serverParkName = "LocalDevelopment";
            var instrumentName = "OPN2101A";
            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"},
            };

            //act
            _sut.CreateCase(_primaryKey, fieldData, instrumentName, serverParkName);

            //arrange
            Assert.IsTrue(_sut.CaseExists(_primaryKey, instrumentName, serverParkName));

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }
    }
}
