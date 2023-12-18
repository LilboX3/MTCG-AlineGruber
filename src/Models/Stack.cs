using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public class Stack
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

        public Card? GetCardAt(int index)
        {
            return UserStack[index];
        }

        public void RemoveCard(Card toRemove)
        {
            if(toRemove == null) return;

            UserStack.Remove(toRemove);
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

        public bool IndexInRange(int index)
        {
            return index >= 0 && index < UserStack.Count;
        }
    }
}
