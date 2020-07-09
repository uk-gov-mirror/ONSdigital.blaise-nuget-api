
using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Integration
{
    public class FluentApiUserServiceTests
    {
        private readonly ConnectionModel _connectionModel;
        private readonly List<string> _serverParks;

        public FluentApiUserServiceTests()
        {
            _connectionModel = new ConnectionModel
            {
                Binding = "HTTP",
                UserName = "Root",
                Password = "Root",
                ServerName = "localhost",
                Port = 8031,
                RemotePort = 8033
            };

            _serverParks = new List<string>
            {
                "tel", "val", "ftf"
            };
        }

        //[Ignore("Wont run without app settings on build environment")]
        [Test]
        public void Given_A_User_Does_Not_Exist_When_I_Call_Add_User_The_User_Is_Added()
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act && assert
            var exists = sut.WithConnection(_connectionModel).User.WithUserName("Jamie").Exists;

            Assert.IsFalse(exists);

            sut
                .WithConnection(_connectionModel)
                .User
                .WithUserName("Jamie")
                .WithPassword("T3st")
                .WithRole("TO_Manager")
                .WithServerParks(_serverParks)
                .Add();

            exists = sut.WithConnection(_connectionModel).User.WithUserName("Jamie").Exists;

            Assert.IsTrue(exists);

            sut
                .WithConnection(_connectionModel)
                .User
                .WithUserName("Jamie")
                .Remove();
        }
    }
}
