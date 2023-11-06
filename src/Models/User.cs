﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class User
    {
        public int Coins { get; set; }
        public Credentials UserCredentials { get; set; }
        public int EloValue { get; set; }
        public Card? CurrentCard { get; set; }

        private Deck _userDeck;
        //temp Deck for battles, to lose and win cards
        private Deck _battleDeck;
        private Stack _userStack;
            
        
        public User(string Username, string Password) {
            Coins = 20;
            UserCredentials = new Credentials(Username, Password);
            EloValue = 100;
            _userDeck = new Deck();
            _userStack = new Stack();
            _battleDeck = new Deck();
            CurrentCard = null;
        }

        public void BuyPackage()
        {
            Package Bought = new Package();
            Coins -= 5;
            AddToStack(Bought);
        }

        private void AddToStack(Package package)
        {
            Card[] cards = package.Cards;
            for(int i=0; i<cards.Length; i++)
            {
                _userStack.AddCard(cards[i]);
            }
        }

        public void LoseElo()
        {
            EloValue -= 5;
        }

        public void WinElo()
        {
            EloValue += 3;
        }

        public Card PlayCard()
        {
            CurrentCard = _userDeck.GetRandom();
            return CurrentCard;
        }

        public Card LoseCard()
        {
            if(CurrentCard == null)
            {
                throw new Exception("Current Card of player is null!");
            }
            Card temp = CurrentCard;
            CurrentCard = null;
            return temp;
        }
        
        public void ChooseDeck()
        {
            if (_userStack.Size == 0)
            {
                Console.WriteLine("Your stack is empty. Consider buying a package!");
                return;
            }

            if (_userDeck.Size == 4)
            {
                Console.WriteLine("Deck full: you already have the maximum of 4 cards, remove one to make space");
                return;
            }

            Console.WriteLine("Your current Stack: ");
            Console.WriteLine(_userStack.ToString());
            Console.WriteLine("Choose the number of card you want to add to your deck: ");
            
            string choice = Console.ReadLine();

            if (IsNumber(choice))
            {
                int index = int.Parse(choice);
                if (_userStack.IndexInRange(index)) {

                    _userDeck.AddNewCard(_userStack.UserStack[index]);
                    _battleDeck.AddNewCard(_userStack.UserStack[index]);

                    _userStack.RemoveCard(_userStack.UserStack[index]);
                    Console.WriteLine("Current deck: \n"+_userDeck.ToString());

                } else
                {
                    Console.WriteLine("No card chosen: invalid index");
                    return;
                }
            } else
            {
                Console.WriteLine("No card chosen: invalid input");
                return;
            }
            
        }

        public void RemoveFromDeck()
        {
            if (_userDeck.Size <= 0)
            {
                Console.WriteLine("Your deck is empty! Buy a package and add cards from your stack");
                return;
            }

            Console.WriteLine("Choose a card you want to remove from your deck");
            Console.WriteLine(_userDeck.ToString());
            string choice = Console.ReadLine();
            if (IsNumber(choice))
            {
                int index = int.Parse(choice);
                if (_userDeck.IndexInRange(index))
                {
                    Card toRemove = _userDeck.CardDeck[index];
                    _userDeck.RemoveCard(toRemove);
                    _battleDeck.RemoveCard(toRemove);
                    _userStack.AddCard(toRemove);
                    Console.WriteLine("Current deck: \n" + _userDeck.ToString());
                    Console.WriteLine("Current stack: \n" + _userStack.ToString());

                }
                else
                {
                    Console.WriteLine("No card chosen: invalid index");
                    return;
                }
            }
            else
            {
                Console.WriteLine("No card chosen: invalid input");
                return;
            }

        }

        private static bool IsNumber(string text)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.IsMatch(text);
        }


    }
}
