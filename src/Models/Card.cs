using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public abstract class Card
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

        //Water > fire, fire > normal, normal > water
        public int CalcDamageAgainst(Element opponentElement)
        {
            int damage = this.Damage;
            //Stronger element
            if(this.ElementType == Element.Water && opponentElement == Element.Fire)
            {
                damage *= 2;
            }
            else if(this.ElementType == Element.Fire && opponentElement == Element.Regular)
            {
                damage *= 2;
            }
            else if(this.ElementType == Element.Regular && opponentElement == Element.Water)
            {
                damage *= 2;
            }

            //Weaker element
            else if(this.ElementType == Element.Fire && opponentElement == Element.Water)
            {
                damage /= 2;
            }
            else if(this.ElementType == Element.Regular && opponentElement == Element.Fire)
            {
                damage /= 2;
            }
            else if(this.ElementType == Element.Water && opponentElement == Element.Regular)
            {
                damage /= 2;
            }

            //Unchanged if both elements are the same
            return damage;
        }
        
    }
}
