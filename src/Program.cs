using System;
namespace MTCG
{
    internal class Program
    {
        static void Main(string[] args)
        {
            User player1 = new User("Lalong");
            User player2 = new User("Goon");
            Console.WriteLine($"Welcome back, {player1.Name}!");
            //Max 100 rounds
            //Randomly chooses cards of user
            //ADD A UNIQUE FEATURE (additional booster for 1 round to 1dotnet add package Npgsql --version 7.0.6 card, spells…)
            //Win lose ratio wär cool
            //Defeated cards are removed from loser and added to winner deck in a round, wenn einer keine karten mehr hat OVER

        }
    }
}