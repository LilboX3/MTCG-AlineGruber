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
        

    }
}
