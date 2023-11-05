using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class Stack
    {
        public List<Card> UserStack { get; set; }
        public int Size { get; set; }
        public Stack() {
            UserStack = new List<Card>();
        }

        public Card? GetCard(string Name)
        {
            foreach(Card card in UserStack)
            {
                if(card.Name == Name) return card;
            }
            return null;
        }

        public void RemoveCard()
        {

            Size--;
        }

        public void AddCard(Card card)
        {
            UserStack.Add(card);
            Size++;
        }

        public override string ToString()
        {
            string StackString = "";
            for(int i = 0; i < Size; i++)
            {
                StackString += i.ToString()+". ";
                StackString += UserStack[i].ToString();
                StackString += "\n";
            }
            return StackString;
        }
    }
}
