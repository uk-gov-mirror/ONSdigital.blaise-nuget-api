using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;
using StatNeth.Blaise.API.Meta;

namespace Blaise.Nuget.Api.Tests.Behaviour.Field
{
    public class FieldTests
    {
        private readonly BlaiseCaseApi _sut;
        private readonly string _primaryKey;

        public FieldTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKey = "9000001";
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Value_Set_When_I_Call_GetFieldValue_Then_The_Correct_Value_Is_Returned()
        {
            //arrange
            const string serverParkName = "localdevelopment";
            const string instrumentName = "OPN2102R";

            var lastUpdated = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"},
                {FieldNameType.LastUpdated.FullName(), lastUpdated}
            };
            
            _sut.CreateCase(_primaryKey, fieldData, instrumentName, serverParkName);

            //act
            var dataRecord = _sut.GetCase(_primaryKey,  instrumentName, serverParkName);
            
            var result = _sut.GetFieldValue(dataRecord, FieldNameType.LastUpdated);

            //arrange
            Assert.AreEqual(lastUpdated, result.ValueAsText);

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }
    }
}
