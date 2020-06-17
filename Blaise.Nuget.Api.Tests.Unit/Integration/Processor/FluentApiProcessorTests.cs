using NUnit.Framework;
using StatNeth.Blaise.API.DataLink;

namespace Blaise.Nuget.Api.Tests.Unit.Integration.Processor
{
    public class FluentApiProcessorTests
    {
        [Ignore("Wont run without app settings on build environment")]
        [TestCase(@"D:\Temp\Processor\32bit\OPN2004A.bdix")]
        [TestCase(@"D:\Temp\Processor\64bit\OPN2004A.bdix")]
        public void Given_A_Bdix_File_When_I_Call_GetDataLink_A_DataLink_Is_Returned(string file)
        {
            //act

            var dataLink = DataLinkManager.GetDataLink(file);

            //assert
            Assert.NotNull(dataLink);
        }
    }
}
