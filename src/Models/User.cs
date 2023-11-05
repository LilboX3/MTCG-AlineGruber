using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class User
    {
        public int Coins { get; set; }
        public Credentials UserCredentials { get; set; }
        public int EloValue { get; set; }
        public Card? CurrentCard { get; set; }

        private Deck _userDeck;
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
            Console.WriteLine("Your current Stack: ");
            Console.WriteLine(_userStack.ToString());
            Console.WriteLine("Choose the number of card you want to add to your deck: ");
            
            string choice = Console.ReadLine();
            if (IsNumber(choice))
            {

            } else
            {

            }
            
        }

        private bool IsNumber(string text)
        {
            Regex regex = new Regex(@"^\d+$");
            return regex.IsMatch(text);
        }




    }
}
