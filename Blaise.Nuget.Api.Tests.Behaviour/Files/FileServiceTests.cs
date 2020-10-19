using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blaise.Nuget.Api.Contracts.Interfaces;
using Blaise.Nuget.Api.Core.Services;
using Blaise.Nuget.Api.Providers;
using NUnit.Framework;

namespace Blaise.Nuget.Api.Tests.Behaviour.Files
{
    public class FileServiceTests
    {
        //[Ignore("")]
        [TestCase("C:\\Blaise5\\Settings")]
        [TestCase("c:\\blaise5\\settings")]
        [TestCase("C:\\BLAISE5\\SETTINGS")]
        public void Given_I_Want_To_Retrieve_A_List_Of_Files_The_Method_Is_Case_Insensitive(string filePath)
        {
            //arrange

            var sut = new FileService(new ConfigurationProvider());

            //act
            var result = sut.GetFiles(filePath);

            //assert
            Assert.AreEqual(6, result.Count());
        }
    }
}
