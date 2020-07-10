//using System;
//using System.Collections.Generic;
//using System.Linq;
//using NUnit.Framework;

//namespace Blaise.Nuget.Api.Tests.Unit.Integration.Factories
//{
//    public class DictionaryTest
//    {
//        private readonly Dictionary<Tuple<string, string>, Tuple<string, DateTime>> _dataLinkConnections;

//        public DictionaryTest()
//        {
//            _dataLinkConnections = new Dictionary<Tuple<string, string>, Tuple<string, DateTime>>();
//        }

//        [Test]
//        public void TestDictionaryWithTupleAsAKey()
//        {
//            //arrange

//            //act && assert
//            var result = GetExistingLink("Instrument1", "ServerPark1");
//            Assert.AreEqual("Fresh!", result);
//            //result = GetExistingLink("Instrument1", "ServerPark1");
//            //Assert.AreEqual("Existing", result);
//            result = GetExistingLink("INSTRUMENT1", "ServerPark1");
//            Assert.AreEqual("Existing", result);
//        }

//        public string GetExistingLink(string instrumentName, string serverParkName)
//        {
//            //if (_dataLinkConnections.Any(c => c.Key.Item1 == instrumentName && c.Key.Item2 == serverParkName))
//            //{
//            //    var existingConnection = _dataLinkConnections
//            //        .First(c => c.Key.Item1 == instrumentName && c.Key.Item2 == serverParkName);

//            //    return "Existing";
//            //}

//            return GetFreshConnection(instrumentName, serverParkName);
//        }

//        private string GetFreshConnection(string instrumentName, string serverParkName)
//        {
//            _dataLinkConnections[new Tuple<string, string>(instrumentName, serverParkName)] = new Tuple<string, DateTime>("fresh1", DateTime.Now);

              

//            return "fresh1";
//        }
//    }
//}
