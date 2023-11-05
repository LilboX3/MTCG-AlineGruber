using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal abstract class Card
    {
        public int Damage { get; set; }
        public string Name { get; set; }
        public Element ElementType { get; set; }
        public Card(int damage, Element element)
        {
            Damage = damage;
            ElementType = element;
            Name = "";
        }

        public override string ToString()
        {
            string CardString = Name + ", damage:" + Damage;
            return CardString;
        }
        
    }
}
