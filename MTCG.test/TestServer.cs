﻿using MTCG.Business_Layer;
using MTCG.Data_Layer;
using MTCG.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.test
{
    [TestFixture]
    internal class TestServer
    {
        [Test]
        public void TestServerNotNull()
        {
            Router router = new Router(new BattleManager(new UserDao(), new DeckDao()), new CardManager(new CardDao()), new TradeManager(new TradeDao()), new UserManager(new UserDao()), new PackageManager(new PackageDao(), new CardDao(), new UserDao()), new ScoreboardManager(new ScoreboardDao()), new DeckManager(new DeckDao()), new IdRouteParser());
            var server = new HttpServer.HttpServer(IPAddress.Any, 10001, router);
            Assert.IsNotNull(server);
        }

        [Test]
        public void TestServerPort()
        {
            Router router = new Router(new BattleManager(new UserDao(), new DeckDao()), new CardManager(new CardDao()), new TradeManager(new TradeDao()), new UserManager(new UserDao()), new PackageManager(new PackageDao(), new CardDao(), new UserDao()), new ScoreboardManager(new ScoreboardDao()), new DeckManager(new DeckDao()), new IdRouteParser());
            var server = new HttpServer.HttpServer(IPAddress.Any, 5432, router);
            Assert.That(server.GetPort(), Is.EqualTo(5432));
        }
       
    }
}
