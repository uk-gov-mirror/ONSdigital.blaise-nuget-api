using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Enums;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Fields
{
    public class ApiFieldTests
    {
        private readonly ConnectionModel _connectionModel;

        public ApiFieldTests()
        {
            _connectionModel = new ConnectionModel
            {
                Binding = "HTTP",
                UserName = "Root",
                Password = "Root",
                ServerName = "localhost",
                Port = 8031,
                RemotePort = 8033,
                ConnectionExpiresInMinutes = 30
            };
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        public void Given_A_Case_That_Exists_With_A_Mode_Value_When_I_Call_GetFieldValue_Then_The_Expected_Value_Is_Returned(int mode)
        {
            //arrange

            var primaryKey = "99000001";
            var instrumentName = "OPN2004A";
            var serverPark = "LocalDevelopment";

            IBlaiseApi sut = new BlaiseApi();

            var payload = new Dictionary<string, string>
            {
                { "Mode", $"{mode}" }
            };

            sut.CreateNewDataRecord(_connectionModel, primaryKey, payload, instrumentName, serverPark);

            //act
            var dataRecord = sut.GetDataRecord(_connectionModel, primaryKey, instrumentName, serverPark);
            var result = sut.GetFieldValue(dataRecord, FieldNameType.Mode).EnumerationValue;

            //assert
            Assert.AreEqual(mode, result);

            //cleanup
            sut.RemoveCase(_connectionModel, primaryKey, instrumentName, serverPark);
        }

        [Test]
        public void Given_A_Case_That_Exists_Without_An_Mode_Value_When_I_Call_GetFieldValue_Then_The_Expected_Value_Is_Returned()
        {
            //arrange
            var primaryKey = "99000001";
            var instrumentName = "OPN2004A";
            var serverPark = "LocalDevelopment";

            IBlaiseApi sut = new BlaiseApi();
            var payload = new Dictionary<string, string>
            {
                {"QID.Case_ID", "1"}
            };

            sut.CreateNewDataRecord(_connectionModel, primaryKey, payload, instrumentName, serverPark);

            //act
            var dataRecord = sut.GetDataRecord(_connectionModel, primaryKey, instrumentName, serverPark);
            
            var result = sut.GetFieldValue(dataRecord, FieldNameType.Mode).EnumerationValue;

            //assert
            Assert.AreEqual(0, result);

            //cleanup
            sut.RemoveCase(_connectionModel, primaryKey, instrumentName, serverPark);

        }
    }
}
