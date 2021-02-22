using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Field
{
    public class GetCaseInUseFieldTests
    {
        private readonly BlaiseCaseApi _sut;
        private readonly string _primaryKey;

        public GetCaseInUseFieldTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKey = "9000001";
        }

        //[Ignore("Integration")]
        [TestCase(0)]
        [TestCase(1)]
        public void Given_CaseInUse_Value_Set_When_I_Call_GetFieldValue_Then_The_Correct_Value_Is_Returned(int caseInUse)
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string instrumentName = "OPN2102R";
            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"},
                {FieldNameType.CaseInUse.FullName(), caseInUse.ToString()}
            };

            _sut.CreateCase(_primaryKey, fieldData, instrumentName, serverParkName);

            //act
            var dataRecord = _sut.GetCase(_primaryKey,  instrumentName, serverParkName);
            var result = _sut.GetFieldValue(dataRecord, FieldNameType.CaseInUse);
   
            //arrange
            Assert.AreEqual(caseInUse, result.IntegerValue);

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }
    }
}
