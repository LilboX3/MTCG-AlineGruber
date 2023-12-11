using System;
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
            if(Coins < 5)
            {
                Console.WriteLine("You only have " + Coins + " coins! You need 5 to buy a package.");
            }
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
            _battleDeck = _userDeck;
        }

        public void WinElo()
        {
            EloValue += 3;
            _battleDeck = _userDeck;
        }

        public Card PlayCard()
        {
            CurrentCard = _battleDeck.GetRandom();
            return CurrentCard;
        }

        public Card LoseCard()
        {
            if(CurrentCard == null)
            {
                throw new ArgumentNullException("Current Card of player is null!");
            }

            Card temp = CurrentCard;
            _battleDeck.RemoveCard(temp);
            CurrentCard = null;
            return temp;
        }

        public void WinCard(Card wonCard)
        {
            if(wonCard == null)
            {
                throw new ArgumentNullException("Won card is null!");
            }
            _battleDeck.AddOpponentCard(wonCard);
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
            PrintStack();
            Console.WriteLine("Choose the number of card you want to add to your deck: ");
            
            string choice = Console.ReadLine();

            if (IsNumber(choice))
            {
                int index = int.Parse(choice);
                if (_userStack.IndexInRange(index)) {

                    _userDeck.AddNewCard(_userStack.UserStack[index]);
                    _battleDeck.AddNewCard(_userStack.UserStack[index]);

                    _userStack.RemoveCard(_userStack.UserStack[index]);
                    Console.WriteLine("Current deck:");
                    PrintDeck();

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
            PrintDeck();
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

                    Console.WriteLine("Current deck:");
                    PrintDeck();
                    Console.WriteLine("Current stack:");
                    PrintStack();
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

        public void PrintDeck()
        {
            Console.WriteLine(_userDeck.ToString());
        }

        public void PrintStack()
        {
            Console.WriteLine(_userStack.ToString());
        }

        public void PrintBattleDeck()
        {
            Console.WriteLine(_battleDeck.ToString());
        }

        public bool DeckIsEmpty()
        {
            if(_battleDeck.Size == 0)
            {
                return true;
            }
            return false;
        }
    }
}
