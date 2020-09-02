using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Backup
{
    public class FluentApiBackupTests
    {
        private readonly ConnectionModel _connectionModel;

        public FluentApiBackupTests()
        {
            _connectionModel = new ConnectionModel
            {
                Binding = "HTTP",
                UserName = "Root",
                Password = "Root",
                ServerName = "localhost",
                Port = 8031,
                RemotePort = 8033,
                ConnectionExpiresInMinutes = 1
            };
        }

        [Ignore("")]
        [Test]
        public void Given_I_Want_To_Backup_To_A_File_Location_When_I_Call_Backup_Then_A_Survey_Is_Backed_Up()
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act
            sut
                .WithConnection(_connectionModel)
                .WithServerPark("LocalDevelopment")
                .WithInstrument("OPN2004A")
                .Survey
                .ToPath(@"D:\Temp\OPN\Backup")
                .Backup();

            //assert
        }

        [Ignore("")]
        [Test]
        public void Given_I_Want_To_Backup_To_A_Bucket_When_I_Call_Backup_Then_A_Survey_Is_Backed_Up()
        {
            //arrange
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Users\Jamie\source\Keys\ons-blaise-dev-93d017495c0a.json");

            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act && assert

            Assert.DoesNotThrow( () =>
            sut
                .WithConnection(_connectionModel)
                .WithServerPark("LocalDevelopment")
                .WithInstrument("OPN2004A")
                .Survey
                .ToBucket(@"ons-blaise-dev-case-backup")
                .Backup()
            );
        }

        [Ignore("")]
        [Test]
        public void Given_I_Want_To_Backup_To_A_Folder_In_A_Bucket_When_I_Call_Backup_Then_A_Survey_Is_Backed_Up()
        {
            //arrange
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"C:\Users\Jamie\source\Keys\ons-blaise-dev-93d017495c0a.json");

            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act && assert

            Assert.DoesNotThrow(() =>
                sut
                    .WithConnection(_connectionModel)
                    .WithServerPark("LocalDevelopment")
                    .WithInstrument("OPN2004A")
                    .Survey
                    .ToBucket(@"ons-blaise-dev-case-backup")
                    .ToPath("test")
                    .Backup()
            );
        }
    }
}
