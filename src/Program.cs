﻿using System;
using MTCG.Models;
namespace MTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {

            User Player1 = new User("Lalong", "123");
            Player1.BuyPackage();
            Player1.ChooseDeck();
            Player1.ChooseDeck();
            Player1.ChooseDeck();
            Player1.RemoveFromDeck();
            //Max 100 rounds
            //Randomly chooses cards of user
            //ADD A UNIQUE FEATURE (additional booster for 1 round to 1dotnet add package Npgsql --version 7.0.6 card, spells…)
            //Win lose ratio wär cool
            //Defeated cards are removed from loser and added to winner deck in a round, wenn einer keine karten mehr hat OVER

        }
    }
}