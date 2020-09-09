using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Handler
{
    public class FluentApiHandlerUnderLoadTests
    {
        private readonly ConnectionModel _connectionModel;
        private readonly Dictionary<string, string> _payload;
        private readonly string _primaryKey;

        public FluentApiHandlerUnderLoadTests()
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

            _payload = new Dictionary<string, string>
            {
                {"QID.CaseID", "54"},
                {"QID.Case_ID", "54"},
                {"QID.Quota", null},
                {"QID.Address", "4"},
                {"QID.HHold", null},
                {"QID.Issue", "1"},
                {"QID.Mode", "TEL"},
            };

            _primaryKey = "90000666";
        }

        [TestCase(100)]
        public void Given_I_Use_Create_Then_Delete_A_Case_X_Times_It_Handles_Successfully(int numberOfTimes)
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            for (var i = 0; i < numberOfTimes; i++)
            {
                //act
                var exists = sut
                    .WithConnection(_connectionModel)
                    .WithInstrument("OPN2004A")
                    .WithServerPark("LocalDevelopment")
                    .Case
                    .WithPrimaryKey(_primaryKey)
                    .Exists;

                //assert
                Assert.False(exists);

                sut
                    .WithConnection(_connectionModel)
                    .WithInstrument("OPN2004A")
                    .WithServerPark("LocalDevelopment")
                    .Case
                    .WithPrimaryKey(_primaryKey)
                    .WithData(_payload)
                    .Add();

                exists = sut
                    .WithConnection(_connectionModel)
                    .WithInstrument("OPN2004A")
                    .WithServerPark("LocalDevelopment")
                    .Case
                    .WithPrimaryKey(_primaryKey)
                    .Exists;

                //assert
                Assert.True(exists);

                //cleanup
                sut
                    .WithConnection(_connectionModel)
                    .WithInstrument("OPN2004A")
                    .WithServerPark("LocalDevelopment")
                    .Case
                    .WithPrimaryKey(_primaryKey)
                    .Remove();
            }
        }
    }
}
