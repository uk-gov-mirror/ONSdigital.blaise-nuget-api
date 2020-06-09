using System;
using Blaise.Nuget.Api.Contracts.Interfaces;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Unit
{
    public class FluentBlaiseApiTests
    {
        [Test]
        public void Given_I_Instantiate_FluentBlaiseApi_No_Exceptions_Are_Thrown()
        {
            //act && assert
            Assert.DoesNotThrow(() => new FluentBlaiseApi());
        }

        [Test]
        public void Given_No_Valid_Step_Taken_When_I_Call_Exists_A_NotSupportedException_Is_Thrown()
        {
            //arrange
            IFluentBlaiseApi sut = new FluentBlaiseApi();

            //act && assert
            var exception = Assert.Throws<NotSupportedException>(() =>
            {
                var exists = sut.Exists;
            });

            Assert.AreEqual("You have not declared a step previously where this action is supported", exception.Message);
        }
    }
}
