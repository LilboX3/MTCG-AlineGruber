using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class Deck
    {
        public List<Card> CardDeck { get; set; }
        public int Size { get; set; }

        public Deck() { 
            CardDeck = new List<Card>();
            Size = 0;
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
            Size++; 

        }

        public void AddOpponentCard(Card card)
        {
            CardDeck.Add(card);
            Size++;
        }
          
        public Card GetRandom()  
        {
            Random rand = new Random();
            int index = rand.Next(0, Size);
            return CardDeck[index];
        }

        public void RemoveCard(string name)
        {

        }

        public override string ToString()
        {
            string DeckString = "";
            for (int i = 0; i < Size; i++)
            {
                DeckString += i.ToString() + ". ";
                DeckString += CardDeck[i].ToString();
                DeckString += "\n";
            }
            return DeckString;
        }


    }
}
