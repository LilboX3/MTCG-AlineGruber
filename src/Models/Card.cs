using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models
{
    internal class Card
    {
        public int Damage { get; set; }
        public string Name { get; set; }

        public Card(int damage, string name)
        {
            Damage = damage;
            Name = name;
        }
        
    }
}
