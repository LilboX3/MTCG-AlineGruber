using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG
{
    internal abstract class Card
    {
        public int Damage { get; set; }
        public string Element { get; set; }
        public string Name { get; set; }
        public Card() { 

        }
    }
}
