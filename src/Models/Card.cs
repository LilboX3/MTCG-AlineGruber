using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    public abstract class Card
    {
        public float Damage { get; set; }
        public string Name { get; set; }  
        public string Id { get; set; }
        public Element ElementType { get; set; }

        public Card(float damage, Element element, string id)
        {
            Damage = damage;
            ElementType = element;
            Name = "";
            Id = id;
        }

        public override string ToString()
        {
            string CardString = Name + ", damage:" + Damage;
            return CardString;
        }

        //Water > fire, fire > normal, normal > water
        public float CalcDamageAgainst(Element opponentElement)
        {
            float damage = this.Damage;
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
