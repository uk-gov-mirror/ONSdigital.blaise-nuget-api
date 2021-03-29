using System;
using System.Collections.Generic;
using Blaise.Nuget.Api.Api;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Users
{
    public class UserTests
    {
        private readonly string _userName;
        private readonly string _password;

        private BlaiseUserApi _sut;

        public UserTests()
        {
            _userName = "";
            _password = "";
        }

        [SetUp]
        public void SetUpTests()
        {
            _sut = new BlaiseUserApi();
        }

        [Test]
        public void Given_A_Valid_User_When_I_Call_ValidateUser_Then_True_Is_Returned()
        {
            //act
            var result = _sut.ValidateUser(_userName, _password);

            //assert
            Assert.IsNotNull(result);
            Assert.True(result);
        }

        [Test]
        public void Given_An_Invalid_User_Name_When_I_Call_ValidateUser_Then_False_Is_Returned()
        {
            //act
            var result = _sut.ValidateUser("meh", _password);

            //assert
            Assert.IsNotNull(result);
            Assert.False(result);
        }

        [Test]
        public void Given_An_Invalid_User_Password_When_I_Call_ValidateUser_Then_False_Is_Returned()
        {
            //act
            var result = _sut.ValidateUser(_userName, "meh");

            //assert
            Assert.IsNotNull(result);
            Assert.False(result);
        }
        
        [Test]
        public void Given_An_Invalid_User_When_I_Call_ValidateUser_Then_False_Is_Returned()
        {

            //act
            var result = _sut.ValidateUser("meh", "meh");

            //assert
            Assert.IsNotNull(result);
            Assert.False(result);
        }
    }
}
