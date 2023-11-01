using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class Deck
    {
        public List<Card> CardDeck { get; set; }
        private int _size;

        public Deck() { 
            CardDeck = new List<Card>();
            _size = 0;
        }

        public void AddNewCard(Card card)
        {
            if(CardDeck.Count > 4)
            {
                //some kinda error?
                Console.WriteLine("Only 4 cards can be added to the starting deck!");
                return;
            }
            CardDeck.Add(card);
            _size++; 

        }

        public void AddOpponentCard(Card card)
        {
            CardDeck.Add(card);
            _size++;
        }

        public Card GetRandom()
        {
            Random rand = new Random();
            int index = rand.Next(0, _size);
            return CardDeck[index];
        }


    }
}
