using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MTCG.Models;

namespace MTCG.Models
{
    public class SpellCard: Card
    {
        public SpellCard(int damage, Element element): base(damage, element) 
        {
            Name = element.ToString() + "Spell";
        }
    }
}
