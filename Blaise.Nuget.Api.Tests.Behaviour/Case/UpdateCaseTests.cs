using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Extensions;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Case
{
    public class UpdateCaseTests
    {
        private readonly BlaiseCaseApi _sut;
        private readonly string _primaryKey;

        public UpdateCaseTests()
        {
            _sut = new BlaiseCaseApi();
            _primaryKey = "9000001";
        }

        [Ignore("Integration")]
        [Test]
        public void Given_Valid_Arguments_When_I_Call_Update_An_Existing_Case_Then_The_Case_Is_Updated()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string instrumentName = "OPN2101A";
            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"}
            };

            _sut.CreateCase(_primaryKey, fieldData, instrumentName, serverParkName);

            fieldData[FieldNameType.TelNo.FullName()] = "0711111111";

            var existingCase = _sut.GetCase(_primaryKey, instrumentName, serverParkName);

            //act
            _sut.UpdateCase(existingCase, fieldData, instrumentName, serverParkName);

            existingCase = _sut.GetCase(_primaryKey, instrumentName, serverParkName);
            var fields = _sut.GetRecordDataFields(existingCase);
            
            //arrange
            Assert.AreEqual("0711111111", fields[FieldNameType.TelNo.FullName()]);

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Existing_Case_Is_Locked_When_I_Call_DataRecordIsLocked_Then_True_Is_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string instrumentName = "OPN2101A";
            const string lockId = "Lock123";

            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"}
            };

            _sut.CreateCase(_primaryKey, fieldData, instrumentName, serverParkName);
            _sut.LockDataRecord(instrumentName, serverParkName, _primaryKey, lockId);

            //act
            var result = _sut.DataRecordIsLocked(instrumentName, serverParkName, _primaryKey);

            //assert
            Assert.IsTrue(result);

            //cleanup
            _sut.UnLockDataRecord(instrumentName, serverParkName, _primaryKey, lockId);
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }

        [Ignore("Integration")]
        [Test]
        public void Given_An_Existing_Case_Is_Not_Locked_When_I_Call_DataRecordIsLocked_Then_False_Is_Returned()
        {
            //arrange
            const string serverParkName = "LocalDevelopment";
            const string instrumentName = "OPN2101A";

            var fieldData = new Dictionary<string, string>
            {
                {FieldNameType.HOut.FullName(), "110"},
                {FieldNameType.TelNo.FullName(), "07000000000"}
            };

            _sut.CreateCase(_primaryKey, fieldData, instrumentName, serverParkName);

            //act
            var result = _sut.DataRecordIsLocked(instrumentName, serverParkName, _primaryKey);

            //assert
            Assert.IsFalse(result);

            //cleanup
            _sut.RemoveCase(_primaryKey, instrumentName, serverParkName);
        }
    }
}
