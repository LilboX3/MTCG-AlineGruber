using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    internal class Deck
    {
        public List<Card> CardDeck { get; set; }

        public Deck() { 
            CardDeck = new List<Card>();
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

        }

        public void AddOpponentCard(Card card)
        {

        }


    }
}
