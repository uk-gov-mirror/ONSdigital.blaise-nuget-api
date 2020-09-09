using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Handler
{
    public class FluentApiToFileHandlerTests
    {
        [Ignore("")]
        [Test]
        public void Given_Valid_Arguments_For_File_When_I_Call_Copy_Then_A_Case_Is_Copied_To_A_File()
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            var connectionModel = new ConnectionModel
            {
                Binding = "",
                UserName = "",
                Password = "",
                ServerName = "",
                Port = 0,
                RemotePort = 0,
                ConnectionExpiresInMinutes = 0
            };

            //act
            sut
                .WithConnection(connectionModel)
                .WithServerPark("LocalDevelopment")
                .WithInstrument("OPN2004A")
                .Case
                .WithPrimaryKey("11000141")
                .Copy
                .ToFile(@"d:\temp\OPN\Handler")
                .ToInstrument("OPN2004A")
                .Handle();

            //assert
        }

        [Ignore("Wont run without library directory being set in app settings")]
        [Test]
        public void Given_Valid_Arguments_For_File_When_I_Call_Move_Then_A_Case_Is_Moved_To_A_File()
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            var connectionModel = new ConnectionModel
            {
                Binding = "",
                UserName = "",
                Password = "",
                ServerName = "",
                Port = 0,
                RemotePort = 0,
                ConnectionExpiresInMinutes = 0
            };

            //act
            sut
                .WithConnection(connectionModel)
                .WithServerPark("LocalDevelopment")
                .WithInstrument("OPN2004A")
                .Case
                .WithPrimaryKey("11000131")
                .Move
                .ToFile(@"d:\temp\OPN\Handler")
                .ToInstrument("OPN2004A")
                .Handle();

            //assert
        }
    }
}
