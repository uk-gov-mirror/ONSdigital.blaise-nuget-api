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
        public void Given_Valid_Arguments_When_I_Call_Backup_Then_A_Survey_Is_Backed_Up()
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
    }
}
