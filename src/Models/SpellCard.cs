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
        public SpellCard(int damage, string id, Element element): base(damage, element, id) 
        {
            Name = element.ToString() + "Spell";
        }

    }
}
