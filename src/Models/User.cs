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
        public int Wins { get; set; }
        public int Losses { get; set; }

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
            Wins = 0;
            Losses = 0;
        }

        public void BuyPackage()
        {
            if(Coins < 5)
            {
                Console.WriteLine("You only have " + Coins + " coins! You need 5 to buy a package.");
            }
            Coins -= 5;
        }

        public void AddToStack(Package package)
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
        
        public void AddToDeck(Card card)
        {
            _userDeck.AddNewCard(card);
            _battleDeck.AddNewCard(card);
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

        public void FillDeckRandom()
        {
            for(int i = 0; i<4; i++)
            {
                Random rnd = new Random();
                int rand = rnd.Next(0, _userStack.Size);
                Card insert = _userStack.GetCardAt(rand);
                if(insert == null)
                {
                    throw new ArgumentNullException("Null exception in FillDeckRandom()");
                }
                _userDeck.AddNewCard(insert);
                _battleDeck.AddNewCard(insert);

            }
        }

    }
}
