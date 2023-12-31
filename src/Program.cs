﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using MTCG.Models;
using MTCG.Business_Layer;
using MTCG.Data_Layer;
using MTCG.Routing;

namespace MTCG
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IPAddress ipAdress = IPAddress.Any;
            int port = 10001;
            Router router = new Router(new BattleManager(new UserDao(), new DeckDao()), new CardManager(new CardDao()), new TradeManager(new TradeDao()), new UserManager(new UserDao()), new PackageManager(new PackageDao(), new CardDao(), new UserDao()), new ScoreboardManager(new ScoreboardDao()), new DeckManager(new DeckDao()), new IdRouteParser());
            HttpServer.HttpServer newServer = new HttpServer.HttpServer(ipAdress, port, router);
            await newServer.StartAsync();

            //Max 100 rounds
            //Randomly chooses cards of user
            //ADD A UNIQUE FEATURE (additional booster for 1 round to 1dotnet add package Npgsql --version 7.0.6 card, spells…)
            //Win lose ratio wär cool
            //Defeated cards are removed from loser and added to winner deck in a round, wenn einer keine karten mehr hat OVER

        }
    }
}