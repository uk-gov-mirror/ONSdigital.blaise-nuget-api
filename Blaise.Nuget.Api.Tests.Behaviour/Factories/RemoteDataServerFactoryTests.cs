using Blaise.Nuget.Api.Contracts.Models;
using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Tests.Behaviour.Factories
{
    public class RemoteDataServerFactoryTests
    {
        private readonly ConnectionModel _connectionModel;

        public RemoteDataServerFactoryTests()
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
        }

        [Test]
        public void Given_I_Call_GetServerParkNames_I_Get_The_Expected_Values_Back()
        {
            //arrange
            var instrumentName = "OPN2004A";
            var serverPark = "LocalDevelopment";

            var blaiseApi = new BlaiseApi();

            //act
            var result = blaiseApi.GetDataSet(_connectionModel, instrumentName, serverPark);

            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<IDataSet>(result);
        }
    }
}
