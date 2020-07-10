﻿using System.Collections.Generic;
using System.Linq;
using Blaise.Nuget.Api.Contracts.Models;
using Blaise.Nuget.Api.Core.Factories;
using Blaise.Nuget.Api.Core.Services;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Integration.Factories
{
    public class ConnectedServerFactoryTests
    {
        /// <summary>
        /// Please ensure the app.config in the test project has values that relate to local environment
        /// </summary>

        [Ignore("Wont run without app settings on build environment")]
        [Test]
        public void Given_I_Call_GetServerParkNames_I_Get_The_Expected_Values_Back()
        {
            //arrange
            var blaiseApi = new BlaiseApi();

            //act
            var result = blaiseApi.GetServerParkNames(blaiseApi.GetDefaultConnectionModel()).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(1, result.Count);
            Assert.True(result.Contains("LocalDevelopment"));
        }


        [Ignore("Wont run without app settings on build environment")]
        [Test]
        public void Given_I_Call_UseConnection_To_Specify_A_Server_When_I_Call_GetServerParkNames_I_Get_The_Expected_Values_Back()
        {
            //arrange
            var blaiseApi = new BlaiseApi();
            var connectionModel = blaiseApi.GetDefaultConnectionModel();
            connectionModel.ServerName = "localhost";

            //act
            var result = blaiseApi.GetServerParkNames(connectionModel).ToList();

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IEnumerable<string>>(result);
            Assert.AreEqual(1, result.Count);
            Assert.True(result.Contains("LocalDevelopment"));
        }

        [Ignore("Wont run without app settings on build environment")]
        [Test]
        public void Given_I_Call_GetConnection_With_A_ConnectionModel_I_Get_A_Connection_Back()
        {
            //arrange
            var sut = new ConnectedServerFactory(new PasswordService());

            var connectionModel = new ConnectionModel
            {
                Binding = "HTTP",
                UserName = "Root",
                Password = "Root",
                ServerName = "localhost",
                Port = 8031,
                RemotePort = 8033,
                ConnectionExpiresInMinutes = 1
            };


            //act
            var result = sut.GetConnection(connectionModel);

            //assert
            Assert.IsNotNull(result);
        }

        [Ignore("Wont run without app settings on build environment")]
        [Test]
        public void Given_I_Call_GetConnection_For_An_Expired_Connection_I_Get_A_Connection_Back()
        {
            //arrange
            var sut = new ConnectedServerFactory(new PasswordService());

            var connectionModel = new ConnectionModel
            {
                Binding = "HTTP",
                UserName = "Root",
                Password = "Root",
                ServerName = "localhost",
                Port = 8031,
                RemotePort = 8033,
                ConnectionExpiresInMinutes = -1
            };
            
            //act &&  assert
            var result = sut.GetConnection(connectionModel);
            Assert.IsNotNull(result);

            result = sut.GetConnection(connectionModel);
            Assert.IsNotNull(result);
        }

        [Ignore("Wont run without app settings on build environment")]
        [Test]
        public void Given_I_Call_GetConnection_With_The_Same_Connection_Model_Multiple_Times_I_Get_A_Connection_Back()
        {
            //arrange
            var sut = new ConnectedServerFactory(new PasswordService());

            var firstConnectionModel = new ConnectionModel
            {
                Binding = "HTTP",
                UserName = "Root",
                Password = "Root",
                ServerName = "localhost",
                Port = 8031,
                RemotePort = 8033,
                ConnectionExpiresInMinutes = 2
            };

            var secondConnectionModel = new ConnectionModel
            {
                Binding = "HTTP",
                UserName = "Root",
                Password = "Root",
                ServerName = "LOCALHOST",
                Port = 8031,
                RemotePort = 8033,
                ConnectionExpiresInMinutes = 2
            };

            //act &&  assert
            var result = sut.GetConnection(firstConnectionModel);
            Assert.IsNotNull(result);
            result = sut.GetConnection(firstConnectionModel);
            Assert.IsNotNull(result);

            result = sut.GetConnection(secondConnectionModel);
            Assert.IsNotNull(result);
            result = sut.GetConnection(secondConnectionModel);
            Assert.IsNotNull(result);
        }
    }
}
