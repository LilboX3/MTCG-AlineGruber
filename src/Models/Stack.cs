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
        private int _size;
        public Stack() {
            UserStack = new List<Card>();
        }

        public Card GetCard(string Name)
        {
            
        }

        public void RemoveCard()
        {

            _size--;
        }

        public void AddCard(Card card)
        {
            UserStack.Add(card);
            _size++;
        }

        public override string ToString()
        {
            string StackString = "";
            for(int i = 0; i < _size; i++)
            {
                StackString += i.ToString()+". ";
                StackString += UserStack[i].ToString();
                StackString += "\n";
            }
            return StackString;
        }
    }
}
