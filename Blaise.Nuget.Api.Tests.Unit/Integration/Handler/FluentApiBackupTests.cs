using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit.Integration.Handler
{
    public class FluentApiBackupTests
    {
        [Ignore("Wont run without library directory being set in app settings")]
        [Test]
        public void Given_Valid_Arguments_When_I_Call_Backup_Then_A_Survey_Is_Backed_Up()
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            var connectionModel = new ConnectionModel
            {
                Binding = "HTTP",
                UserName = "Root",
                Password = "Root",
                ServerName = "localhost",
                Port = 8031,
                RemotePort = 8033
            };

            //act
            sut
                .WithConnection(connectionModel)
                .WithServerPark("LocalDevelopment")
                .WithInstrument("OPN2004A")
                .WithFile(@"D:\Temp\OPN\Backup")
                .Survey
                .Backup();

            //assert
        }
    }
}
