using System.Collections.Generic;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.User
{
    public class FluentApiUserTests
    {
        private readonly ConnectionModel _connectionModel;

        private readonly string _userName;

        private readonly string _password;

        private readonly string _role;

        private List<string> _serverParks;

        public FluentApiUserTests()
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

            _userName = "jamiekerr";
            _password = "J@mieK3rr";
            _role = "DST_TECH";
            _serverParks = new List<string>();
        }


        [Test]
        public void Given_Valid_User_Details_When_I_Call_AddUser_A_User_Is_Added_To_Blaise()
        {
            //arrange
            _serverParks = new List<string>
            {
                "LocalDevelopment"
			};


			IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act
            sut
                .WithConnection(_connectionModel)
                .User
                .WithUserName(_userName)
                .WithPassword(_password)
                .WithRole(_role)
                .WithServerParks(_serverParks)
                .WithDefaultServerPark("LocalDevelopment")
                .Add();

			var result = sut
                .WithConnection(_connectionModel)
                .User
                .WithUserName(_userName)
                .Exists;


			//assert
			Assert.True(result);

            sut
                .WithConnection(_connectionModel)
                .User
                .WithUserName(_userName)
                .Remove();
        }

        [Test]
        public void Given_No_ServerParks_Specified_When_I_Call_AddUser_A_User_Is_Added_To_Blaise()
        {
            //arrange
            _serverParks = new List<string>();

            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act
            sut
                .WithConnection(_connectionModel)
                .User
                .WithUserName(_userName)
                .WithPassword(_password)
                .WithRole(_role)
                .WithServerParks(_serverParks)
                .Add();

            var result = sut
                .WithConnection(_connectionModel)
                .User
                .WithUserName(_userName)
                .Exists;


            //assert
            Assert.True(result);

            sut
                .WithConnection(_connectionModel)
                .User
                .WithUserName(_userName)
                .Remove();
        }
    }
}
